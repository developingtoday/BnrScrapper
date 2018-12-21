import { Component, OnInit, OnDestroy } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { Subscription } from "rxjs";
import { FormGroup, FormBuilder, FormControl } from "@angular/forms";
import { BackendService } from "../shared/backend.service";
import { Loan } from "../shared/models/loan.model";

@Component({
  selector: "app-loaneditor",
  templateUrl: "./loaneditor.component.html",
  styleUrls: ["./loaneditor.component.scss"]
})
export class LoanEditorComponent implements OnInit, OnDestroy {
  private id: string;
  private loan: Loan;
  loanForm: FormGroup = new FormGroup({
    loanName: new FormControl(""),
    loanTransaction: new FormControl("")
  });
  private sub: Subscription;
  constructor(
    fb: FormBuilder,
    private route: ActivatedRoute,
    private backendService: BackendService
  ) {}
  ngOnInit() {
    this.sub = this.route.params.subscribe(a => {
      this.id = a["id"];
      this.backendService.getLoanById(this.id).subscribe(s => {
        this.loan = s;
      });
    });
  }
  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
