import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UnderProductionComponent } from './under-production.component';

describe('UnderProductionComponent', () => {
  let component: UnderProductionComponent;
  let fixture: ComponentFixture<UnderProductionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UnderProductionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UnderProductionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
