import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoaneditorComponent } from './loaneditor.component';

describe('LoaneditorComponent', () => {
  let component: LoaneditorComponent;
  let fixture: ComponentFixture<LoaneditorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoaneditorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoaneditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
