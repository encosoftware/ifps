import { Injectable } from '@angular/core';
import { ValueTupleOfStringAndString, ApiImagesClient, ApiImagesMultipleClient } from '../clients';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import { FileParameter } from '../models/FileParameter';

@Injectable({
  providedIn: 'root'
})
export class UploadPicService {
  uploadeLabel: string;
  view: BehaviorSubject<string | ArrayBuffer> = new BehaviorSubject('/assets/icons/photoplaceholder_.jpg');
  constructor(
    private client: ApiImagesClient,
    private clientMulti: ApiImagesMultipleClient,
  ) { }

  public UploadFile(data, folder: string, name?: string): Observable<ValueTupleOfStringAndString> {
    this.uploadeLabel = name ? name : data.name;
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

  createImages(files: File[] | null | undefined, container: string | null): Observable<ValueTupleOfStringAndString[] | null> {
    const fileArray = Array.from(files);
    const fileParameter: FileParameter[] = fileArray.map(file => ({
      data: file,
      fileName: file.name
    }));
    return this.clientMulti.createImages(fileParameter, container).pipe()
  }
}
