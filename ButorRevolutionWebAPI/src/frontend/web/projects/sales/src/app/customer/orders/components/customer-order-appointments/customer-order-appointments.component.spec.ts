import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerOrderAppointmentsComponent } from './customer-order-appointments.component';

describe('CustomerOrderAppointmentsComponent', () => {
  let component: CustomerOrderAppointmentsComponent;
  let fixture: ComponentFixture<CustomerOrderAppointmentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerOrderAppointmentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerOrderAppointmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
