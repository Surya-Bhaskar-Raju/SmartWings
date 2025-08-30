import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewpastbookingsComponent } from './viewpastbookings.component';

describe('ViewpastbookingsComponent', () => {
  let component: ViewpastbookingsComponent;
  let fixture: ComponentFixture<ViewpastbookingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewpastbookingsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewpastbookingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
