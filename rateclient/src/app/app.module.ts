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
  MatListModule, MatTableModule, MatPaginatorModule, MatSortModule
} from "@angular/material";
import { LayoutModule } from "@angular/cdk/layout";
import { SidenavComponent } from "./sidenav/sidenav.component";
import { CallbackComponent } from "./callback/callback.component";
import { AuthService } from "./shared/Auth.service";
import { RouterModule} from "@angular/router";
import { ROUTES } from './app.routes';
import { RatedataComponent } from './ratedata/ratedata.component';
import { HomeComponent } from './home/home.component';
@NgModule({
  declarations: [AppComponent, SidenavComponent, CallbackComponent, RatedataComponent, HomeComponent],
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
    RouterModule.forRoot(ROUTES),
    MatTableModule,
    MatPaginatorModule,
    MatSortModule
  ],
  providers: [AuthService],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(public auth: AuthService) {
    auth.handleAuthentication();
  }
}
