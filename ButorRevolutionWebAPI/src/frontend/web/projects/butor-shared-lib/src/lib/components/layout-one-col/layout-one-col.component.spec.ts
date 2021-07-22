import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutOneColComponent } from './layout-one-col.component';

describe('LayoutOneColComponent', () => {
  let component: LayoutOneColComponent;
  let fixture: ComponentFixture<LayoutOneColComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LayoutOneColComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LayoutOneColComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
