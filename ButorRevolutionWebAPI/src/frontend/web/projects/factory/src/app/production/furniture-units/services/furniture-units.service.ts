import { Injectable, Inject } from '@angular/core';
import { ApiFurnitureunitsClient, PagedListDtoOfFurnitureUnitListDto, FurnitureUnitListDto, API_BASE_URL, ApiFurnitureunitsXxlClient } from '../../../shared/clients';
import { Observable } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { IFurnitureUnitModel, IFurnitureUnitFilterModel } from '../models/furniture-unit.model';
import { map } from 'rxjs/operators';
import { HttpEventType, HttpClient } from '@angular/common/http';

@Injectable()
export class FurnitureUnitsService {

  constructor(
    private furnitureUnitsClient: ApiFurnitureunitsClient,
    private furnitureUnitsXxlClient: ApiFurnitureunitsXxlClient,
    private httpClient: HttpClient,
    @Inject(API_BASE_URL) private baseUrl?: string
    ) { }

  public uploadXxlFileForCncGeneration(furnitureUnitId: string, containerName: string, fileName: string): Observable<void> {
    return this.furnitureUnitsXxlClient.uploadXxlFileForCncGeneration(furnitureUnitId, containerName, fileName);
  }

  public uploadDocument(data) {
    var uploadURL = this.baseUrl + '/api/documents?container=XxlData';

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
    )
  }

  getFurnitureUnits(filter: IFurnitureUnitFilterModel): Observable<PagedData<IFurnitureUnitModel>> {
    return this.furnitureUnitsClient.getFurnitureUnits(
      filter.code,
      filter.description,
      0,
      filter.isUploaded,
      undefined,
      filter.pager.pageIndex,
      filter.pager.pageSize
    ).pipe(
      map((dto: PagedListDtoOfFurnitureUnitListDto): PagedData<IFurnitureUnitModel> => ({
        items: dto.data.map((x: FurnitureUnitListDto): IFurnitureUnitModel => ({
          id: x.id,
          code: x.code,
          description: x.description,
          category: x.category.name,
          isUploaded: x.hasCncFile
        })),
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        totalCount: dto.totalCount
      })))
  }
}
