import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MaterialMenuComponent } from './material-menu.component';

describe('MaterialMenuComponent', () => {
  let component: MaterialMenuComponent;
  let fixture: ComponentFixture<MaterialMenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MaterialMenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MaterialMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
