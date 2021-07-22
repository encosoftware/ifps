import { Injectable } from '@angular/core';
import {
  ApiCargosClient,
  PagedListDtoOfCargoListDto,
  CargoDetailsDto,
  ProductListDto,
  CargoUpdateDto,
  ProductsUpdateDto,
  ApiCargostatustypesDropdownClient,
  CargoStatusTypeDropdownListDto,
  ApiCargosDetailsClient,
  CargoDetailsWithAllInformationDto,
  ProductListWithAllInfrmationDto
} from '../../../shared/clients';
import { ICargoFilterModel, ICargoListViewModel, ICargoDetailsViewModel, IProductListViewModel } from '../models/cargo.model';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { format } from 'date-fns';
import { ISupplyDropdownModel, ISupplyDropModel } from '../../supply-orders/models/supply-orders.model';
import { IArrivedCargoViewModel } from '../models/arrived-cargo.model';
import { ICargoPreviewListViewModel } from '../../supply-orders/models/cargo-preview.model';

@Injectable({
  providedIn: 'root'
})
export class CargoService {

  constructor(
    private cargosClient: ApiCargosClient,
    private statusDropdownClient: ApiCargostatustypesDropdownClient,
    private arrivedCargoClient: ApiCargosDetailsClient,
  ) { }

  listCargos(filter: ICargoFilterModel): Observable<PagedData<ICargoListViewModel>> {
    return this.cargosClient.list(
      filter.cargoId,
      filter.status,
      undefined,
      undefined,
      filter.supplier,
      filter.bookedBy,
      undefined,
      filter.pager.pageIndex,
      filter.pager.pageSize
    ).pipe(
      map((dto: PagedListDtoOfCargoListDto): PagedData<ICargoListViewModel> => ({
        items: dto.data.map((x): ICargoListViewModel => ({
          id: x.id,
          bookedBy: x.bookedByUser.name,
          cargoId: x.cargoName,
          created: format(x.createdOn, 'yyyy. MM. dd.'),
          currency: x.totalCost.currency.name,
          status: x.status.translation,
          statusEnum: x.status.cargoStatus,
          statusId: x.status.id,
          supplier: x.supplierName.name,
          totalCost: x.totalCost.value
        })),
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        totalCount: dto.totalCount
      }))
    );
  }

  getConfirmationCargo(id: number): Observable<ICargoDetailsViewModel[]> {
    return this.cargosClient.cargoDetails(id).pipe(
      map((dto: CargoDetailsDto): ICargoDetailsViewModel[] => {
        const retArray: ICargoDetailsViewModel[] = [];
        const tempObj: ICargoDetailsViewModel = {
          cargoId: dto.cargoDetailsForProducts.cargoName,
          bookedBy: dto.cargoDetailsForProducts.bookedByUser.name,
          created: format(dto.cargoDetailsForProducts.created, 'yyyy. MM. dd.'),
          isArrived: false,
          status: dto.cargoDetailsForProducts.status.translation,
          contractingParty: dto.cargoDetailsForProducts.contractingParty,
          currency: dto.cargoDetailsForProducts.netCost.currency,
          netCost: dto.cargoDetailsForProducts.netCost.value,
          totalCost: dto.cargoDetailsForProducts.totalGrossCost.value,
          vat: dto.cargoDetailsForProducts.vat.value,
          supplier: dto.cargoDetailsForProducts.supplierName.name,
          productList: dto.productList.map((x: ProductListDto): IProductListViewModel => ({
            id: x.id,
            isChecked: false,
            material: x.materialCode,
            missing: x.missing,
            name: x.materialName,
            orderedAmount: x.orderedAmount,
            refused: x.refused,
            packageCode: x.packageCode,
            packageName: x.packageName,
            packageSize: x.packageSize,
          }))
        };
        retArray.push(tempObj);
        return retArray;
      })
    );
  }

  updateCargoConfirmation(id: number, cargo: ICargoDetailsViewModel[]): Observable<void> {
    const tempProducts = cargo[0].productList.map((x: IProductListViewModel): ProductsUpdateDto => new ProductsUpdateDto({
      id: x.id,
      missing: x.missing,
      refused: x.refused
    })
    );
    const dto = new CargoUpdateDto({
      products: tempProducts
    });
    return this.cargosClient.updateProductsByCargo(id, dto);
  }

