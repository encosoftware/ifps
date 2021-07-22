import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutThreeColComponent } from './layout-three-col.component';

describe('LayoutThreeColComponent', () => {
  let component: LayoutThreeColComponent;
  let fixture: ComponentFixture<LayoutThreeColComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LayoutThreeColComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LayoutThreeColComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
