import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { EdgingMachinesPageComponent } from './edging_machines-page.component';

describe('EdgingMachinesPageComponent', () => {
  let component: EdgingMachinesPageComponent;
  let fixture: ComponentFixture<EdgingMachinesPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EdgingMachinesPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EdgingMachinesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
