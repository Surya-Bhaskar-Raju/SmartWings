import { ComponentFixture, TestBed } from '@angular/core/testing';


import { GetallbookingsComponent } from './getallbookings.component';
describe('GetallbookingsComponent', () => {
  let component: GetallbookingsComponent;
  let fixture: ComponentFixture<GetallbookingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetallbookingsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetallbookingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
