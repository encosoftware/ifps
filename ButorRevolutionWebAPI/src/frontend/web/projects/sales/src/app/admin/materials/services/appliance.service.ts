import { Injectable } from '@angular/core';
import { IAppliancesListViewModel, IAppliancesModel, IAppliancesFilterModel, IBrandListModel } from '../models/appliences.model';
import { Observable } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { map } from 'rxjs/operators';
import {
  ApiAppliancesClient,
  PagedListDtoOfApplianceMaterialListDto,
  ApplianceMaterialCreateDto,
  ApiCompaniesClient,
  CompanyTypeEnum,
  PagedListDtoOfCompanyListDto,
  ImageCreateDto,
  PriceCreateDto,
  ApplianceMaterialDetailsDto,
  ApplianceMaterialUpdateDto,
  ImageUpdateDto,
  PriceUpdateDto,
  GroupingCategoryEnum,
  GroupingCategoryListDto,
  ApiGroupingcategoriesFlatListClient,
  ApiCurrenciesClient,
  CurrencyDto
} from '../../../shared/clients';
import { IGroupingModel } from '../models/worktops.model';
import { ISelectItemModel } from '../../../shared/models/select-items.model';

@Injectable({
  providedIn: 'root'
})
export class ApplianceService {

  constructor(
    private client: ApiAppliancesClient,
    private companyClient: ApiCompaniesClient,
    private groupingClient: ApiGroupingcategoriesFlatListClient,
    private currencyClient: ApiCurrenciesClient,
  ) { }

  postAppliance(appliance: IAppliancesModel): Observable<string> {
    const dto = new ApplianceMaterialCreateDto({
      code: appliance.code,
      categoryId: appliance.categoryId,
      companyId: appliance.brandId,
      description: appliance.description,
      hanaCode: appliance.hanaCode,
      imageCreateDto: new ImageCreateDto({
        containerName: appliance.picture.containerName,
        fileName: appliance.picture.fileName
      }),
      purchasingPrice: new PriceCreateDto({
        currencyId: appliance.purchasingCurrencyId,
        value: appliance.purchasingPrice
      }),
      sellPrice: new PriceCreateDto({
        currencyId: appliance.sellingCurrencyId,
        value: appliance.sellPrice
      })
    });
    return this.client.createApplianceMaterial(dto);
  }

  getAppliance(id: string): Observable<IAppliancesModel> {
    return this.client.getApplianceMaterialById(id).pipe(
      map((dto: ApplianceMaterialDetailsDto): IAppliancesModel => ({
        id: dto.id,
        code: dto.code,
        brandId: dto.companyId,
        categoryId: dto.categoryId,
        description: dto.description,
        hanaCode: dto.hanaCode,
        picture: {
          containerName: dto.image.containerName,
          fileName: dto.image.fileName
        },
        purchasingCurrencyId: dto.purchasingPrice.currencyId,
        purchasingPrice: dto.purchasingPrice.value,
        sellingCurrencyId: dto.sellPrice.currencyId,
        sellPrice: dto.sellPrice.value
      }))
    );
  }

  getApplianceList(filter: IAppliancesFilterModel): Observable<PagedData<IAppliancesListViewModel>> {
    return this.client.getAccessoryMaterials(
      filter.brand,
      filter.code,
      filter.description,
      filter.categoryId,
      filter.hanaCode,
      null,
      filter.pager.pageIndex,
      filter.pager.pageSize
    ).pipe(
      map((dto: PagedListDtoOfApplianceMaterialListDto): PagedData<IAppliancesListViewModel> => ({
        items: dto.data.map(x => ({
          id: x.id,
          code: x.code,
          description: x.description,
          category: x.category.name,
          categoryId: x.category.id,
          brand: x.brand,
          hanaCode: x.hanaCode,
          purchasingPrice: x.purchasingPrice.value,
          purchasingCurrency: x.purchasingPrice.currency,
          sellPrice: x.sellPrice.value,
          sellingCurrency: x.sellPrice.currency,
          currencyId: x.purchasingPrice.currencyId,
          picture: {
            fileName: x.image.fileName,
            containerName: x.image.containerName
          }
        })),
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        totalCount: dto.totalCount
      }))
    );
  }

  putAppliance(id: string, appliance: IAppliancesListViewModel): Observable<void> {
    const dto = new ApplianceMaterialUpdateDto({
      categoryId: appliance.categoryId,
      companyId: appliance.brandId,
      description: appliance.description,
      hanaCode: appliance.hanaCode,
      imageUpdateDto: new ImageUpdateDto({
        containerName: appliance.picture.containerName,
        fileName: appliance.picture.fileName
      }),
      purchasingPrice: new PriceUpdateDto({
        currencyId: appliance.purchasingCurrencyId,
        value: appliance.purchasingPrice
      }),
      sellPrice: new PriceUpdateDto({
        currencyId: appliance.sellingCurrencyId,
        value: appliance.sellPrice
      })
    });
    return this.client.putApplianceMaterial(id, dto);
  }

  deleteAppliance(id: string): Observable<void> {
    return this.client.deleteApplianceMaterial(id);
  }

  getCompanies(): Observable<IBrandListModel[]> {
    return this.companyClient.get('', CompanyTypeEnum.None, '', '', null, undefined, undefined).pipe(
      map((dto: PagedListDtoOfCompanyListDto): IBrandListModel[] => {
        const retObj: IBrandListModel[] = [];
        dto.data.forEach(x => {
          const tempCompany: IBrandListModel = {
            id: x.id,
            name: x.name
          };
          retObj.push(tempCompany);
        });
        return retObj;
      })
    );
  }

  getCategories(): Observable<IGroupingModel[]> {
    return this.groupingClient.getRootCategories(GroupingCategoryEnum.Appliances, undefined).pipe(
      map((dto: GroupingCategoryListDto[]): IGroupingModel[] => {
        const retObj: IGroupingModel[] = [];
        dto.forEach(x => {
          const temp: IGroupingModel = {
            id: x.id,
            name: x.name
          };
          retObj.push(temp);
        });
        return retObj;
      })
    );
  }

  getCurrencies(): Observable<ISelectItemModel[]> {
    return this.currencyClient.getCurrencies().pipe(
      map((dto: CurrencyDto[]): ISelectItemModel[] => {
        return dto.map((item: CurrencyDto): ISelectItemModel => ({
          options: item.name,
          value: item.id
        }));
      })
    );
  }
}
