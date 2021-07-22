import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewProductUnitComponent } from './new-product-unit.component';

describe('NewProductUnitComponent', () => {
  let component: NewProductUnitComponent;
  let fixture: ComponentFixture<NewProductUnitComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewProductUnitComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewProductUnitComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
