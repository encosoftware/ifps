import { Component, OnInit, Input } from '@angular/core';
import { ConfigImageSlider, IimagesSliderModel, IPictureModel } from '../../models/image-slider';

@Component({
  selector: 'app-image-slider',
  templateUrl: './image-slider.component.html',
  styleUrls: ['./image-slider.component.scss'],
})
export class ImageSliderComponent implements OnInit {

  @Input('images') set images(images: IPictureModel[]) {
    this.imageUrls = images;
    if (this.imageUrls.length > 0) {
      this.selectedPic = images[0];
    } 
  }
  @Input() sliderConfig: ConfigImageSlider = {
    slidesToShow: 5,
    slidesToScroll: 1,
    infinite: true
  };
  @Input() set disableButton(disableButtons: boolean) {
    this.disableButtonUp = disableButtons;
    this.disableButtonDown = disableButtons;
  };

  public imageUrls: IPictureModel[];
  public imageUrl: IPictureModel[];
  public state = 'void';
  public disableButtonUp = false;
  public disableButtonDown = false;
  public start = 0;
  public count = this.sliderConfig.slidesToShow;
  public selectedPic: IPictureModel;



  constructor() { }

  ngOnInit() {
    this.count = (this.imageUrls.length > this.sliderConfig.slidesToShow) ? this.sliderConfig.slidesToShow : this.imageUrls.length;
    this.setCurrImages(this.imageUrls);
  }


  slideUp() {
    this.state = 'up';
    this.setCurrImages(this.imageUrls, 'Up');

  }
  slideDown() {
    this.state = 'down';
    this.setCurrImages(this.imageUrls, 'Down');
  }

  setCurrImages(Imgarray: IPictureModel[], direction?: 'Down' | 'Up') {

    this.imageUrl = [];
    if (direction) {
      if (direction === 'Down') {
        this.start++;
      } else {
        this.start--;
      }
    }

    if (!this.sliderConfig.infinite && this.start === 0) {
      this.disableButtonUp = true;
    } else {
      this.disableButtonUp = false;
    }


    if (!this.sliderConfig.infinite && this.count + this.start > Imgarray.length - 1) {
      this.disableButtonDown = true;
    } else {
      this.disableButtonDown = false;

    }

    for (let i = 0; i < this.count; i++) {
      let index = i + this.start;

      if (index > Imgarray.length - 1) {
        index = index % Imgarray.length;
      }

      if (index < 0) {
        const repeatCounter = Math.abs(Math.floor(index / Imgarray.length));
        index = index + repeatCounter * Imgarray.length;
      }

      this.imageUrl.push(Imgarray[index]);
    }
  }
  onFinish(event) {
    this.state = 'void';
  }

  onStart(event) {
  }

  imageSelect(selected) {
    this.selectedPic = selected;
  }
}

