import { Injectable } from '@angular/core';
import {
  ApiRequiredmaterialsSelectedClient,
  SelectedRequiredMaterialsDto,
  TempCargoDetailsForRequiredMaterialsDto,
  ApiRequiredmaterialsClient,
  PriceCreateDto,
  AddressCreateDto,
  PagedListDtoOfRequiredMaterialsListDto,
  ApiRequiredmaterialsDropdownClient,
  MaterialCodeListDto,
  ApiCompaniesDropdownClient,
  SupplierDropdownListDto,
  ApiCurrenciesClient,
  CurrencyDto,
  ApiCountriesClient,
  CountryListDto,
  CargoCreateDto,
  ApiCargosClient,
  OrderedMaterialPackageCreateDto,
  ApiCargosPreviewClient,
  ApiRequiredmaterialsExportClient,
} from '../../../shared/clients';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { format } from 'date-fns';
import {
  ISupplyOrderListViewModel,
  ISupplyOrderFilterViewModel,
  ISupplyDropdownModel,
  ISupplyDropModel
} from '../models/supply-orders.model';
import { PagedData } from '../../../shared/models/paged-data.model';
import { ICargoPreviewModel, ICargoPreviewListViewModel } from '../models/cargo-preview.model';
import { IDropDownViewModel } from '../models/cargo-shipping.model';
import {
  TempCargoDetailsForRequiredMaterialsViewModel,
  ICargoShippingViewModel
} from '../models/supply-material.models';

@Injectable({
  providedIn: 'root'
})
export class SupplyService {

  constructor(
    private requiredMaterialsSelectedClient: ApiRequiredmaterialsSelectedClient,
    private requiredMaterialsClient: ApiRequiredmaterialsClient,
    private materialDropdownClient: ApiRequiredmaterialsDropdownClient,
    private companyDropdownClient: ApiCompaniesDropdownClient,
    private currencyDropdownClient: ApiCurrenciesClient,
    private countryDropdownClient: ApiCountriesClient,
    private cargosClient: ApiCargosClient,
    private previewClient: ApiCargosPreviewClient,
    private csvClient: ApiRequiredmaterialsExportClient
  ) { }

  downloadCsv(filter: ISupplyOrderFilterViewModel): Observable<any> {
    return this.csvClient.exportCsv(filter.orderId,
      filter.workingNumber,
      filter.material,
      filter.name,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined);
  }

