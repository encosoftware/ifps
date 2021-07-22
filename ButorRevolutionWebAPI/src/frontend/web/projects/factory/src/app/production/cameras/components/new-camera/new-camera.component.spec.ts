import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NewCameraComponent } from './new-camera.component';


describe('NewCameraComponent', () => {
  let component: NewCameraComponent;
  let fixture: ComponentFixture<NewCameraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewCameraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewCameraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
