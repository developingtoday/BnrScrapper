import { NO_ERRORS_SCHEMA } from "@angular/core";
import { CallbackComponent } from "./callback.component";
import { ComponentFixture, TestBed } from "@angular/core/testing";

describe("CallbackComponent", () => {

  let fixture: ComponentFixture<CallbackComponent>;
  let component: CallbackComponent;
  beforeEach(() => {
    TestBed.configureTestingModule({
      schemas: [NO_ERRORS_SCHEMA],
      providers: [
      ],
      declarations: [CallbackComponent]
    });

    fixture = TestBed.createComponent(CallbackComponent);
    component = fixture.componentInstance;

  });

  it("should be able to create component instance", () => {
    expect(component).toBeDefined();
  });
  
});
