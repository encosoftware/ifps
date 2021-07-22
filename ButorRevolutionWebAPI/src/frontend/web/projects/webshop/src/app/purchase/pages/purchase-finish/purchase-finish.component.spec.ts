import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PurchaseFinishComponent } from './purchase-finish.component';

describe('PurchaseFinishComponent', () => {
  let component: PurchaseFinishComponent;
  let fixture: ComponentFixture<PurchaseFinishComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchaseFinishComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchaseFinishComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
