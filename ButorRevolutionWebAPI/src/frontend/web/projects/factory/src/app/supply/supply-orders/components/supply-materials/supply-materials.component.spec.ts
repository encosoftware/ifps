import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SupplyMaterialsComponent } from './supply-materials.component';

describe('SupplyMaterialsComponent', () => {
  let component: SupplyMaterialsComponent;
  let fixture: ComponentFixture<SupplyMaterialsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SupplyMaterialsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SupplyMaterialsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
