import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorktopsComponent } from './worktops.component';

describe('WorktopsComponent', () => {
  let component: WorktopsComponent;
  let fixture: ComponentFixture<WorktopsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorktopsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorktopsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
