import { Injectable } from '@angular/core';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import {
    ICargoListViewModel,
    ICargoFilterListViewModel,
    ICargoEditViewModel,
    ICargoStatusViewModel,
    ICargoSupplierCompanyViewModel,
    ICargoBookedByViewModel,
    ICargoProductListViewModel
} from '../models/cargo.model';
import {
    ApiCargosBystockClient,
    IPagedListDtoOfCargoListByStockDto,
    CargoStatusEnum,
    ApiCargostatustypesDropdownClient,
    CargoStatusTypeDropdownListDto,
    ApiCompaniesDropdownClient,
    SupplierDropdownListDto,
    ApiUsersDropdownClient,
    BookedByDropdownListDto,
    ApiCargosBystockDetailsClient,
    CargoStockDetailsDto,
    ApiStoragecellsDropdownClient,
    StorageCellDropdownListDto,
    UpdateCargoStockDto,
    OrderedPackageUpdateDto,
    ApiCargosExportClient
} from '../../../shared/clients';
import { ICellDropDownViewModel } from '../../stocked-items/models/stocked-items.model';
import { parse } from 'date-fns';



@Injectable({
    providedIn: 'root'
})
export class CargoService {

    constructor(
        private cargoClient: ApiCargosBystockClient,
        private cargoStatusesClient: ApiCargostatustypesDropdownClient,
        private cargoSupplierClient: ApiCompaniesDropdownClient,
        private bookedByClient: ApiUsersDropdownClient,
        private cargoDetailsClient: ApiCargosBystockDetailsClient,
        private cellClient: ApiStoragecellsDropdownClient,
        private csvClient: ApiCargosExportClient
    ) { }

    downloadCsv(filter: ICargoFilterListViewModel): Observable<any> {
        const tempFrom = filter.arrivedOn ? new Date(filter.arrivedOn) : undefined;
        const tempTo = filter.arrivedOn ? new Date() : undefined;
        return this.csvClient.exportCsv(
            filter.cargoId,
            filter.status,
            tempFrom,
            tempTo,
            filter.supplier,
            filter.bookedBy,
            undefined,
            undefined,
            undefined);
    }

    getCargos(filter: ICargoFilterListViewModel): Observable<PagedData<ICargoListViewModel>> {
        const tempFrom = filter.arrivedOn ? new Date(filter.arrivedOn) : undefined;
        const tempTo = filter.arrivedOn ? new Date() : undefined;
        return this.cargoClient.listCargosByStock(
            filter.cargoId,
            filter.status,
            tempFrom,
            tempTo,
            filter.supplier,
            filter.bookedBy,
            undefined,
            filter.pager.pageIndex,
            filter.pager.pageSize
        ).pipe(
            map((dto: IPagedListDtoOfCargoListByStockDto): PagedData<ICargoListViewModel> => ({
                items: dto.data.map(x => ({
                    id: x.id,
                    cargoId: x.cargoName,
                    status: x.status.translation,
                    statusEnum: CargoStatusEnum[x.status.cargoStatus],
                    arrivedOn: x.arrivedOn,
                    supplier: x.supplierName.name,
                    bookedBy: x.bookedByUser.name
                })),
                pageIndex: filter.pager.pageIndex,
                pageSize: filter.pager.pageSize,
                totalCount: dto.totalCount
            }))
        );
    }

    getCells(): Observable<ICellDropDownViewModel[]> {
        return this.cellClient.storageCellDropdownList().pipe(map(res => res.map(this.cellDropDownToViewModel)));
    }

    private cellDropDownToViewModel(model: StorageCellDropdownListDto): ICellDropDownViewModel {
        return { name: model.name, id: model.id };
    }


    getCargoStatuses(): Observable<ICargoStatusViewModel[]> {
        return this.cargoStatusesClient.cargoStatusTypeDropdownList().pipe(map(res => res.map(this.cargoStatusToViewModel)));
    }

    cargoStatusToViewModel(model: CargoStatusTypeDropdownListDto): ICargoStatusViewModel {
        return { name: model.statusName, id: model.id };
    }

