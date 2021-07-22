import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DecorboardsComponent } from './decorboards.component';

describe('DecorboardsComponent', () => {
  let component: DecorboardsComponent;
  let fixture: ComponentFixture<DecorboardsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DecorboardsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DecorboardsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
