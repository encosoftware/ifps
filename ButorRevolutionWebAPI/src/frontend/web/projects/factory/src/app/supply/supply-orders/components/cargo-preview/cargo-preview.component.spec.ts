import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CargoPreviewComponent } from './cargo-preview.component';

describe('CargoPreviewComponent', () => {
  let component: CargoPreviewComponent;
  let fixture: ComponentFixture<CargoPreviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CargoPreviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CargoPreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
