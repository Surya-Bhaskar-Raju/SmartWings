import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAircraftComponent } from './add-aircraft.component';

describe('AddAircraftComponent', () => {
  let component: AddAircraftComponent;
  let fixture: ComponentFixture<AddAircraftComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddAircraftComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddAircraftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
