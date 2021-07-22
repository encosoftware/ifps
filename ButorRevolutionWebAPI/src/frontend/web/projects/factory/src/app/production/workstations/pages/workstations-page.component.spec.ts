import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkstationsPageComponent } from './workstations-page.component';

describe('WorkstationsPageComponent', () => {
  let component: WorkstationsPageComponent;
  let fixture: ComponentFixture<WorkstationsPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkstationsPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkstationsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
