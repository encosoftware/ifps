import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OptimisationModalComponent } from './optimisation-modal.component';

describe('OptimisationModalComponent', () => {
  let component: OptimisationModalComponent;
  let fixture: ComponentFixture<OptimisationModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OptimisationModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OptimisationModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
