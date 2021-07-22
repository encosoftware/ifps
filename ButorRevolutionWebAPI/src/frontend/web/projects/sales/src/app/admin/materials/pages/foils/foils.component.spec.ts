import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FoilsComponent } from './foils.component';

describe('FoilsComponent', () => {
  let component: FoilsComponent;
  let fixture: ComponentFixture<FoilsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FoilsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FoilsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
