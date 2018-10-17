import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { RoborHistoric } from "./models/roborhistoric.model";
import { environment } from "src/environments/environment";
import { catchError } from "rxjs/operators";

@Injectable()
export class BackendService {
  constructor(private http: HttpClient) {}

  getRobors(from: string, to: string): Observable<RoborHistoric[]> {
    return this.http.get<RoborHistoric[]>(
      `${environment.url}/Rate?begin=${from}&end=${to}`
    );
  }
}
