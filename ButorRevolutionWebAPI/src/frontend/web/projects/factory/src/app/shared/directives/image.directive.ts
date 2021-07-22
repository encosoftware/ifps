import { Directive, TemplateRef, Input, ElementRef, Renderer2, OnInit } from '@angular/core';
import { UploadPicService } from '../services/upload-pic.service';
import { IPictureModel } from '../../admin/materials/models/decorboards.model';

@Directive({
  selector: '[factoryImage]'
})
export class ImageDirective {
  @Input('factoryImage') set factoryImage(factoryImage: IPictureModel | null) {
    // this.elementRef.nativeElement.src = '/assets/icons/photoplaceholder_.jpg';
    if (!(factoryImage === null)) {
      this.service.getThumbnail(factoryImage.containerName, factoryImage.fileName).subscribe(
        (resp: string | ArrayBuffer) => this.renderer.setAttribute(this.elementRef.nativeElement, 'src', resp.toString())
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
