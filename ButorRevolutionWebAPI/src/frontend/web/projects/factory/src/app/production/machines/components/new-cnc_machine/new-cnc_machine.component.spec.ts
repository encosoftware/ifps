import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NewCncMachineComponent } from './new-cnc_machine.component';


describe('NewCncMachineComponent', () => {
  let component: NewCncMachineComponent;
  let fixture: ComponentFixture<NewCncMachineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewCncMachineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewCncMachineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
