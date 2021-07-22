import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewProductDialogComponent } from './new-product.component';

describe('NewProductComponent', () => {
  let component: NewProductDialogComponent;
  let fixture: ComponentFixture<NewProductDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewProductDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewProductDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
