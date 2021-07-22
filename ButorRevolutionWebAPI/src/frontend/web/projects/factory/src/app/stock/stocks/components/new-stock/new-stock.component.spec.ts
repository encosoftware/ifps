import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { NewStorageComponent } from './new-stock.component';

describe('NewStockComponent', () => {
  let component: NewStorageComponent;
  let fixture: ComponentFixture<NewStorageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewStorageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewStorageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
