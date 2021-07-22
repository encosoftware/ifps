import { Component, OnInit, Input } from '@angular/core';
import { ConfigImageSlider } from '../../models/image-slider';
import { FurnitureUnitByWebshopDetailsRecomendationModel } from '../../../products-details/models/details';

@Component({
  selector: 'app-image-slider-row',
  templateUrl: './image-slider-row.component.html',
  styleUrls: ['./image-slider-row.component.scss']
})
export class ImageSliderRowComponent implements OnInit {

  @Input('images') set images(images: FurnitureUnitByWebshopDetailsRecomendationModel[]) {
    this.imageUrls = images;
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

  public imageUrls: FurnitureUnitByWebshopDetailsRecomendationModel[] = [];
  public imageUrl: FurnitureUnitByWebshopDetailsRecomendationModel[] = [];
  public state = 'void';
  public disableButtonUp = false;
  public disableButtonDown = false;
  public start = 0;
  public count = this.sliderConfig.slidesToShow;
  public selectedPic: string;



  constructor() { }

  ngOnInit() {
    this.count = (this.imageUrls.length > this.sliderConfig.slidesToShow) ?
      this.sliderConfig.slidesToShow :
      this.imageUrls.length;
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

  setCurrImages(Imgarray: FurnitureUnitByWebshopDetailsRecomendationModel[], direction?: 'Down' | 'Up') {
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
    this.selectedPic = selected.target.src;
  }


}
