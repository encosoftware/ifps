import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewCellComponent } from './new-cell.component';

describe('NewCellComponent', () => {
  let component: NewCellComponent;
  let fixture: ComponentFixture<NewCellComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewCellComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewCellComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
