import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { API_BASE_URL, ApiDocumentsTypesClient, ApiDocumentsFoldersClient } from '../../../shared/clients';
import { DocumentTypeModel, DocumentFolderTypeModel } from '../models/orders';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FileUploaderService {

  constructor(
    private httpClient: HttpClient,
    private typesClient: ApiDocumentsTypesClient,
    private folderClient: ApiDocumentsFoldersClient,
    @Inject(API_BASE_URL) private baseUrl?: string
  ) { }

  public upload(data) {
    let uploadURL = this.baseUrl + '/api/documents?container=OrderDocuments';

    const content = new FormData();
    content.append('file', data, data.name);

    return this.httpClient.post<any>(uploadURL, content, {
      reportProgress: true,
      observe: 'events'
    }).pipe(map((event) => {
      switch (event.type) {
        case HttpEventType.Sent:
          return {
            status: 'started',
            name: data.name,
          };

        case HttpEventType.UploadProgress:
          const progress = Math.round(100 * event.loaded / event.total);
          return {
            status: 'progress',
            loaded: Math.round(event.loaded / 1024),
            total: Math.round(event.total / 1024),
            percentage: progress,
            name: data.name
          };
        case HttpEventType.Response:
          return {
            status: 'uploaded',
            data: event.body
          };
      }
    })
    );
  }

  getDocumentTypes(): Observable<DocumentTypeModel[] | null> {
    return this.typesClient.getDocumentTypes().pipe(
      map(res => res.map(ins => ({
        id: ins.id,
        documentType: ins.documentType,
        translation: ins.translation
      })))
    )
  }
  getDocumentFolders(): Observable<DocumentFolderTypeModel[] | null> {
    return this.folderClient.getDocumentFolders().pipe(
      map(res => res.map(ins => ({
        folderId: ins.folderId,
        folderName: ins.folderName,
        supportedTypes: ins.supportedTypes.map(resp => ({
          id: resp.id,
          documentType: resp.documentType,
          translation: resp.translation
        })),
      })))
    )
  }
}
