import { Injectable } from '@angular/core';
import {
  ApiWorkstationsClient,
  PagedListDtoOfWorkStationListDto,
  WorkStationListDto,
  WorkStationCreateDto,
  WorkStationUpdateDto,
  WorkStationDetailsDto,
  ApiWorkstationtypesClient,
  WorkStationTypeListDto,
  ApiWorkstationsActivateClient,
  ApiCamerasNamesClient,
  CameraNameListDto,
  CFCProductionStateListDto,
  ApiCfcproductionstatesClient,
  ApiWorkstationsCamerasClient,
  WorkStationCameraListDto,
  WorkStationCameraCreateDto,
  WorkStationCameraDetailsDto,
  MachinesDropdownDto,
  ApiMachinesDropdownMachinesClient
} from '../../../shared/clients';
import { Observable } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import {
  IWorkstationsListModel,
  IWorkstationsFilterModel,
  IWorkstationDetailsModel,
  IWorkstationDropdownModel,
  ICameraDropdownModel,
  ICFCProductionStateDropdownModel,
  IWorkstationCameraCreateModel,
  IWorkstationCameraDetailsModel,
  IMachineDropdownModel
} from '../models/workstations.model';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class WorkstationService {

  constructor(
    private workstationsClient: ApiWorkstationsClient,
    private typesClient: ApiWorkstationtypesClient,
    private activateClient: ApiWorkstationsActivateClient,
    private camerasClient: ApiCamerasNamesClient,
    private cfcProductionStatesClient: ApiCfcproductionstatesClient,
    private workstationsCamerasClient: ApiWorkstationsCamerasClient,
    private machinesClient: ApiMachinesDropdownMachinesClient
  ) { }

  getWorkstationList(filter: IWorkstationsFilterModel): Observable<PagedData<IWorkstationsListModel>> {
    return this.workstationsClient.getWorkStations(
      filter.name,
      filter.optimalCrew,
      filter.machine,
      filter.type,
      filter.status,
      undefined,
      filter.pager.pageIndex,
      filter.pager.pageSize
    ).pipe(
      map((dto: PagedListDtoOfWorkStationListDto): PagedData<IWorkstationsListModel> => ({
        items: dto.data.map((x: WorkStationListDto): IWorkstationsListModel => ({
          id: x.id,
          machine: x.machineName.name,
          name: x.name,
          optimalCrew: x.optimalCrew,
          statusText: x.status ? 'active' : 'deactivated',
          type: x.workStationType.translation,
          status: x.status
        })),
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        totalCount: dto.totalCount
      }))
    );
  }

  getWorkstation(id: number): Observable<IWorkstationDetailsModel> {
    return this.workstationsClient.getWorkStation(id).pipe(
      map((dto: WorkStationDetailsDto): IWorkstationDetailsModel => ({
        name: dto.name,
        machine: dto.machineId,
        optimalCrew: dto.optimalCrew,
        type: dto.workStationTypeId
      }))
    );
  }

  postWorkstation(workstation: IWorkstationDetailsModel): Observable<number> {
    const dto = new WorkStationCreateDto({
      isActive: workstation.isActive,
      machineId: workstation.machine,
      name: workstation.name,
      optimalCrew: workstation.optimalCrew,
      workStationTypeId: workstation.type
    });
    return this.workstationsClient.createWorkStation(dto);
  }

  putWorkstation(id: number, workstation: IWorkstationDetailsModel): Observable<void> {
    const dto = new WorkStationUpdateDto({
      machineId: workstation.machine,
      name: workstation.name,
      optimalCrew: workstation.optimalCrew,
      workStationTypeId: workstation.type
    });

    return this.workstationsClient.updateWorkStation(id, dto);
  }

  putWorkstationCameras(id: number, cameras: IWorkstationCameraCreateModel): Observable<void> {
    const dto = new WorkStationCameraCreateDto({start: new WorkStationCameraListDto({
      cameraId: cameras.start.camera,
      cfcProductionStateId: cameras.start.state
    }), finish: new WorkStationCameraListDto({
      cameraId: cameras.finish.camera,
      cfcProductionStateId: cameras.finish.state
    })
  });

    return this.workstationsCamerasClient.addCameras(id, dto);
  }

  deleteWorkstation(id: number): Observable<void> {
    return this.workstationsClient.deleteWorkStation(id);
  }

  getWorkstationCameras(id: number): Observable<IWorkstationCameraDetailsModel> {
    return this.workstationsCamerasClient.getCameras(id).pipe(
      map((dto: WorkStationCameraDetailsDto): IWorkstationCameraDetailsModel => ({
        start: {
          camera: dto.start.cameraId,
          state: dto.start.cfcProductionStateId
        },
        finish: dto.finish != null ? {
          camera: dto.finish.cameraId,
          state: dto.finish.cfcProductionStateId
        } : null
      }))
    );
  }

  getWorkstationTypes(): Observable<IWorkstationDropdownModel[]> {
    return this.typesClient.getWorkStationTypes().pipe(
      map((dto: WorkStationTypeListDto[]): IWorkstationDropdownModel[] => {
        return dto.map((x: WorkStationTypeListDto): IWorkstationDropdownModel => ({
          id: x.id,
          name: x.translation ? x.translation : 'Nincs translation'
        }));
      })
    );
  }

  getCameras(id: number): Observable<ICameraDropdownModel[]> {
    return this.camerasClient.getCameraNameList(id).pipe(
      map((dto: CameraNameListDto[]): ICameraDropdownModel[] => {
        return dto.map((x: CameraNameListDto): ICameraDropdownModel => ({
          id: x.id,
          name: x.name ? x.name : 'Nincs translation'
        }));
      })
    );
  }

  getMachines(): Observable<IMachineDropdownModel[]> {
    return this.machinesClient.getMachinesDropdown().pipe(
      map((dto: MachinesDropdownDto[]): IMachineDropdownModel[] => {
        return dto.map((x: MachinesDropdownDto): IMachineDropdownModel => ({
          id: x.id,
          name: x.name ? x.name : 'Nincs translation'
        }));
      })
    );
  }

  getCFCProductionStates(): Observable<ICFCProductionStateDropdownModel[]> {
    return this.cfcProductionStatesClient.getCFCProductionStates().pipe(
      map((dto: CFCProductionStateListDto[]): ICFCProductionStateDropdownModel[] => {
        return dto.map((x: CFCProductionStateListDto): ICameraDropdownModel => ({
          id: x.id,
          name: x.name ? x.name : 'Nincs translation'
        }));
      })
    );
  }

  activateWorkstation(id: number): Observable<void> {
    return this.activateClient.setAvailabilityOfWorkStation(id);
  }
}
