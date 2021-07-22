import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { MaterialPackagePageComponent } from './material-packages-page.component';

describe('MaterialPackagePageComponent', () => {
  let component: MaterialPackagePageComponent;
  let fixture: ComponentFixture<MaterialPackagePageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MaterialPackagePageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MaterialPackagePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
