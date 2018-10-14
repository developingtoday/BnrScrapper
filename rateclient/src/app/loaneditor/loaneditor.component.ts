import { Component, OnInit, OnDestroy } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Subscription } from "rxjs";
import {FormGroup, FormBuilder} from "@angular/forms";

@Component({
  selector: "app-loaneditor",
  templateUrl: "./loaneditor.component.html",
  styleUrls: ["./loaneditor.component.scss"]
})
export class LoanEditorComponent implements OnInit,OnDestroy {
  private id: number;
  options: FormGroup;
  private sub: Subscription;
  constructor(fb: FormBuilder,private route:ActivatedRoute) {
    this.options = fb.group({
      hideRequired: false,
      floatLabel: 'auto',
    });
  }
  ngOnInit() {
    this.sub = this.route.params.subscribe(a => {
      this.id = +a["id"];
    });
  }
  ngOnDestroy(){
    this.sub.unsubscribe();
  }
}
