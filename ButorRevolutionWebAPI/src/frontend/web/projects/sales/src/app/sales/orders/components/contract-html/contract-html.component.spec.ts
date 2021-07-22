import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractHtmlComponent } from './contract-html.component';

describe('ContractHtmlComponent', () => {
  let component: ContractHtmlComponent;
  let fixture: ComponentFixture<ContractHtmlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContractHtmlComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContractHtmlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
