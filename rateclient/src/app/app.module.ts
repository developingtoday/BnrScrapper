import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { AppComponent } from './app.component';
import { AuthService } from './shared/Auth.service';
import { CallbackComponent } from './callback/callback.component';
import { RouterModule } from '@angular/router';
import { ROUTES } from './app.routes';
import { HomeComponent } from './home/home.component';

@NgModule({
  declarations: [AppComponent, CallbackComponent, HomeComponent],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
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
