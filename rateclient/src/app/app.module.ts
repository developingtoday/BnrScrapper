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
  MatListModule, MatTableModule, MatPaginatorModule, MatSortModule, MAT_LABEL_GLOBAL_OPTIONS, MatCheckboxModule, MatInputModule, MatSelectModule, MatRadioModule, MatDatepickerModule, MatNativeDateModule
} from "@angular/material";
import { LayoutModule } from "@angular/cdk/layout";
import { SidenavComponent } from "./sidenav/sidenav.component";
import { CallbackComponent } from "./callback/callback.component";
import { AuthService } from "./shared/Auth.service";
import { RouterModule} from "@angular/router";
import { ROUTES } from './app.routes';
import { RatedataComponent } from './ratedata/ratedata.component';
import { HomeComponent } from './home/home.component';
import { LoanComponent } from './loan/loan.component';
import { LoanEditorComponent } from './loaneditor/loaneditor.component';
import { CommonModule } from "@angular/common";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
@NgModule({
  declarations: [AppComponent, SidenavComponent, CallbackComponent,
    RatedataComponent, HomeComponent, LoanComponent, LoanEditorComponent],
  imports: [
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    CommonModule,
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
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatCheckboxModule,
    MatInputModule,
    MatSelectModule,
    MatRadioModule,
    MatDatepickerModule,
    MatNativeDateModule,
    RouterModule.forRoot(ROUTES),

  ],
  providers: [AuthService,
  {provide:MAT_LABEL_GLOBAL_OPTIONS, useValue: {float: 'always'}}
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(public auth: AuthService) {
    auth.handleAuthentication();
  }
}
