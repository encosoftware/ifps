import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderAdditionalsComponent } from './order-additionals.component';

describe('OrderAdditionalsComponent', () => {
  let component: OrderAdditionalsComponent;
  let fixture: ComponentFixture<OrderAdditionalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrderAdditionalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderAdditionalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
