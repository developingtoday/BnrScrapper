import { Injectable } from "@angular/core";
import { Router, Route } from "@angular/router";
import { filter, mergeMap } from "rxjs/operators";
import * as auth0 from "auth0-js";
import { Subject, Observable, timer, of, BehaviorSubject } from "rxjs";

(window as any).global = window;

@Injectable()
export class AuthService {
  auth0 = new auth0.WebAuth({
    clientID: "1DgxLrxeSeDL5mC81k8Af61s4Ko3okIq",
    domain: "developingtoday.eu.auth0.com",
    responseType: "token id_token",
    redirectUri: "http://localhost:4200/callback",
    scope: "openid email profile"
  });

  private userProfile$: BehaviorSubject<any>;
  private refreshSub:any;

  get userProfileSubject(): Subject<any> {
    return this.userProfile$;
}

  constructor(private router: Router) {
    this.userProfile$ = new BehaviorSubject(JSON.parse(localStorage.getItem("profile")));

  }



  public login(): void {
    this.auth0.authorize();
  }

  public handleAuthentication(): void {
    this.auth0.parseHash((err, authResult) => {
      if (authResult && authResult.accessToken && authResult.idToken) {
        window.location.hash = "";
        this.setSession(authResult);
        this.getProfile(authResult);
      } else if (err) {
        this.router.navigate(["/home"]);
        console.log(err);
      } else {
        this.login();
      }
    });
  }

  private redirect() {
    // Redirect with or without 'tab' query parameter
    // Note: does not support additional params besides 'tab'
    const fullRedirect = decodeURI(localStorage.getItem('authRedirect'));
    const redirectArr = fullRedirect.split('?tab=');
    const navArr = [redirectArr[0] || '/'];
    const tabObj = redirectArr[1] ? { queryParams: { tab: redirectArr[1] }} : null;

    if (!tabObj) {
      this.router.navigate(navArr);
    } else {
      this.router.navigate(navArr, tabObj);
    }
    // Redirection completed; clear redirect from storage
    this.clearRedirect();
  }

  private clearRedirect() {
    // Remove redirect from localStorage
    localStorage.removeItem('authRedirect');
  }

  private setSession(authResult): void {
    // Set the time that the Access Token will expire at
    const expiresAt = JSON.stringify(
      authResult.expiresIn * 1000 + new Date().getTime()
    );
    localStorage.setItem("access_token", authResult.accessToken);
    localStorage.setItem("id_token", authResult.idToken);
    localStorage.setItem("expires_at", expiresAt);
    this.scheduleRenewal();
  }

  public logout(): void {
    // Remove tokens and expiry time from localStorage
    localStorage.removeItem("access_token");
    localStorage.removeItem("id_token");
    localStorage.removeItem("expires_at");
    localStorage.removeItem("profile");
    // Go back to the home route
    this.clearRedirect();
    this.router.navigate(["/"]);
    this.userProfile$.next(null);
    this.auth0.logout({
      clientId: "1DgxLrxeSeDL5mC81k8Af61s4Ko3okIq"    });

  }

  renewToken() {
    // Check for valid Auth0 session
    this.auth0.checkSession({}, (err, authResult) => {
      if (authResult && authResult.accessToken) {
        this.setSession(authResult);
        this.getProfile(authResult);
      } else {
        this.clearExpiration();
      }
    });
  }

  private clearExpiration() {
    // Remove token expiration from localStorage
    localStorage.removeItem('expires_at');
  }



  public isAuthenticated(): boolean {
    // Check whether the current time is past the
    // Access Token's expiry time
    const expiresAt = JSON.parse(localStorage.getItem("expires_at") || "{}");
    return new Date().getTime() < expiresAt;
  }

  private getProfile(authresult): void {
    const self = this;
    this.auth0.client.userInfo(authresult.accessToken, (err, profile) => {
      if (profile) {
        self.userProfile$.next(profile);
        localStorage.setItem("profile",JSON.stringify(profile));
        this.redirect();
      }
    });
  }

  public scheduleRenewal() {
    if (!this.isAuthenticated()) { return; }
    this.unscheduleRenewal();

    const expiresAt = JSON.parse(window.localStorage.getItem('expires_at'));

    const expiresIn$ = of(expiresAt).pipe(
      mergeMap(
        expiresAt => {
          const now = Date.now();
          // Use timer to track delay until expiration
          // to run the refresh at the proper time
          return timer(Math.max(1, expiresAt - now));
        }
      )
    );

    // Once the delay time from above is
    // reached, get a new JWT and schedule
    // additional refreshes
    this.refreshSub = expiresIn$.subscribe(
      () => {
        this.renewToken();
        this.scheduleRenewal();
      }
    );
  }

  public unscheduleRenewal() {
    if (this.refreshSub) {
      this.refreshSub.unsubscribe();
    }
  }
}