  sendOrderIds(ids: number[], supplierId: number, bookedById: number): Observable<TempCargoDetailsForRequiredMaterialsViewModel> {
    const requiredIds = new SelectedRequiredMaterialsDto({
      requiredMaterialsIds: ids,
      supplierId: supplierId,
      bookedById: bookedById
    });
    return this.requiredMaterialsSelectedClient.createSelectedRequiredMaterial(requiredIds).pipe(
      map((dto: TempCargoDetailsForRequiredMaterialsDto): TempCargoDetailsForRequiredMaterialsViewModel => ({
        cargoDetailsBeforeSaveCargo: dto.cargoDetailsBeforeSaveCargo ? ({
          cargoName: dto.cargoDetailsBeforeSaveCargo.cargoName ? dto.cargoDetailsBeforeSaveCargo.cargoName : undefined,
          bookedBy: dto.cargoDetailsBeforeSaveCargo.bookedBy ? dto.cargoDetailsBeforeSaveCargo.bookedBy : undefined,
          createdOn: dto.cargoDetailsBeforeSaveCargo.createdOn,
          contractingPartyName: dto.cargoDetailsBeforeSaveCargo.contractingPartyName ?
            dto.cargoDetailsBeforeSaveCargo.contractingPartyName : undefined,
          supplierName: dto.cargoDetailsBeforeSaveCargo.supplierName ? dto.cargoDetailsBeforeSaveCargo.supplierName : undefined,
          netCost: dto.cargoDetailsBeforeSaveCargo
            ?
            ({
              value: dto.cargoDetailsBeforeSaveCargo.netCost.value ? dto.cargoDetailsBeforeSaveCargo.netCost.value : 0,
              currencyId: dto.cargoDetailsBeforeSaveCargo.netCost.currencyId ?
                dto.cargoDetailsBeforeSaveCargo.netCost.currencyId : undefined,
              currency: dto.cargoDetailsBeforeSaveCargo.netCost.currency ? dto.cargoDetailsBeforeSaveCargo.netCost.currency : undefined
            }) :
            undefined,
          vatValue: dto.cargoDetailsBeforeSaveCargo.vatValue,
          vat: dto.cargoDetailsBeforeSaveCargo.vat
            ?
            ({
              value: dto.cargoDetailsBeforeSaveCargo.vat.value ? dto.cargoDetailsBeforeSaveCargo.vat.value : 0,
              currencyId: dto.cargoDetailsBeforeSaveCargo.vat.currencyId ? dto.cargoDetailsBeforeSaveCargo.vat.currencyId : undefined,
              currency: dto.cargoDetailsBeforeSaveCargo.vat.currency ? dto.cargoDetailsBeforeSaveCargo.vat.currency : undefined
            }) :
            undefined,
          totalGrossCost: dto.cargoDetailsBeforeSaveCargo.totalGrossCost
            ?
            ({
              value: dto.cargoDetailsBeforeSaveCargo.netCost.value ?
                dto.cargoDetailsBeforeSaveCargo.totalGrossCost.value : 0,
              currencyId: dto.cargoDetailsBeforeSaveCargo.totalGrossCost.currencyId ?
                dto.cargoDetailsBeforeSaveCargo.totalGrossCost.currencyId : undefined,
              currency: dto.cargoDetailsBeforeSaveCargo.totalGrossCost.currency ?
                dto.cargoDetailsBeforeSaveCargo.totalGrossCost.currency : undefined
            }) : undefined,
        }) : undefined,
        materials: dto.materials ? dto.materials.map(materials =>
          ({
            materialCode: materials.materialCode ? materials.materialCode : undefined,
            name: materials.name ? materials.name : undefined,
            materialPackages: materials.materialPackages ? materials.materialPackages.map(packageMaterial =>
              ({
                id: packageMaterial.id,
                name: packageMaterial.packageName,
                packageSize: packageMaterial.packageSize,
                price: packageMaterial.price ? ({
                  value: packageMaterial.price.value ? packageMaterial.price.value : undefined,
                  currencyId: packageMaterial.price.currencyId ? packageMaterial.price.currencyId : undefined,
                  currency: packageMaterial.price.currency ? packageMaterial.price.currency : undefined,
                }) : undefined,
              })) : undefined,
            materialPackagesSelected: undefined,
            requiredAmount: materials.requiredAmount,
            stockedAmount: materials.stockedAmount,
            minAmount: materials.minAmount,
            advisedAmount: materials.advisedAmount,
            underOrderAmount: materials.underOrderAmount,
            orderdAmount: undefined
          })
        ) : undefined,
        additionals: dto.additionals ? dto.additionals.map(additionals =>
          ({
            materialCode: additionals.materialCode ? additionals.materialCode : undefined,
            name: additionals.name ? additionals.name : undefined,
            materialPackages: additionals.materialPackages ? additionals.materialPackages.map(packageMaterial =>
              ({
                id: packageMaterial.id,
                name: packageMaterial.packageName,
                packageSize: packageMaterial.packageSize,
                price: packageMaterial.price ? ({
                  value: packageMaterial.price.value ? packageMaterial.price.value : undefined,
                  currencyId: packageMaterial.price.currencyId ? packageMaterial.price.currencyId : undefined,
                  currency: packageMaterial.price.currency ? packageMaterial.price.currency : undefined,
                }) : undefined,
              })) : undefined,
            materialPackagesSelected: undefined,
            stockedAmount: additionals.stockedAmount,
            minAmount: additionals.minAmount,
            advisedAmount: additionals.advisedAmount,
            underOrderAmount: additionals.underOrderAmount,
            orderdAmount: undefined
          })
        ) : undefined
      }))
    );
  }

  postCargo(cargo: TempCargoDetailsForRequiredMaterialsViewModel, bookedById: number, supplierId: number, shipping: ICargoShippingViewModel): Observable<number> {
    const dto = new CargoCreateDto({
      cargoName: cargo.cargoDetailsBeforeSaveCargo ? cargo.cargoDetailsBeforeSaveCargo.cargoName : undefined,
      bookedById: bookedById,
      supplierId: supplierId,
      shippingCost: shipping ? new PriceCreateDto({
        value: shipping.shippingCost,
        currencyId: shipping.currencyId,
      }) : undefined,
      shippingAddress: shipping ? new AddressCreateDto({
        address: shipping.shippingAddress ? shipping.shippingAddress.address : undefined,
        postCode: shipping.shippingAddress ? shipping.shippingAddress.postCode : undefined,
        city: shipping.shippingAddress ? shipping.shippingAddress.city : undefined,
        countryId: shipping.shippingAddress ? shipping.shippingAddress.countryId : undefined,
      }) : undefined,
      notes: shipping ? shipping.note : undefined,
      additionals: [],
      materials: cargo.materials ? cargo.materials.filter(res => res.materialPackagesSelected).map(res => new OrderedMaterialPackageCreateDto({
        packageId: res.materialPackagesSelected ? res.materialPackagesSelected.id : undefined,
        orderedPackageNum: res.orderdAmount
      })) : undefined,
    });
    cargo.additionals.forEach(x => {
      if (x.materialPackagesSelected) {
        const temp = new OrderedMaterialPackageCreateDto({
          packageId: x.materialPackagesSelected.id,
          orderedPackageNum: x.orderdAmount ? x.orderdAmount : null
        });
        dto.additionals.push(temp);
      }
    });
    return this.cargosClient.createCargoFromRequiredMaterials(dto);
  }

