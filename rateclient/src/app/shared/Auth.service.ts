import { Injectable } from "@angular/core";
import { Router, Route } from "@angular/router";
import { filter } from "rxjs/operators";
import * as auth0 from "auth0-js";
import { Subject } from "rxjs";

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

  private userProfile$: Subject<any>;

  get userProfileSubject(): Subject<any> {
    return this.userProfile$;
}

  constructor(private router: Router) {
    this.userProfile$ = new Subject<any>();
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
  }

  public logout(): void {
    // Remove tokens and expiry time from localStorage
    localStorage.removeItem("access_token");
    localStorage.removeItem("id_token");
    localStorage.removeItem("expires_at");
    // Go back to the home route
    this.clearRedirect();
    this.router.navigate(["/"]);
    this.userProfile$.next(null);

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
        this.redirect();
      }
    });
  }
}
