import { Injectable } from '@angular/core';
import {
  PagedListDtoOfCncMachineListDto, CncMachineCreateDto, CncMachineUpdateDto, CncMachineDetailsDto, ApiCncmachinesClient, ApiCompaniesDropdownClient, SupplierDropdownListDto,
} from '../../../shared/clients';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { PagedData } from '../../../shared/models/paged-data.model';
import { ICncMachinesFilterViewModel, ICncMachinesListViewModel, ICncMachinesDetailsViewModel, ICncMachinesCreateViewModel, ISupplierDropdownModel } from '../models/cnc_machines.model';

@Injectable({
  providedIn: 'root'
})
export class CncMachineService {

  constructor(
    private cncMachineClient: ApiCncmachinesClient,
    private companiesDrpdownClient: ApiCompaniesDropdownClient
  ) { }

  getCncMachines(filter: ICncMachinesFilterViewModel): Observable<PagedData<ICncMachinesListViewModel>> {
    return this.cncMachineClient.getCncMachines(
      filter.machineName,
      filter.softwareVersion,
      filter.serialNumber,
      filter.code,
      filter.yearOfManufacture === undefined ? undefined : +filter.yearOfManufacture,
      filter.brandId === undefined ? undefined : +filter.brandId,
      undefined,
      filter.pager.pageIndex,
      filter.pager.pageSize
    ).pipe(
      map((dto: PagedListDtoOfCncMachineListDto): PagedData<ICncMachinesListViewModel> => ({
        items: dto.data.map(x => ({
          id: x.id,
          machineName: x.machineName,
          serialNumber: x.serialNumber,
          softwareVersion: x.softwareVersion,
          code: x.code,
          yearOfManufacture: x.yearOfManufacture,
          brandName: x.brandName,
          sharedFolderPath: x.sharedFolderPath
        })),
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        totalCount: dto.totalCount
      }))
    );
  }

  getCncMachine(id: number): Observable<ICncMachinesDetailsViewModel> {
    return this.cncMachineClient.getCncMachine(id).pipe(map(this.deatailsToViewModel));
  }

  private deatailsToViewModel(dto: CncMachineDetailsDto): ICncMachinesDetailsViewModel {
    return {
      code: dto.code,
      machineName: dto.machineName,
      serialNumber: dto.serialNumber,
      softwareVersion: dto.softwareVersion,
      yearOfManufacture: dto.yearOfManufacture,
      brandId: dto.brandId,
      sharedFolderPath: dto.sharedFolderPath
    };
  }

  deleteCncMachine(id: number): Observable<void> {
    return this.cncMachineClient.deleteCncMachine(id);
  }

  updateCncMachine(id: number, model: ICncMachinesDetailsViewModel): Observable<void> {
    let dto = new CncMachineUpdateDto({
      code: model.code,
      machineName: model.machineName,
      serialNumber: model.serialNumber,
      softwareVersion: model.softwareVersion,
      yearOfManufacture: model.yearOfManufacture,
      brandId: model.brandId,
      sharedFolderPath: model.sharedFolderPath
    });
    return this.cncMachineClient.updateCncMachine(id, dto);
  }

  createCncMachine(model: ICncMachinesCreateViewModel): Observable<number> {
    let dto = new CncMachineCreateDto({
      machineName: model.machineName,
      serialNumber: model.serialNumber,
      softwareVersion: model.softwareVersion,
      code: model.code,
      yearOfManufacture: model.yearOfManufacture,
      brandId: model.brandId,
      sharedFodlerPath: model.sharedFolderPath
    });
    return this.cncMachineClient.createCncMachine(dto);
  }

  getSuppliers(): Observable<ISupplierDropdownModel[]> {
    return this.companiesDrpdownClient.supplierDropdownList().pipe(
      map((dto: SupplierDropdownListDto[]): ISupplierDropdownModel[] => {
        return dto.map((x: SupplierDropdownListDto): ISupplierDropdownModel => ({
          id: x.id,
          name: x.supplierName ? x.supplierName : 'Nincs translation'
        }));
      })
    );
  }
}
