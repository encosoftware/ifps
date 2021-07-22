import { Directive, ElementRef, Renderer2, Input } from '@angular/core';
import { UploadPicService } from '../services/upload-pic.service';
import { IPictureModel } from '../../sales/dashboard/models/messages.model';

@Directive({
  selector: '[butorImage]'
})
export class ImageDirective {
  @Input('butorImage') set butorImage(butorImage: IPictureModel | null) {
    // this.elementRef.nativeElement.src = '/assets/icons/photoplaceholder_.jpg';
    if (!(butorImage === undefined)) {
      this.service.getThumbnail(butorImage.containerName, butorImage.fileName).subscribe(
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
    private service: UploadPicService,
  ) { }

}
