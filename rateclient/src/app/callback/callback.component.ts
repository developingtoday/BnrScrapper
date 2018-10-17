import { Component, OnInit } from "@angular/core";
import { AuthService } from "../shared/Auth.service";

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.scss']
})

export class CallbackComponent implements OnInit {

  constructor(private auth:AuthService) {
    auth.handleAuthentication();
  }

  ngOnInit() {

  }
}