  listOrders(filter: ISupplyOrderFilterViewModel): Observable<PagedData<ISupplyOrderListViewModel>> {
    return this.requiredMaterialsClient.list(
      filter.orderId,
      filter.workingNumber,
      filter.material,
      filter.name,
      undefined,
      undefined,
      undefined,
      filter.pager.pageIndex,
      filter.pager.pageSize
    ).pipe(
      map((dto: PagedListDtoOfRequiredMaterialsListDto): PagedData<ISupplyOrderListViewModel> => ({
        items: dto.data.map((x): ISupplyOrderListViewModel => ({
          id: x.id,
          amount: x.amount,
          deadline: format(x.deadline, 'yyyy. MM. dd.'),
          material: x.materialCode,
          name: x.name,
          orderId: x.orderName,
          supplier: x.suppliers.map((res): ISupplyDropModel => ({
            id: res.id,
            name: res.supplierName,
          })),
          workingNumber: x.workingNumber
        })),
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        totalCount: dto.totalCount
      }))
    );
  }

  getPreview(id: number): Observable<ICargoPreviewModel> {
    return this.previewClient.previewCargoDetails(id).pipe(
      map((dto): ICargoPreviewModel => ({
        cargoId: dto.cargoName,
        contractingCompany: {
          contactPersonName: dto.contractingParty.userName,
          companyName: dto.contractingParty.companyName,
          address: {
            address: dto.contractingParty.companyAddress.address,
            city: dto.contractingParty.companyAddress.city,
            postCode: dto.contractingParty.companyAddress.postCode
          },
          email: dto.contractingParty.email,
          phone: dto.contractingParty.phone,
        },
        supplierCompany: {
          address: {
            address: dto.supplier.supplierAddress.address,
            city: dto.supplier.supplierAddress.city,
            postCode: dto.supplier.supplierAddress.postCode
          },
          companyName: dto.supplier.companyName,
          contactPersonName: dto.supplier.contactPerson.name,
          phone: dto.supplier.contactPerson.phone,
          email: dto.supplier.contactPerson.email
        },
        shipping: {
          name: dto.shipping.name,
          note: dto.shipping.note,
          phone: dto.shipping.phone,
          address: {
            address: dto.shipping.shippingAddress.address,
            city: dto.shipping.shippingAddress.city,
            postCode: dto.shipping.shippingAddress.postCode
          }
        },
        items: dto.orderedPackages.map((x): ICargoPreviewListViewModel => ({
          amount: x.amount,
          currency: x.packagePrice.currency,
          material: x.materialCode,
          name: x.materialName,
          packageCode: x.packageCode,
          packageName: x.packageName,
          packageSize: x.packageSize,
          price: x.packagePrice.value,
          subTotal: x.subtotal.value,
        })),
        shippingPrice: dto.previewCost.shipping.value,
        subtotal: dto.previewCost.subTotal.value,
        total: dto.previewCost.total.value,
        vat: dto.previewCost.vat.value
      }))
    );
  }

  getMaterials(): Observable<ISupplyDropdownModel[]> {
    return this.materialDropdownClient.materialCodeList().pipe(
      map((dto: MaterialCodeListDto[]): ISupplyDropdownModel[] => {
        return dto.map((x: MaterialCodeListDto): ISupplyDropdownModel => ({
          id: x.id,
          name: x.materialCode
        }));
      })
    );
  }

  getCompanies(): Observable<ISupplyDropModel[]> {
    return this.companyDropdownClient.supplierDropdownList().pipe(
      map((dto: SupplierDropdownListDto[]): ISupplyDropModel[] => {
        return dto.map((x: SupplierDropdownListDto): ISupplyDropModel => ({
          id: x.id,
          name: x.supplierName
        }));
      })
    );
  }

  getCurrencies(): Observable<IDropDownViewModel[]> {
    return this.currencyDropdownClient.getCurrencies().pipe(
      map((dto: CurrencyDto[]): IDropDownViewModel[] => {
        return dto.map((x: CurrencyDto): IDropDownViewModel => ({
          id: x.id,
          translation: x.name
        }));
      })
    );
  }

  getCountries(): Observable<IDropDownViewModel[]> {
    return this.countryDropdownClient.getCountries().pipe(
      map((dto: CountryListDto[]): IDropDownViewModel[] => {
        return dto.map((x: CountryListDto): IDropDownViewModel => ({
          id: x.id,
          translation: x.translation
        }));
      })
    );
  }

}
