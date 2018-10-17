import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()
export class TokenInterceptor implements HttpInterceptor{
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>{
    if(localStorage.getItem('access_token')){
      const r = req.clone();
      r.headers.append("Authorization", `Bearer ${localStorage.getItem('access_token')}`);
      r.headers.append("Content-Type","application/json");
      return next.handle(r);
    }
    return next.handle(req);
  }
}
