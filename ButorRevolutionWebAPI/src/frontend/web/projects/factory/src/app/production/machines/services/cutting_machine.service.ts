import { Injectable } from '@angular/core';
import {
  ApiCuttingmachinesClient,
  PagedListDtoOfCuttingMachineListDto,
  CuttingMachineDetailsDto,
  CuttingMachineUpdateDto,
  CuttingMachineCreateDto,
  ApiCompaniesDropdownClient,
  SupplierDropdownListDto,
} from '../../../shared/clients';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PagedData } from '../../../shared/models/paged-data.model';
import { ICuttingMachinesListViewModel, ICuttingMachinesFilterViewModel, ICuttingMachinesDetailsViewModel, ICuttingMachinesCreateViewModel, ISupplierDropdownModel } from '../models/cutting_machines.model';

@Injectable({
  providedIn: 'root'
})
export class CuttingMachineService {

  constructor(
    private cuttingMachineClient: ApiCuttingmachinesClient,
    private companiesDrpdownClient: ApiCompaniesDropdownClient
  ) { }

  getCuttingMachines(filter: ICuttingMachinesFilterViewModel): Observable<PagedData<ICuttingMachinesListViewModel>> {
    return this.cuttingMachineClient.getCuttingMachines(
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
      map((dto: PagedListDtoOfCuttingMachineListDto): PagedData<ICuttingMachinesListViewModel> => ({
        items: dto.data.map(x => ({
          id: x.id,
          machineName: x.machineName,
          serialNumber: x.serialNumber,
          softwareVersion: x.softwareVersion,
          code: x.code,
          yearOfManufacture: x.yearOfManufacture,
          brandName: x.brandName
        })),
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        totalCount: dto.totalCount
      }))
    );
  }

  getCuttingMachine(id: number): Observable<ICuttingMachinesDetailsViewModel> {
    return this.cuttingMachineClient.getCuttingMachine(id).pipe(map(this.deatailsToViewModel));
  }

  private deatailsToViewModel(dto: CuttingMachineDetailsDto): ICuttingMachinesDetailsViewModel {
    return {
      code: dto.code,
      machineName: dto.machineName,
      serialNumber: dto.serialNumber,
      softwareVersion: dto.softwareVersion,
      yearOfManufacture: dto.yearOfManufacture,
      brandId: dto.brandId
    };
  }

  deleteCuttingMachine(id: number): Observable<void> {
    return this.cuttingMachineClient.deleteCuttingMachine(id);
  }

  updateCuttingMachine(id: number, model: ICuttingMachinesDetailsViewModel): Observable<void> {
    let dto = new CuttingMachineUpdateDto({
      code: model.code,
      machineName: model.machineName,
      serialNumber: model.serialNumber,
      softwareVersion: model.softwareVersion,
      yearOfManufacture: model.yearOfManufacture,
      brandId: model.brandId
    });
    return this.cuttingMachineClient.updateCuttingMachine(id, dto);
  }

  createCuttingMachine(model: ICuttingMachinesCreateViewModel): Observable<number> {
    let dto = new CuttingMachineCreateDto({
      machineName: model.machineName,
      serialNumber: model.serialNumber,
      softwareVersion: model.softwareVersion,
      code: model.code,
      yearOfManufacture: model.yearOfManufacture,
      brandId: model.brandId
    });
    return this.cuttingMachineClient.createCuttingMachine(dto);
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
