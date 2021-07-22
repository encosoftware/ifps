import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutTwoColComponent } from './layout-two-col.component';

describe('LayoutTwoColComponent', () => {
  let component: LayoutTwoColComponent;
  let fixture: ComponentFixture<LayoutTwoColComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LayoutTwoColComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LayoutTwoColComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
