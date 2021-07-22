import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BtnHamburgerComponent } from './btn-hamburger.component';

describe('BtnHamburgerComponent', () => {
  let component: BtnHamburgerComponent;
  let fixture: ComponentFixture<BtnHamburgerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BtnHamburgerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BtnHamburgerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
