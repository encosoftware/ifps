import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MultiUploadPicComponent } from './multi-upload-pic.component';

describe('MultiUploadPicComponent', () => {
  let component: MultiUploadPicComponent;
  let fixture: ComponentFixture<MultiUploadPicComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MultiUploadPicComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MultiUploadPicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
