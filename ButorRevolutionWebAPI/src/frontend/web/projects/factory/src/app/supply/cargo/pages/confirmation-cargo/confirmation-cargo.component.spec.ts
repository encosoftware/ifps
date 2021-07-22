import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmationCargoComponent } from './confirmation-cargo.component';

describe('ConfirmationCargoComponent', () => {
  let component: ConfirmationCargoComponent;
  let fixture: ComponentFixture<ConfirmationCargoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmationCargoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmationCargoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
