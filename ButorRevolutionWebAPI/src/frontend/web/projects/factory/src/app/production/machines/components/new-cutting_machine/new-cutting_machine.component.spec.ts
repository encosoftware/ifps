import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NewCuttingMachineComponent } from './new-cutting_machine.component';


describe('NewCuttingMachineComponent', () => {
  let component: NewCuttingMachineComponent;
  let fixture: ComponentFixture<NewCuttingMachineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewCuttingMachineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewCuttingMachineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
