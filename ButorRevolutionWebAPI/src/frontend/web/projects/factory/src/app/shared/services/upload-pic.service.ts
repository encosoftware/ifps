import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { ApiImagesClient, ValueTupleOfStringAndString, FileParameter } from '../clients';
import { switchMap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UploadPicService {
  uploadeLabel: string;
  view: BehaviorSubject<string | ArrayBuffer> = new BehaviorSubject('/assets/icons/photoplaceholder_.jpg');
  constructor(private client: ApiImagesClient) { }

  public UploadFile(data, folder: string): Observable<ValueTupleOfStringAndString> {
    this.uploadeLabel = data.name;
    const fileParameter: FileParameter = {
      data,
      fileName: this.uploadeLabel
    };
    return this.client.createImage(fileParameter, folder).pipe();
  }

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
