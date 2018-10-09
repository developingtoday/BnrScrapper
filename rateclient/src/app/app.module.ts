import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { AppComponent } from "./app.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import {
  MatGridListModule,
  MatCardModule,
  MatMenuModule,
  MatIconModule,
  MatButtonModule,
  MatToolbarModule,
  MatSidenavModule,
  MatListModule
} from "@angular/material";
import { LayoutModule } from "@angular/cdk/layout";
import { SidenavComponent } from "./sidenav/sidenav.component";
import { CallbackComponent } from "./callback/callback.component";
import { AuthService } from "./shared/Auth.service";
import { RouterModule} from "@angular/router";
import { ROUTES } from './app.routes';
@NgModule({
  declarations: [AppComponent, SidenavComponent, CallbackComponent],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    MatGridListModule,
    MatCardModule,
    MatMenuModule,
    MatIconModule,
    MatButtonModule,
    LayoutModule,
    MatToolbarModule,
    MatSidenavModule,
    MatListModule,
    RouterModule.forRoot(ROUTES)
  ],
  providers: [AuthService],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(public auth: AuthService) {
    auth.handleAuthentication();
  }
}
