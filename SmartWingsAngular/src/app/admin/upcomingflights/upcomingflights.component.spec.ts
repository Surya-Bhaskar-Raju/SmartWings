import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpcomingflightsComponent } from './upcomingflights.component';

describe('UpcomingflightsComponent', () => {
  let component: UpcomingflightsComponent;
  let fixture: ComponentFixture<UpcomingflightsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpcomingflightsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpcomingflightsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
