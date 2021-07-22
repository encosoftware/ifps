import { Injectable } from '@angular/core';
import { IDecorboardListViewModel, IDecorboardModel, IDecorboardFilterModel, IGroupingModel } from '../models/decorboards.model';
import { Observable, of } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { map } from 'rxjs/operators';
import {
  ApiDecorboardsClient,
  PagedListDtoOfDecorBoardMaterialListDto,
  DecorBoardMaterialCreateDto,
  DimensionCreateDto,
  ImageCreateDto,
  PriceCreateDto,
  DecorBoardMaterialDetailsDto,
  DecorBoardMaterialUpdateDto,
  DimensionUpdateDto,
  ImageUpdateDto,
  PriceUpdateDto,
  ApiGroupingcategoriesFlatListClient,
  GroupingCategoryEnum,
  GroupingCategoryListDto,
  ApiCurrenciesClient,
  CurrencyDto
} from '../../../shared/clients';
import { ISelectItemModel } from '../../../shared/models/select-items.model';

@Injectable({
  providedIn: 'root'
})
export class DecorboardService {

  constructor(
    private client: ApiDecorboardsClient,
    private groupingClient: ApiGroupingcategoriesFlatListClient,
    private currencyClient: ApiCurrenciesClient,
  ) { }

  postDecorboard(decorboard: IDecorboardModel): Observable<string> {
    const dto = new DecorBoardMaterialCreateDto({
      code: decorboard.code,
      categoryId: decorboard.categoryId,
      description: decorboard.description,
      dimension: new DimensionCreateDto({
        length: decorboard.length,
        thickness: decorboard.thickness,
        width: decorboard.width
      }),
      hasFiberDirection: decorboard.fiberHeading,
      imageCreateDto: new ImageCreateDto({
        containerName: decorboard.picture.containerName,
        fileName: decorboard.picture.fileName
      }),
      price: new PriceCreateDto({
        currencyId: decorboard.currencyId,
        value: decorboard.purchasingPrice
      }),
      transactionMultiplier: decorboard.transactionPrice
    });
    return this.client.createDecorBoardMaterial(dto);
  }

  getDecorboard(id: string): Observable<IDecorboardModel> {
    return this.client.getDecorBoardMaterialById(id).pipe(
      map((dto: DecorBoardMaterialDetailsDto): IDecorboardModel => ({
        id: dto.id,
        categoryId: dto.categoryId,
        code: dto.code,
        fiberHeading: dto.hasFiberDirection,
        currencyId: dto.price.currencyId,
        description: dto.description,
        purchasingPrice: dto.price.value,
        length: dto.dimension.length,
        thickness: dto.dimension.thickness,
        width: dto.dimension.width,
        transactionPrice: dto.transactionMultiplier,
        picture: {
          containerName: dto.image.containerName,
          fileName: dto.image.fileName
        }
      }))
    );
  }

  getDecorboardList(filter: IDecorboardFilterModel): Observable<PagedData<IDecorboardListViewModel>> {
    return this.client.getAccessoryMaterials(
      filter.code,
      filter.description,
      filter.categoryId,
      null,
      filter.pager.pageIndex,
      filter.pager.pageSize
    ).pipe(
      map((dto: PagedListDtoOfDecorBoardMaterialListDto): PagedData<IDecorboardListViewModel> => ({
        items: dto.data.map(x => ({
          id: x.id,
          code: x.code,
          description: x.description,
          category: x.category.name,
          categoryId: x.category.id,
          length: x.dimension.length,
          width: x.dimension.width,
          thickness: x.dimension.thickness,
          purchasingPrice: x.price.value,
          currencyId: x.price.currencyId,
          currency: x.price.currency,
          transactionPrice: x.transactionMultiplier,
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

  putDecorboard(id: string, decorboard: IDecorboardModel): Observable<void> {
    const dto = new DecorBoardMaterialUpdateDto({
      categoryId: decorboard.categoryId,
      description: decorboard.description,
      dimension: new DimensionUpdateDto({
        length: decorboard.length,
        thickness: decorboard.thickness,
        width: decorboard.width
      }),
      hasFiberDirection: decorboard.fiberHeading,
      imageUpdateDto: new ImageUpdateDto({
        containerName: decorboard.picture.containerName,
        fileName: decorboard.picture.fileName
      }),
      priceUpdateDto: new PriceUpdateDto({
        currencyId: decorboard.currencyId,
        value: decorboard.purchasingPrice
      }),
      transactionMultiplier: decorboard.transactionPrice
    });
    return this.client.putDecorBoardMaterial(id, dto);
  }

  deleteDecorboard(id: string): Observable<void> {
    return this.client.deleteDecorBoardMaterial(id);
  }

  getCategories(): Observable<IGroupingModel[]> {
    return this.groupingClient.getRootCategories(GroupingCategoryEnum.DecorBoard, undefined).pipe(
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