    getCargoSupplierCompanies(): Observable<ICargoSupplierCompanyViewModel[]> {
        return this.cargoSupplierClient.supplierDropdownList().pipe(map(res => res.map(this.supplierToViewModel)));
    }

    supplierToViewModel(model: SupplierDropdownListDto): ICargoSupplierCompanyViewModel {
        return { name: model.supplierName, id: model.id };
    }

    getCargoBookedBy(): Observable<ICargoBookedByViewModel[]> {
        return this.bookedByClient.bookedByDropdownList().pipe(map(res => res.map(this.bookedByToViewModel)));
    }

    bookedByToViewModel(model: BookedByDropdownListDto): ICargoBookedByViewModel {
        return { name: model.name, id: model.id };
    }

    getCargo(id: number): Observable<ICargoEditViewModel> {
        return this.cargoDetailsClient.cargoDetailsByStock(id).pipe(
            map(this.CargoDetailsToViewModel)
        );
    }

    private CargoDetailsToViewModel(dto: CargoStockDetailsDto): ICargoEditViewModel {
        let model: ICargoEditViewModel = {
            basics: {
                cargoId: dto.cargoDetails.cargoName,
                status: dto.cargoDetails.status.translation,
                stockedOn: dto.cargoDetails.stockedOn,
                bookedBy: dto.cargoDetails.bookedBy.name,
                created: dto.cargoDetails.created,
                supplier: dto.cargoDetails.supplier.name,
                grossCost: dto.cargoDetails.totalGrossCost.value,
                grossCostCurrency: dto.cargoDetails.totalGrossCost.currency
            },
            details: {
                cargoId: dto.moreCargoDetails.cargoName,
                contract: {
                    name: dto.moreCargoDetails.contractingParty.companyName,
                    address: dto.moreCargoDetails.contractingParty.companyAddress.postCode + ' ' +
                        dto.moreCargoDetails.contractingParty.companyAddress.city + ' ' +
                        dto.moreCargoDetails.contractingParty.companyAddress.address,
                    person: dto.moreCargoDetails.contractingParty.userName,
                    phone: dto.moreCargoDetails.contractingParty.phone,
                    email: dto.moreCargoDetails.contractingParty.email
                },
                supplier: {
                    name: dto.moreCargoDetails.supplier.companyName,
                    address: dto.moreCargoDetails.supplier.supplierAddress.postCode + ' ' + dto.moreCargoDetails.supplier.supplierAddress.city + ' ' + dto.moreCargoDetails.supplier.supplierAddress.address,
                    person: dto.moreCargoDetails.supplier.contactPerson?.name,
                    phone: dto.moreCargoDetails.supplier.contactPerson?.phone,
                    email: dto.moreCargoDetails.supplier.contactPerson?.email
                },
                shipping: {
                    address: dto.moreCargoDetails.shipping && dto.moreCargoDetails.shipping.shippingAddress ? dto.moreCargoDetails.shipping.shippingAddress.postCode + ' ' + dto.moreCargoDetails.shipping.shippingAddress.city + ' ' + dto.moreCargoDetails.shipping.shippingAddress.address : '',
                    name: dto.moreCargoDetails.shipping.name,
                    phone: dto.moreCargoDetails.shipping.phone,
                    notes: dto.moreCargoDetails.shipping ? dto.moreCargoDetails.shipping.note : ''
                },
                products: []
            }
        };
        for (let item of dto.moreCargoDetails.orderedPackages) {
            model.details.products.push({
                id: item.materialPackageId,
                material: item.materialCode,
                name: item.name,
                workingNr: '',
                arrived: item.arrived,
                refused: item.refused,
                cellId: item.cell.id
            });
        }
        return model;
    }

    putCargoStockDetails(id: number, products: ICargoProductListViewModel[]): Observable<void> {
        let dtoList = new UpdateCargoStockDto({
            package: []
        });
        for (let product of products) {
            let dto = new OrderedPackageUpdateDto({
                packageId: product.id,
                cellId: product.cellId,
                arrivedAmount: product.arrived
            });
            dtoList.package.push(dto);
        }
        return this.cargoDetailsClient.updateCargoByStock(id, dtoList);
    }


}
