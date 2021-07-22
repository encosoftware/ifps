import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImageSliderRowComponent } from './image-slider-row.component';

describe('ImageSliderRowComponent', () => {
  let component: ImageSliderRowComponent;
  let fixture: ComponentFixture<ImageSliderRowComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImageSliderRowComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImageSliderRowComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
