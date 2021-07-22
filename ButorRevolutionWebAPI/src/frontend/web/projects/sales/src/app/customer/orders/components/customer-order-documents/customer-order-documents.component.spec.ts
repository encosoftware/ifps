import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerOrderDocumentsComponent } from './customer-order-documents.component';

describe('CustomerOrderDocumentsComponent', () => {
  let component: CustomerOrderDocumentsComponent;
  let fixture: ComponentFixture<CustomerOrderDocumentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerOrderDocumentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerOrderDocumentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
