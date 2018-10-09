import { Routes } from '@angular/router';
import { CallbackComponent } from './callback/callback.component';
import { AppComponent } from './app.component';

export const ROUTES: Routes = [
  { path: '', component: AppComponent },
  { path: 'callback', component: CallbackComponent },
  { path: '**', redirectTo: '' }
];
