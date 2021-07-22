import { Injectable } from '@angular/core';
import { IAccessoryListViewModel, IAccessoryModel, IAccessoryFilterModel } from '../models/accessories.model';
import { Observable, of } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { map } from 'rxjs/operators';
import {
  ApiAccessoriesClient,
  PagedListDtoOfAccessoryMaterialListDto,
  AccessoryMaterialCreateDto,
  ImageCreateDto,
  PriceCreateDto,
  AccessoryMaterialDetailsDto,
  AccessoryMaterialUpdateDto,
  ImageUpdateDto,
  PriceUpdateDto,
  GroupingCategoryEnum,
  GroupingCategoryListDto,
  ApiGroupingcategoriesFlatListClient,
  ApiCurrenciesClient,
  CurrencyDto
} from '../../../shared/clients';
import { ISelectItemModel } from '../../../shared/models/select-items.model';

@Injectable({
  providedIn: 'root'
})
export class AccessoryService {

  constructor(
    private client: ApiAccessoriesClient,
    private groupingClient: ApiGroupingcategoriesFlatListClient,
    private currencyClient: ApiCurrenciesClient,
  ) { }

  postAccessory(accessory: IAccessoryModel): Observable<string> {
    const dto = new AccessoryMaterialCreateDto({
      code: accessory.code,
      categoryId: accessory.categoryId,
      description: accessory.description,
      isOptional: accessory.structurallyOptional,
      isRequiredForAssembly: accessory.optMount,
      transactionMultiplier: accessory.transactionPrice,
      imageCreateDto: new ImageCreateDto({
        containerName: accessory.picture.containerName,
        fileName: accessory.picture.fileName
      }),
      price: new PriceCreateDto({
        currencyId: accessory.currencyId,
        value: accessory.purchasingPrice
      })
    });
    return this.client.createAccessoryMaterial(dto);
  }

  getAccessory(id: string) {
    return this.client.getAccessoryMaterialById(id).pipe(
      map((dto: AccessoryMaterialDetailsDto): IAccessoryModel => {
        const retObj: IAccessoryModel = {
          id: dto.id,
          code: dto.code,
          description: dto.description,
          categoryId: dto.categoryId,
          structurallyOptional: dto.isOptional,
          optMount: dto.isRequiredForAssembly,
          purchasingPrice: dto.price.value,
          currencyId: dto.price.currencyId,
          transactionPrice: dto.transactionMultiplier,
          picture: {
            fileName: dto.image.fileName,
            containerName: dto.image.containerName
          }
        };
        return retObj;
      }));
  }

  getAccessoryList(filter: IAccessoryFilterModel): Observable<PagedData<IAccessoryListViewModel>> {
    return this.client.getAccessoryMaterials(
      filter.code,
      filter.description,
      filter.categoryId,
      filter.structurallyOptional,
      filter.optMount,
      null,
      filter.pager.pageIndex,
      filter.pager.pageSize
    ).pipe(
      map((dto: PagedListDtoOfAccessoryMaterialListDto): PagedData<IAccessoryListViewModel> => ({
        items: dto.data.map(x => ({
          id: x.id,
          code: x.code,
          description: x.description,
          category: x.category.name,
          categoryId: x.category.id,
          structurallyOptional: x.isOptional,
          optMount: x.isRequiredForAssembly,
          purchasingPrice: x.price.value,
          currencyId: x.price.currencyId,
          currency: x.price.currency,
          transactionPrice: x.transactionMultiplier,
          picture: {
            fileName: x.image.fileName,
            containerName: x.image.containerName
          },
        })),
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        totalCount: dto.totalCount
      }))
    );
  }

  putAccessory(id: string, accessory: IAccessoryListViewModel): Observable<void> {
    const dto = new AccessoryMaterialUpdateDto({
      categoryId: accessory.categoryId,
      description: accessory.description,
      transactionMultiplier: accessory.transactionPrice,
      imageUpdateDto: new ImageUpdateDto({
        containerName: accessory.picture.containerName,
        fileName: accessory.picture.fileName
      }),
      priceUpdateDto: new PriceUpdateDto({
        currencyId: accessory.currencyId,
        value: accessory.purchasingPrice
      })
    });
    return this.client.putAccessoryMaterial(id, dto);
  }

  deleteAccessory(id: string): Observable<void> {
    return this.client.deleteAccessoryMaterial(id);
  }

  getCategories(): Observable<ISelectItemModel[]> {
    return this.groupingClient.getRootCategories(GroupingCategoryEnum.Accessories, undefined).pipe(
      map((dto: GroupingCategoryListDto[]): ISelectItemModel[] => {
        const retObj: ISelectItemModel[] = [];
        dto.forEach(x => {
          const temp: ISelectItemModel = {
            value: x.id,
            options: x.name
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
