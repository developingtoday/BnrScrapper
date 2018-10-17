import { Routes } from '@angular/router';
import { CallbackComponent } from './callback/callback.component';
import { AppComponent } from './app.component';
import { RatedataComponent } from './ratedata/ratedata.component';
import { HomeComponent } from './home/home.component';
import { LoanComponent } from './loan/loan.component';
import { LoanEditorComponent } from './loaneditor/loaneditor.component';
import { AuthguardService } from './shared/authguard.service';

export const ROUTES: Routes = [
  { path: '', component: HomeComponent },
  { path: 'callback', component: CallbackComponent },
  {path: 'rates', component: RatedataComponent},
  {path: 'loan', component: LoanComponent, canActivate:[AuthguardService]},
  {path: 'loan/edit/:id', component: LoanEditorComponent,canActivate:[AuthguardService]},
  {path: 'loan/edit', component: LoanEditorComponent,canActivate:[AuthguardService]},
  { path: '**', redirectTo: '' }
];
