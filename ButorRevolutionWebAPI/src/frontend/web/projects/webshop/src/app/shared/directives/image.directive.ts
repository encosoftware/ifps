import { Directive, ElementRef, Renderer2, Input } from '@angular/core';
import { PictureService } from '../services/picture.service';
import { IPictureModel } from '../models/image-slider';

@Directive({
  selector: '[webshopImage]'
})
export class ImageDirective {
  @Input('webshopImage') set webshopImage(webshopImage: IPictureModel | null) {
    // this.elementRef.nativeElement.src = '/assets/icons/photoplaceholder_.jpg';
    if (!(webshopImage === undefined)) {
      this.service.getThumbnail(webshopImage.containerName, webshopImage.fileName).subscribe(
        (resp: string | ArrayBuffer) => this.renderer.setAttribute(this.elementRef.nativeElement, 'src', resp.toString()),
        err => this.renderer.setAttribute(this.elementRef.nativeElement, 'src', '/assets/icons/photoplaceholder_.jpg')
      );
    } else {
      this.renderer.setAttribute(this.elementRef.nativeElement, 'src', '/assets/icons/photoplaceholder_.jpg');
    }
  }

  constructor(
    private elementRef: ElementRef<any>,
    private renderer: Renderer2,
    private service: PictureService,
  ) { }
}
