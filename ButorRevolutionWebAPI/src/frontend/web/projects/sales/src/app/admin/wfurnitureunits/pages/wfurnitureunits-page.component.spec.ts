import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { WFUPageComponent } from './wfurnitureunits-page.component';

describe('WFUPageComponent', () => {
  let component: WFUPageComponent;
  let fixture: ComponentFixture<WFUPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WFUPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WFUPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
