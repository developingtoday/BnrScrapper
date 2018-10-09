
import { fakeAsync, ComponentFixture, TestBed } from '@angular/core/testing';

import { RatedataComponent } from './ratedata.component';

describe('RatedataComponent', () => {
  let component: RatedataComponent;
  let fixture: ComponentFixture<RatedataComponent>;

  beforeEach(fakeAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ RatedataComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RatedataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should compile', () => {
    expect(component).toBeTruthy();
  });
});
