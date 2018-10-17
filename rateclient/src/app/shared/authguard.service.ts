import { Injectable } from "@angular/core";
import {
  CanActivate,
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot
} from "@angular/router";
import { AuthService } from "./Auth.service";
import { Observable } from "rxjs";

@Injectable()
export class AuthguardService implements CanActivate {
  constructor(private auth: AuthService,private router:Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    if(!this.auth.isAuthenticated()) {
      localStorage.setItem('authRedirect',state.url);
      this.auth.login();
      return false;
    }
    return true;
  }
}
