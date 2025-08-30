import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchbookingbyrefidComponent } from './searchbookingbyrefid.component';

describe('SearchbookingbyrefidComponent', () => {
  let component: SearchbookingbyrefidComponent;
  let fixture: ComponentFixture<SearchbookingbyrefidComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SearchbookingbyrefidComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchbookingbyrefidComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
