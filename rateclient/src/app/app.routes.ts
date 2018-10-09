import { Routes } from '@angular/router';
import { CallbackComponent } from './callback/callback.component';
import { AppComponent } from './app.component';
import { RatedataComponent } from './ratedata/ratedata.component';
import { HomeComponent } from './home/home.component';

export const ROUTES: Routes = [
  { path: '', component: HomeComponent },
  { path: 'callback', component: CallbackComponent },
  {path:'rates', component:RatedataComponent}
  { path: '**', redirectTo: '' }
];
