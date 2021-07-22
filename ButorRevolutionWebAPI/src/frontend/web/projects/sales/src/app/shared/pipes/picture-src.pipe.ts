import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'pictureSrc'
})
export class PictureSrcPipe implements PipeTransform {

  transform(baseUrl: string, container: string, name: string): any {
    let picture;
    if (container && name) {
      picture = `${baseUrl}/api/images?containerName=${container}&fileName=${name}`;
    } else {
      picture = `/assets/icons/photoplaceholder_.jpg`;
    }

    return picture;
  }

}
