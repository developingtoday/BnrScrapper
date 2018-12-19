import { DataSource } from "@angular/cdk/collections";
import { MatPaginator, MatSort } from "@angular/material";
import { map, switchMap, withLatestFrom } from "rxjs/operators";
import { Observable, of as observableOf, merge, empty, of } from "rxjs";
import { Loan } from "../shared/models/loan.model";
import { BackendService } from "../shared/backend.service";
import { AuthService } from "../shared/Auth.service";

/**
 * Data source for the Loan view. This class should
 * encapsulate all logic for fetching and manipulating the displayed data
 * (including sorting, pagination, and filtering).
 */
export class LoanDataSource extends DataSource<Loan> {
  data: Loan[]=[];
  user$: Observable<any>;

  constructor(
    private paginator: MatPaginator,
    private sort: MatSort,
    private service: BackendService,
    private auth: AuthService
  ) {
    super();
    this.user$ = this.auth.userProfileSubject;
  }

  /**
   * Connect this data source to the table. The table will only update when
   * the returned stream emits new items.
   * @returns A stream of the items to be rendered.
   */
  connect(): Observable<Loan[]> {
    // Combine everything that affects the rendered data into one update
    // stream for the data-table to consume.



    const dataMutations = [
      of(1).pipe(
        withLatestFrom(this.user$,(f1, a) => a),
        switchMap(m => this.service.getLoans(m.email))
        ),
      this.paginator.page,
      this.sort.sortChange
    ];

    // Set the paginators length
    this.paginator.length = this.data.length;

    return merge(...dataMutations).pipe(
      map(x => {
        this.data=x;
        return this.getPagedData(this.getSortedData([...this.data]));
      })
    );
  }

  /**
   *  Called when the table is being destroyed. Use this function, to clean up
   * any open connections or free any held resources that were set up during connect.
   */
  disconnect() {}

  /**
   * Paginate the data (client-side). If you're using server-side pagination,
   * this would be replaced by requesting the appropriate data from the server.
   */
  private getPagedData(data: Loan[]) {
    const startIndex = this.paginator.pageIndex * this.paginator.pageSize;
    return data.splice(startIndex, this.paginator.pageSize);
  }

  /**
   * Sort the data (client-side). If you're using server-side sorting,
   * this would be replaced by requesting the appropriate data from the server.
   */
  private getSortedData(data: Loan[]) {
    if (!this.sort.active || this.sort.direction === "") {
      return data;
    }

    return data.sort((a, b) => {
      const isAsc = this.sort.direction === "asc";
      switch (this.sort.active) {
        case "name":
          return compare(a.bankRate, b.bankRate, isAsc);
        case "id":
          return compare(+a.id, +b.id, isAsc);
        default:
          return 0;
      }
    });
  }
}

/** Simple sort comparator for example ID/Name columns (for client-side sorting). */
function compare(a, b, isAsc) {
  return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}
