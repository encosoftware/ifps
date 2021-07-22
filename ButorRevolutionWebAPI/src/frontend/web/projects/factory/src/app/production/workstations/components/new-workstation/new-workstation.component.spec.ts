import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewWorkstationComponent } from './new-workstation.component';

describe('NewWorkstationComponent', () => {
  let component: NewWorkstationComponent;
  let fixture: ComponentFixture<NewWorkstationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewWorkstationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewWorkstationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
