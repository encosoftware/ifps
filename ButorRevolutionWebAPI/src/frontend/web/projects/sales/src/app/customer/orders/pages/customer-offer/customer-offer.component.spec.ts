import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerOfferComponent } from './customer-offer.component';

describe('CustomerOfferComponent', () => {
  let component: CustomerOfferComponent;
  let fixture: ComponentFixture<CustomerOfferComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerOfferComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerOfferComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
