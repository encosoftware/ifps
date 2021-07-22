import { Injectable } from '@angular/core';
import { ApiImagesClient } from '../clients';
import { switchMap, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PictureService {

  constructor(private client: ApiImagesClient) { }

  public getThumbnail(containerName: string | null, fileName: string | null): Observable<string | ArrayBuffer> {
    return this.client.getImage(containerName, fileName).pipe(
      switchMap((res) => Observable.create(obs => {
        const reader = new FileReader();
        reader.onerror = err => obs.error(err);
        reader.onabort = err => obs.error(err);
        reader.onload = () => obs.next(reader.result);
        reader.onloadend = () => obs.complete();

        return reader.readAsDataURL(res.data);
      })),
      catchError((err) => of(err.message))
    );
  }
}
