import { Component } from "@angular/core";
import {
  BreakpointObserver,
  Breakpoints,
  BreakpointState
} from "@angular/cdk/layout";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { AuthService } from "../shared/Auth.service";

@Component({
  selector: "app-sidenav",
  templateUrl: "./sidenav.component.html",
  styleUrls: ["./sidenav.component.css"]
})
export class SidenavComponent {
  isHandset$: Observable<boolean> = this.breakpointObserver
    .observe(Breakpoints.Handset)
    .pipe(map(result => result.matches));
  useProfile$: Observable<any>;

  constructor(
    private breakpointObserver: BreakpointObserver,
    private auth: AuthService
  ) {
    this.useProfile$ = this.auth.userProfileSubject;

  }

  private logout(){
    this.auth.logout();
  }
}
