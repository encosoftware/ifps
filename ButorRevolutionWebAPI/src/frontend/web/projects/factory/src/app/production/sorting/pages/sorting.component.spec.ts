import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SortingPageComponent } from './sorting.component';

describe('SortingComponent', () => {
  let component: SortingPageComponent;
  let fixture: ComponentFixture<SortingPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SortingPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SortingPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
