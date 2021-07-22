import { Injectable } from '@angular/core';
import { IWorktopListViewModel, IWorktopModel, IWorktopFilterModel, IGroupingModel } from '../models/worktops.model';
import { Observable, of } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { map } from 'rxjs/operators';
import {
  ApiWorktopboardsClient,
  PagedListDtoOfWorktopBoardMaterialListDto,
  WorktopBoardMaterialCreateDto,
  DimensionCreateDto,
  PriceCreateDto,
  WorktopBoardMaterialUpdateDto,
  PriceUpdateDto,
  ApiGroupingcategoriesFlatListClient,
  GroupingCategoryEnum,
  GroupingCategoryListDto,
  ImageUpdateDto,
  ImageCreateDto,
  ApiCurrenciesClient,
  CurrencyDto
} from '../../../shared/clients';
import { ISelectItemModel } from '../../../shared/models/select-items.model';

@Injectable({
  providedIn: 'root'
})
export class WorkTopService {

  constructor(
    private client: ApiWorktopboardsClient,
    private groupingClient: ApiGroupingcategoriesFlatListClient,
    private currencyClient: ApiCurrenciesClient,
  ) { }

  postWorktop(worktop: IWorktopModel): Observable<string> {
    const dto = new WorktopBoardMaterialCreateDto({
      code: worktop.code,
      categoryId: worktop.categoryId,
      description: worktop.description,
      imageCreateDto: new ImageCreateDto({
        containerName: worktop.picture.containerName,
        fileName: worktop.picture.fileName
      }),
      dimension: new DimensionCreateDto({
        length: worktop.length,
        thickness: worktop.thickness,
        width: worktop.width,
      }),
      price: new PriceCreateDto({
        value: worktop.purchasingPrice,
        currencyId: worktop.currencyId
      }),
      transactionMultiplier: worktop.transactionPrice,
      hasFiberDirection: worktop.fiberHeading
    });
    return this.client.createWorktopBoardMaterial(dto);
  }

  getWorktop(id: string): Observable<IWorktopModel> {
    return this.client.getWorktopBoardMaterialById(id).pipe(
      map((dto): IWorktopModel => ({
        id: dto.id,
        categoryId: dto.categoryId,
        code: dto.code,
        currencyId: dto.price.currencyId,
        purchasingPrice: dto.price.value,
        description: dto.description,
        length: dto.dimension.length,
        thickness: dto.dimension.thickness,
        width: dto.dimension.width,
        transactionPrice: dto.transactionMultiplier,
        fiberHeading: dto.hasFiberDirection,
        picture: {
          containerName: dto.image.containerName,
          fileName: dto.image.fileName
        }
      }))
    );
  }

  getWorktopList(filter: IWorktopFilterModel): Observable<PagedData<IWorktopListViewModel>> {
    return this.client.getAccessoryMaterials(
      filter.code,
      filter.description,
      filter.categoryId,
      null,
      filter.pager.pageIndex,
      filter.pager.pageSize
    ).pipe(
      map((dto: PagedListDtoOfWorktopBoardMaterialListDto): PagedData<IWorktopListViewModel> => ({
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

  putWorktop(id: string, worktop: IWorktopModel): Observable<void> {
    const dto = new WorktopBoardMaterialUpdateDto({
      categoryId: worktop.categoryId,
      description: worktop.description,
      imageUpdateDto: new ImageUpdateDto({
        containerName: worktop.picture.containerName,
        fileName: worktop.picture.fileName
      }),
      dimension: new DimensionCreateDto({
        length: worktop.length,
        thickness: worktop.thickness,
        width: worktop.width,
      }),
      priceUpdateDto: new PriceUpdateDto({
        value: worktop.purchasingPrice,
        currencyId: worktop.currencyId
      }),
      transactionMultiplier: worktop.transactionPrice,
      hasFiberDirection: worktop.fiberHeading
    });
    return this.client.putWorktopBoardMaterial(id, dto);
  }

  deleteWorktop(id: string): Observable<void> {
    return this.client.deleteWorktopBoardMaterial(id);
  }

  getCategories(): Observable<IGroupingModel[]> {
    return this.groupingClient.getRootCategories(GroupingCategoryEnum.WorktopBoard, undefined).pipe(
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
