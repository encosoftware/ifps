import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerOrderMessagesComponent } from './customer-order-messages.component';

describe('CustomerOrderMessagesComponent', () => {
  let component: CustomerOrderMessagesComponent;
  let fixture: ComponentFixture<CustomerOrderMessagesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerOrderMessagesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerOrderMessagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
