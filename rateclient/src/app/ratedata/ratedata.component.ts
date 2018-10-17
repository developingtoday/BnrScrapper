import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { RatedataDataSource } from './ratedata-datasource';
import { BackendService } from '../shared/backend.service';

@Component({
  selector: 'app-ratedata',
  templateUrl: './ratedata.component.html',
  styleUrls: ['./ratedata.component.css']
})
export class RatedataComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  dataSource: RatedataDataSource;

  /** Columns displayed in the table. Columns IDs can be added, removed, or reordered. */
  displayedColumns = ['data', 'robor3M','robor6M'];
 constructor(private service:BackendService){

 }
  ngOnInit() {
    this.dataSource = new RatedataDataSource(this.paginator, this.sort,this.service);

  }
}
