import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { PrintCodesComponent } from './print-codes.component';

describe('PrintCodesComponent', () => {
  let component: PrintCodesComponent;
  let fixture: ComponentFixture<PrintCodesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrintCodesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrintCodesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
