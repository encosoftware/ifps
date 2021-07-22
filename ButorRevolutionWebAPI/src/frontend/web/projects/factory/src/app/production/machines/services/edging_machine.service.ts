import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PagedData } from '../../../shared/models/paged-data.model';
import { IEdgingMachinesDetailsViewModel, IEdgingMachinesCreateViewModel, IEdgingMachinesFilterViewModel, IEdgingMachinesListViewModel, ISupplierDropdownModel } from '../models/edging_machines.model';
import { EdgingMachineDetailsDto, EdgingMachineUpdateDto, EdgingMachineCreateDto, PagedListDtoOfEdgingMachineListDto, ApiEdgebandingClient, ApiEdgingmachinesClient, ApiCompaniesDropdownClient, SupplierDropdownListDto } from '../../../shared/clients';

@Injectable({
  providedIn: 'root'
})
export class EdgingMachineService {

  constructor(
    private edgingMachineClient: ApiEdgingmachinesClient,
    private companiesDrpdownClient: ApiCompaniesDropdownClient
  ) { }

  getEdgingMachines(filter: IEdgingMachinesFilterViewModel): Observable<PagedData<IEdgingMachinesListViewModel>> {
    return this.edgingMachineClient.getEdgingMachines(
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
      map((dto: PagedListDtoOfEdgingMachineListDto): PagedData<IEdgingMachinesListViewModel> => ({
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

  getEdgingMachine(id: number): Observable<IEdgingMachinesDetailsViewModel> {
    return this.edgingMachineClient.getEdgingMachine(id).pipe(map(this.deatailsToViewModel));
  }

  private deatailsToViewModel(dto: EdgingMachineDetailsDto): IEdgingMachinesDetailsViewModel {
    return {
      code: dto.code,
      machineName: dto.machineName,
      serialNumber: dto.serialNumber,
      softwareVersion: dto.softwareVersion,
      yearOfManufacture: dto.yearOfManufacture,
      brandId: dto.brandId
    };
  }

  deleteEdgingMachine(id: number): Observable<void> {
    return this.edgingMachineClient.deleteEdgingMachine(id);
  }

  updateEdgingMachine(id: number, model: IEdgingMachinesDetailsViewModel): Observable<void> {
    let dto = new EdgingMachineUpdateDto({
      code: model.code,
      machineName: model.machineName,
      serialNumber: model.serialNumber,
      softwareVersion: model.softwareVersion,
      yearOfManufacture: model.yearOfManufacture,
      brandId: model.brandId
    });
    return this.edgingMachineClient.updateEdgingMachine(id, dto);
  }

  createEdgingMachine(model: IEdgingMachinesCreateViewModel): Observable<number> {
    let dto = new EdgingMachineCreateDto({
      machineName: model.machineName,
      serialNumber: model.serialNumber,
      softwareVersion: model.softwareVersion,
      code: model.code,
      yearOfManufacture: model.yearOfManufacture,
      brandId: model.brandId
    });
    return this.edgingMachineClient.createEdgingMachine(dto);
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
