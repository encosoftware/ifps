import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArrivedCargoComponent } from './arrived-cargo.component';

describe('ArrivedCargoComponent', () => {
  let component: ArrivedCargoComponent;
  let fixture: ComponentFixture<ArrivedCargoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArrivedCargoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArrivedCargoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
