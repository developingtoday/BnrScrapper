import { Component } from '@angular/core';
import { AuthService } from './shared/Auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],

})
export class AppComponent {
  title = 'app works!';
  constructor(private auth:AuthService){
    // auth.handleAuthentication();
  }

  public test(){
    this.auth.login();
  }
}
