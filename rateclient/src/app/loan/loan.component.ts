import { Component, OnInit, ViewChild } from "@angular/core";
import { MatPaginator, MatSort } from "@angular/material";
import { LoanDataSource } from "./loan-datasource";
import { BackendService } from "../shared/backend.service";
import { AuthService } from "../shared/Auth.service";

@Component({
  selector: "app-loan",
  templateUrl: "./loan.component.html",
  styleUrls: ["./loan.component.scss"]
})
export class LoanComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  dataSource: LoanDataSource;

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ["name","rateofDatePayment", "bankRate"];

  constructor(
    private backendService: BackendService,
    private auth: AuthService
  ){

  }

  ngOnInit() {
    this.dataSource = new LoanDataSource(
      this.paginator,
      this.sort,
      this.backendService,
      this.auth
    );
  }
}
