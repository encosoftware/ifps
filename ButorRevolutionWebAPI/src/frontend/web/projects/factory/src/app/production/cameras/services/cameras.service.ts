import { Injectable } from '@angular/core';
import {
  CameraListDto,
  ApiCamerasClient,
  PagedListDtoOfCameraListDto,
  CameraDetailsDto,
  CameraCreateDto,
  CameraUpdateDto
} from '../../../shared/clients';
import { Observable } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { map } from 'rxjs/operators';
import { ICameraFilterModel, ICameraListModel, ICameraDetailsModel } from '../models/cameras.model';

@Injectable({
  providedIn: 'root'
})
export class CameraService {

  constructor(
    private camerasClient: ApiCamerasClient
  ) { }

  getCameraList(filter: ICameraFilterModel): Observable<PagedData<ICameraListModel>> {
    return this.camerasClient.getCameras(
      filter.name,
      filter.type,
      filter.ipAddress,
      undefined,
      filter.pager.pageIndex,
      filter.pager.pageSize
    ).pipe(
      map((dto: PagedListDtoOfCameraListDto): PagedData<ICameraListModel> => ({
        items: dto.data.map((x: CameraListDto): ICameraListModel => ({
          id: x.id,
          name: x.name,
          ipAddress: x.ipAddress,
          type: x.type
        })),
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        totalCount: dto.totalCount
      }))
    );
  }

  getCamera(id: number): Observable<ICameraDetailsModel> {
    return this.camerasClient.getCamera(id).pipe(
      map((dto: CameraDetailsDto): ICameraDetailsModel => ({
        name: dto.name,
        ipAddress: dto.ipAddress,
        type: dto.type
      }))
    );
  }

  postCamera(camera: ICameraDetailsModel): Observable<number> {
    const dto = new CameraCreateDto({
      name: camera.name,
      ipAddress: camera.ipAddress,
      type: camera.type
    });
    return this.camerasClient.createCamera(dto);
  }

  putCamera(id: number, camera: ICameraDetailsModel): Observable<void> {
    const dto = new CameraUpdateDto({
      name: camera.name,
      ipAddress: camera.ipAddress,
      type: camera.type
    });

    return this.camerasClient.updateCamera(id, dto);
  }

  deleteCamera(id: number): Observable<void> {
    return this.camerasClient.deleteCamera(id);
  }
}