  getArrivedCargo(id: number): Observable<IArrivedCargoViewModel[]> {
    return this.arrivedCargoClient.cargoDetailsWithAllInformation(id).pipe(
      map((dto: CargoDetailsWithAllInformationDto): IArrivedCargoViewModel[] => {
        const retArray: IArrivedCargoViewModel[] = [];
        const tempObj: IArrivedCargoViewModel = {
          cargoId: dto.cargoDetails.cargoName,
          status: dto.cargoDetails.status.translation,
          stockedOn: dto.cargoDetails.stockedOn ? format(dto.cargoDetails.stockedOn, 'yyyy. MM. dd.') : '',
          bookedBy: dto.cargoDetails.bookedBy.name,
          created: format(dto.cargoDetails.created, 'yyyy. MM. dd.'),
          supplier: dto.cargoDetails.supplier.name,
          totalGrossCost: dto.cargoDetails.totalGrossCost.value,
          contractingCompany: {
            address: {
              address: dto.cargoListDetails.contractingParty.companyAddress.address,
              city: dto.cargoListDetails.contractingParty.companyAddress.city,
              postCode: dto.cargoListDetails.contractingParty.companyAddress.postCode
            },
            companyName: dto.cargoListDetails.contractingParty.companyName,
            contactPersonName: dto.cargoListDetails.contractingParty.userName,
            email: dto.cargoListDetails.contractingParty.email,
            phone: dto.cargoListDetails.contractingParty.phone
          },
          supplierCompany: {
            address: {
              address: dto.cargoListDetails.supplier.supplierAddress.address,
              city: dto.cargoListDetails.supplier.supplierAddress.city,
              postCode: dto.cargoListDetails.supplier.supplierAddress.postCode
            },
            companyName: dto.cargoListDetails.supplier.companyName,
            contactPersonName: dto.cargoListDetails.supplier.contactPerson ? dto.cargoListDetails.supplier.contactPerson.name : '',
            email: dto.cargoListDetails.supplier.contactPerson ? dto.cargoListDetails.supplier.contactPerson.email : '',
            phone: dto.cargoListDetails.supplier.contactPerson ? dto.cargoListDetails.supplier.contactPerson.phone : ''
          },
          shipping: {
            address: {
              address: dto.cargoListDetails.shipping.shippingAddress.address,
              city: dto.cargoListDetails.shipping.shippingAddress.city,
              postCode: dto.cargoListDetails.shipping.shippingAddress.postCode
            },
            name: dto.cargoListDetails.shipping.name,
            note: dto.cargoListDetails.shipping.note,
            phone: dto.cargoListDetails.shipping.phone
          },
          items: dto.cargoListDetails.products.map((x: ProductListWithAllInfrmationDto): ICargoPreviewListViewModel => ({
            material: x.materialCode,
            amount: x.orderedAmount,
            missing: x.missing,
            currency: x.subtotal.currency,
            name: x.materialName,
            packageName: x.packageName,
            packageCode: x.packageCode,
            packageSize: x.packageSize,
            price: x.packagePrice.value,
            refused: x.refused,
            subTotal: x.subtotal.value
          })),
          currency: dto.cargoListDetails.cost.shipping.currency,
          shippingPrice: dto.cargoListDetails.cost.shipping.value,
          subtotal: dto.cargoListDetails.cost.subTotal.value,
          total: dto.cargoListDetails.cost.total.value,
          vat: dto.cargoListDetails.cost.vat.value
        };
        retArray.push(tempObj);
        return retArray;
      })
    );
  }

  deleteCargo(id: number): Observable<void> {
    return this.cargosClient.deleteCargo(id);
  }

  getStatuses(): Observable<ISupplyDropModel[]> {
    return this.statusDropdownClient.cargoStatusTypeDropdownList().pipe(
      map((dto: CargoStatusTypeDropdownListDto[]): ISupplyDropModel[] => {
        return dto.map((x: CargoStatusTypeDropdownListDto): ISupplyDropModel => ({
          id: x.id,
          name: x.statusName
        }));
      })
    );
  }
}
