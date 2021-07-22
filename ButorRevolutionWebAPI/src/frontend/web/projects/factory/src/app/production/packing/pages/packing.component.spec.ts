import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PackingPageComponent } from './packing.component';

describe('PackingComponent', () => {
  let component: PackingPageComponent;
  let fixture: ComponentFixture<PackingPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PackingPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PackingPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
