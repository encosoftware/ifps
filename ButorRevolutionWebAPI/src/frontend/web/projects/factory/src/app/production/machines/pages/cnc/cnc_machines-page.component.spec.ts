import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { CncMachinesPageComponent } from './cnc_machines-page.component';

describe('CncMachinesPageComponent', () => {
  let component: CncMachinesPageComponent;
  let fixture: ComponentFixture<CncMachinesPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CncMachinesPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CncMachinesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
