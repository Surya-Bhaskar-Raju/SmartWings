import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewAircraftComponent } from './view-aircraft.component';

describe('ViewAircraftComponent', () => {
  let component: ViewAircraftComponent;
  let fixture: ComponentFixture<ViewAircraftComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewAircraftComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewAircraftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
