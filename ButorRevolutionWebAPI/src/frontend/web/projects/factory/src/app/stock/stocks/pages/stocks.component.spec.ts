import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { StoragesComponent } from './stocks.component';

describe('StocksComponent', () => {
  let component: StoragesComponent;
  let fixture: ComponentFixture<StoragesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StoragesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StoragesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
