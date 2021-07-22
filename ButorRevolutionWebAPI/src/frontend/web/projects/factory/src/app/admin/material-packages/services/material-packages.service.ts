import { Injectable } from '@angular/core';
import {
  ApiMaterialpackagesClient,
  PagedListDtoOfMaterialPackageListDto,
  MaterialPackageListDto,
  MaterialPackageDetailsDto,
  MaterialPackageCreateDto,
  MaterialPackageUpdateDto,
  MaterialListForDropdownDto,
  ApiMaterialsDropdownClient,
  SupplierDropdownListDto,
  ApiCompaniesDropdownClient,
  ApiCurrenciesClient,
  CurrencyDto,
  PriceCreateDto,
  PriceUpdateDto
} from '../../../shared/clients';
import { Observable } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { map } from 'rxjs/operators';
import { IMaterialPackageListModel, IMaterialPackageFilterModel, IMaterialPackageDetailsModel, IMaterialDropdownModel, ISupplierDropdownModel, ICurrencyListViewModel } from '../models/material-packages.model';

@Injectable({
  providedIn: 'root'
})
export class MaterialPackageService {

  constructor(
    private materialPackagesClient: ApiMaterialpackagesClient,
    private materialsDropdownClient: ApiMaterialsDropdownClient,
    private companiesDropdownClient: ApiCompaniesDropdownClient,
    private currencyClient: ApiCurrenciesClient
  ) { }

  getMaterialPackageList(filter: IMaterialPackageFilterModel): Observable<PagedData<IMaterialPackageListModel>> {
    return this.materialPackagesClient.getMaterialPackages(
      filter.code,
      filter.description,
      filter.supplierName,
      filter.size,
      undefined,
      filter.pager.pageIndex,
      filter.pager.pageSize
    ).pipe(
      map((dto: PagedListDtoOfMaterialPackageListDto): PagedData<IMaterialPackageListModel> => ({
        items: dto.data.map((x: MaterialPackageListDto): IMaterialPackageListModel => ({
          id: x.id,
          code: x.code,
          description: x.description,
          size: x.size,
          supplierName: x.supplierName,
          price: x.price.value,
          currency: x.price.currency
        })),
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        totalCount: dto.totalCount
      }))
    );
  }

  getMaterialPackage(id: number): Observable<IMaterialPackageDetailsModel> {
    return this.materialPackagesClient.getMaterialPackageDetails(id).pipe(
      map((dto: MaterialPackageDetailsDto): IMaterialPackageDetailsModel => ({
        materialId: dto.materialId,
        supplierId: dto.supplierId,
        price: dto.price.value,
        currencyId: dto.price.currencyId,
        size: dto.size,
        code: dto.code,
        description: dto.description
      }))
    );
  }

  postMaterialPackage(materialPackage: IMaterialPackageDetailsModel): Observable<number> {
    const dto = new MaterialPackageCreateDto({
      materialId: materialPackage.materialId,
      supplierId: materialPackage.supplierId,
      size: materialPackage.size,
      code: materialPackage.code,
      description: materialPackage.description,
      price: new PriceCreateDto({
        value: materialPackage.price,
        currencyId: materialPackage.currencyId
      })
    });
    return this.materialPackagesClient.createMaterialPackage(dto);
  }

  putMaterialPackage(id: number, materialPackage: IMaterialPackageDetailsModel): Observable<void> {
    const dto = new MaterialPackageUpdateDto({
      materialId: materialPackage.materialId,
      supplierId: materialPackage.supplierId,
      size: materialPackage.size,
      code: materialPackage.code,
      description: materialPackage.description,
      price: new PriceUpdateDto({
        value: materialPackage.price,
        currencyId: materialPackage.currencyId
      })
    });

    return this.materialPackagesClient.updateMaterialPackage(id, dto);
  }

  deleteMaterialPackage(id: number): Observable<void> {
    return this.materialPackagesClient.deleteMaterialPackage(id);
  }

  getMaterials(): Observable<IMaterialDropdownModel[]> {
    return this.materialsDropdownClient.getMaterialsForDropdown().pipe(
      map((dto: MaterialListForDropdownDto[]): IMaterialDropdownModel[] => {
        return dto.map((x: MaterialListForDropdownDto): IMaterialDropdownModel => ({
          id: x.id,
          name: x.materialCode ? x.materialCode : 'Nincs translation'
        }));
      })
    );
  }

  getSuppliers(): Observable<ISupplierDropdownModel[]> {
    return this.companiesDropdownClient.supplierDropdownList().pipe(
      map((dto: SupplierDropdownListDto[]): ISupplierDropdownModel[] => {
        return dto.map((x: SupplierDropdownListDto): ISupplierDropdownModel => ({
          id: x.id,
          name: x.supplierName ? x.supplierName : 'Nincs translation'
        }));
      })
    );
  }

  
  getCurrencies(): Observable<ICurrencyListViewModel[]> {
    return this.currencyClient.getCurrencies().pipe(
      map((dto: CurrencyDto[]): ICurrencyListViewModel[] => {
        return dto.map((x: CurrencyDto): ICurrencyListViewModel => ({
          id: x.id,
          name: x.name ? x.name : 'Nincs translation'
        }));
      })
    );
}
}
