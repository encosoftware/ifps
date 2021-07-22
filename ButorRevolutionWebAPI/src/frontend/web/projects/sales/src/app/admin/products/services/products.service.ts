import { Injectable, Optional, Inject } from '@angular/core';
import {
  IProductsFilterViewModel,
  IProductsListViewModel,
  IProductCreateModel,
  IProductDetailsModel,
  IFoilListModel,
  IPictureModel,
  IFurnitureUnitCategoryModel
} from '../models/products.models';
import { Observable } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { map } from 'rxjs/operators';
import {
  ApiFurniturecomponentsClient,
  ApiFurnitureunitsClient,
  PagedListDtoOfFurnitureUnitListDto,
  FurnitureUnitCreateDto,
  ImageCreateDto,
  FurnitureUnitDetailsDto,
  FurnitureUnitUpdateDto,
  ApiFoilsClient,
  PagedListDtoOfFoilMaterialListDto,
  FurnitureComponentCreateDto,
  FurnitureComponentDetailsDto,
  FurnitureComponentUpdateDto,
  ImageUpdateDto,
  API_BASE_URL,
  ApiAccessoriesCodeClient,
  AccessoryMaterialCodesDto,
  ApiGroupingcategoriesFlatListClient,
  GroupingCategoryEnum,
  GroupingCategoryListDto,
  ApiAccessoryfurnitureunitsClient,
  AccessoryFurnitureUnitCreateDto,
  AccessoryFurnitureUnitDetailsDto,
  AccessoryFurnitureUnitUpdateDto,
  ApiDecorboardsDropdownClient,
  DecorBoardMaterialWithImageDto,
  FurnitureUnitListDto,
  ApiFurnitureunittypesClient,
  FurnitureUnitTypeListDto
} from '../../../shared/clients';
import { IProductsFrontViewModel, IProductsFrontDetailsViewModel, IMaterialCodeViewModel } from '../models/front.model';
import { IProductsCorpusViewModel, IProductsCorpusDetailsViewModel } from '../models/corpus.model';
import { IProductsAccessoriesViewModel } from '../models/accessory.model';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {

  constructor(
    private unitClinet: ApiFurnitureunitsClient,
    private componentClient: ApiFurniturecomponentsClient,
    private foilClient: ApiFoilsClient,
    private accessoryClinet: ApiAccessoryfurnitureunitsClient,
    private accessoryCodeClient: ApiAccessoriesCodeClient,
    private groupingCategoryClient: ApiGroupingcategoriesFlatListClient,
    private decorboardsDropdownClient: ApiDecorboardsDropdownClient,
    private unitTypeClient: ApiFurnitureunittypesClient,
    @Optional() @Inject(API_BASE_URL) private baseUrl?: string
  ) { }

  getProductsList = (filter: IProductsFilterViewModel): Observable<PagedData<IProductsListViewModel>> =>
    this.unitClinet.getFurnitureUnits(
      filter.code,
      filter.description,
      filter.categoryId,
      null,
      filter.pager.pageIndex,
      filter.pager.pageSize
    ).pipe(
      map((dto: PagedListDtoOfFurnitureUnitListDto): PagedData<IProductsListViewModel> => ({
        items: dto.data.map((x: FurnitureUnitListDto) => ({
          id: x.id,
          src: this.baseUrl + '/api/images?containerName=' + x.imageThumbnail.containerName + '&fileName=' + x.imageThumbnail.fileName,
          description: x.description,
          categoryId: x.category.id,
          category: x.category.name,
          code: x.code,
          size: {
            width: x.width,
            depth: x.depth,
            height: x.height
          },
          materialCost: x.materialCost.value,
          materialCurrency: x.materialCost.currency,
          sellPrice: x.sellPrice.value,
          sellPriceCurrency: x.sellPrice.currency
        })),
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        totalCount: dto.totalCount
      }))
    )

  postProducts(product: IProductCreateModel): Observable<string> {
    const dto = new FurnitureUnitCreateDto({
      categoryId: product.categoryId,
      code: product.code,
      depth: product.size.depth,
      height: product.size.height,
      width: product.size.width,
      description: product.description,
      furnitureUnitTypeId: product.furnitureUnitTypeId,
      imageCreateDto: new ImageCreateDto({
        containerName: product.picture.containerName,
        fileName: product.picture.fileName
      })
    });
    return this.unitClinet.postFurnitureUnit(dto);
  }

  putProducts(id: string, product: IProductDetailsModel, picture: IPictureModel): Observable<void> {
    const dto = new FurnitureUnitUpdateDto({
      code: product.code,
      categoryId: product.categoryId,
      depth: product.size.depth,
      height: product.size.height,
      width: product.size.width,
      description: product.description,
      imageUpdateDto: new ImageCreateDto({
        containerName: picture.containerName,
        fileName: picture.fileName
      })
    });
    return this.unitClinet.updateFurnitureUnit(id, dto);
  }

  getProductsEdit(id: string): Observable<IProductDetailsModel> {
    return this.unitClinet.getFurnitureUnitById(id).pipe(
      map((dto: FurnitureUnitDetailsDto): IProductDetailsModel => ({
        code: dto.code,
        categoryId: dto.categoryId,
        size: {
          depth: dto.depth,
          height: dto.height,
          width: dto.width
        },
        description: dto.description,
        picture: {
          containerName: dto.imageDetailsDto.containerName,
          fileName: dto.imageDetailsDto.fileName
        },
        accessories: dto.accessoryFurnitureComponents.map((a): IProductsAccessoriesViewModel => ({
          amount: a.amount,
          id: a.id,
          name: a.name,
          materialCode: a.code,
          picture: {
            containerName: a.imageThumbnailListDto.containerName,
            fileName: a.imageThumbnailListDto.fileName
          },
          src:
            this.baseUrl
            + '/api/images?containerName='
            + a.imageThumbnailListDto.containerName
            + '&fileName='
            + a.imageThumbnailListDto.fileName
        })),
        corpus: dto.corpusFurnitureComponents.map((c): IProductsCorpusViewModel => ({
          id: c.id,
          amount: c.amount,
          name: c.name,
          code: c.code,
          size: {
            height: c.height,
            width: c.width,
            depth: null
          },
          edging: {
            bottom: c.bottomFoilName,
            left: c.leftFoilName,
            right: c.rightFoilName,
            top: c.topFoilName
          },
          src: this.baseUrl
            + '/api/images?containerName='
            + c.imageThumbnailListDto.containerName
            + '&fileName='
            + c.imageThumbnailListDto.fileName,
        })),
        front: dto.frontFurnitureComponents.map((f): IProductsFrontViewModel => ({
          id: f.id,
          name: f.name,
          size: {
            height: f.height,
            width: f.width,
            depth: null
          },
          amount: f.amount,
          code: f.code,
          edging: {
            bottom: f.bottomFoilName,
            left: f.leftFoilName,
            right: f.rightFoilName,
            top: f.topFoilName
          },
          src: this.baseUrl
            + '/api/images?containerName='
            + f.imageThumbnailListDto.containerName
            + '&fileName='
            + f.imageThumbnailListDto.fileName,
        }))
      }))
    );
  }

  deleteProductUnit(id: string): Observable<void> {
    return this.unitClinet.deleteFurnitureUnit(id);
  }

  getFoils(): Observable<IFoilListModel[]> {
    return this.foilClient.getAccessoryMaterials('', '', null, undefined, undefined).pipe(
      map((dto: PagedListDtoOfFoilMaterialListDto): IFoilListModel[] =>
        dto.data.map(x => ({
          id: x.id,
          code: x.code
        }))
      )
    );
  }

  postFront(front: IProductsFrontDetailsViewModel): Observable<string> {
    const dto = new FurnitureComponentCreateDto({
      amount: front.amount,
      bottomFoilId: front.bottomId,
      furnitureUnitId: front.furnitureUnitId,
      height: front.size.height,
      leftFoilId: front.leftId,
      materialId: front.materialId,
      rightFoilId: front.rightId,
      name: front.name,
      width: front.size.width,
      topFoilId: front.topId,
      type: front.typeId
    });
    return this.componentClient.postFurnitureComponent(dto);
  }

  postCorpus(corpus: IProductsCorpusDetailsViewModel): Observable<string> {
    const dto = new FurnitureComponentCreateDto({
      amount: corpus.amount,
      bottomFoilId: corpus.bottomId,
      furnitureUnitId: corpus.furnitureUnitId,
      height: corpus.size.height,
      leftFoilId: corpus.leftId,
      materialId: corpus.materialId,
      rightFoilId: corpus.rightId,
      name: corpus.name,
      width: corpus.size.width,
      topFoilId: corpus.topId,
      type: corpus.typeId
    });
    return this.componentClient.postFurnitureComponent(dto);
  }

  getFurnitureComponent(id: string): Observable<IProductsFrontDetailsViewModel> {
    return this.componentClient.getFurnitureComponentById(id).pipe(
      map((dto: FurnitureComponentDetailsDto): IProductsFrontDetailsViewModel => ({
        amount: dto.amount,
        bottomId: dto.bottomFoilId,
        leftId: dto.leftFoilId,
        materialId: dto.materialId,
        name: dto.name,
        rightId: dto.rightFoilId,
        topId: dto.topFoilId,
        size: {
          height: dto.height,
          width: dto.width
        },
        picture: {
          containerName: dto.imageThumbnailDetailsDto.containerName,
          fileName: dto.imageThumbnailDetailsDto.fileName
        }
      }))
    );
  }

  putFurnitureComponent(id: string, front: IProductsFrontDetailsViewModel): Observable<void> {
    const dto = new FurnitureComponentUpdateDto({
      amount: front.amount,
      bottomFoilId: front.bottomId,
      height: front.size.height,
      leftFoilId: front.leftId,
      materialId: front.materialId,
      name: front.name,
      rightFoilId: front.rightId,
      topFoilId: front.topId,
      width: front.size.width,
      imageUpdateDto: new ImageUpdateDto({
        containerName: front.picture.containerName,
        fileName: front.picture.fileName
      })
    });
    return this.componentClient.updateFurnitureComponent(id, dto);
  }

  deleteFurnitureComponent(id: string): Observable<void> {
    return this.componentClient.deleteFurnitureComponent(id);
  }

  deleteAccessoryFurniture(id: number): Observable<void> {
    return this.accessoryClinet.deleteFurnitureUnit(id);
  }

  postAccessoryComponent(accessory: IProductsAccessoriesViewModel): Observable<number> {
    const dto = new AccessoryFurnitureUnitCreateDto({
      amount: accessory.amount,
      furnitureUnitId: accessory.furnitureUnitId,
      materialId: accessory.materialId,
      name: accessory.name,
    });
    return this.accessoryClinet.createAccessoryFurnitureUnit(dto);
  }

  getAccessoryComponent(id: number): Observable<IProductsAccessoriesViewModel> {
    return this.accessoryClinet.getAccessoryFurnitureUnitById(id).pipe(
      map((dto: AccessoryFurnitureUnitDetailsDto): IProductsAccessoriesViewModel => ({
        amount: dto.amount,
        materialId: dto.materialId,
        name: dto.name,
        picture: {
          containerName: dto.imageThumbnailDetailsDto.containerName,
          fileName: dto.imageThumbnailDetailsDto.fileName
        }
      }))
    );
  }

  putAccessoryComponent(id: number, accessory: IProductsAccessoriesViewModel): Observable<void> {
    const dto = new AccessoryFurnitureUnitUpdateDto({
      amount: accessory.amount,
      materialId: accessory.materialId,
      name: accessory.name
    });
    return this.accessoryClinet.updateAccessoryFurnitureUnit(id, dto);
  }

  getDecorMaterialCodes(): Observable<IMaterialCodeViewModel[]> {
    return this.decorboardsDropdownClient.getDecorBoardMaterialsForDropdown().pipe(
      map((dto: DecorBoardMaterialWithImageDto[]): IMaterialCodeViewModel[] => {
        return dto.map((materialCode: DecorBoardMaterialWithImageDto): IMaterialCodeViewModel => ({
          id: materialCode.id,
          code: materialCode.code,
          src: this.baseUrl + '/api/images?containerName=' + materialCode.image.containerName + '&fileName=' + materialCode.image.fileName,
          fileName: materialCode.image.fileName,
          containerName: materialCode.image.containerName,
          description: materialCode.description
        }));
      })
    );
  }

  getAccessoryCode(): Observable<IMaterialCodeViewModel[]> {
    return this.accessoryCodeClient.getAccessoryMaterialCodes().pipe(
      map((dto: AccessoryMaterialCodesDto[]): IMaterialCodeViewModel[] => {
        return dto.map((acc: AccessoryMaterialCodesDto): IMaterialCodeViewModel => ({
          id: acc.id,
          code: acc.code
        }));
      })
    );
  }

  getFurnitureUnitCategories(): Observable<IFurnitureUnitCategoryModel[]> {
    return this.groupingCategoryClient.getRootCategories(GroupingCategoryEnum.FurnitureUnitType, undefined).pipe(
      map((dto: GroupingCategoryListDto[]): IFurnitureUnitCategoryModel[] => {
        return dto.map((x: GroupingCategoryListDto): IFurnitureUnitCategoryModel => ({
          id: x.id,
          name: x.name
        }));
      })
    );
  }

  getFurnitureUnitIds(): Observable<IFurnitureUnitCategoryModel[]> {
    return this.unitTypeClient.getFurnitureUnitTypes().pipe(
      map((dto: FurnitureUnitTypeListDto[]): IFurnitureUnitCategoryModel[] => {
        return dto.map((x: FurnitureUnitTypeListDto): IFurnitureUnitCategoryModel => ({
          id: x.id,
          name: x.name
        }));
      })
    );
  }
}
