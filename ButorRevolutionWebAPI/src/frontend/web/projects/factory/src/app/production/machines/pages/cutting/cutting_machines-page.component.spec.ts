import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CuttingMachinesPageComponent } from './cutting_machines-page.component';

describe('CuttingMachinesPageComponent', () => {
  let component: CuttingMachinesPageComponent;
  let fixture: ComponentFixture<CuttingMachinesPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CuttingMachinesPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CuttingMachinesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
