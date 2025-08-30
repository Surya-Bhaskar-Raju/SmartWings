import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AircraftRevenueComponent } from './aircraft-revenue.component';

describe('AircraftRevenueComponent', () => {
  let component: AircraftRevenueComponent;
  let fixture: ComponentFixture<AircraftRevenueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AircraftRevenueComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AircraftRevenueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
