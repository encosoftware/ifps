import { Injectable } from '@angular/core';
import { IFoilsListViewModel, IFoilsModel, IFoilsFilterModel } from '../models/foils.model';
import { Observable, of } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { map, delay } from 'rxjs/operators';
import {
  ApiFoilsClient,
  PagedListDtoOfFoilMaterialListDto,
  FoilMaterialCreateDto,
  ImageCreateDto,
  PriceCreateDto,
  FoilMaterialDetailsDto,
  FoilMaterialUpdateDto,
  ImageUpdateDto,
  PriceUpdateDto,
  ApiCurrenciesClient,
  CurrencyDto
} from '../../../shared/clients';
import { ISelectItemModel } from '../../../shared/models/select-items.model';

@Injectable({
  providedIn: 'root'
})
export class FoilService {

  constructor(
    private client: ApiFoilsClient,
    private currencyClient: ApiCurrenciesClient,
  ) { }

  postFoil(foil: IFoilsModel): Observable<string> {
    const dto = new FoilMaterialCreateDto({
      code: foil.code,
      description: foil.description,
      imageCreateDto: new ImageCreateDto({
        containerName: foil.picture.containerName,
        fileName: foil.picture.fileName
      }),
      price: new PriceCreateDto({
        currencyId: foil.currencyId,
        value: foil.purchasingPrice
      }),
      thickness: foil.thickness,
      transactionMultiplier: foil.transactionPrice
    });
    return this.client.createFoilMaterial(dto);
  }

  getFoil(id: string): Observable<IFoilsModel> {
    return this.client.getFoilMaterialById(id).pipe(
      map((dto: FoilMaterialDetailsDto): IFoilsModel => ({
        id: dto.id,
        code: dto.code,
        currencyId: dto.price.currencyId,
        description: dto.description,
        purchasingPrice: dto.price.value,
        thickness: dto.thickness,
        transactionPrice: dto.transactionMultiplier,
        picture: {
          containerName: dto.image.containerName,
          fileName: dto.image.fileName
        }
      }))
    );
  }

  getFoilList(filter: IFoilsFilterModel): Observable<PagedData<IFoilsListViewModel>> {
    return this.client.getAccessoryMaterials(filter.code, filter.description, null, filter.pager.pageIndex, filter.pager.pageSize).pipe(
      map((dto: PagedListDtoOfFoilMaterialListDto): PagedData<IFoilsListViewModel> => ({
        items: dto.data.map(x => ({
          id: x.id,
          code: x.code,
          description: x.description,
          thickness: x.thickness,
          purchasingPrice: x.price.value,
          currencyId: x.price.currencyId,
          currency: x.price.currency,
          transactionPrice: x.transactionMultiplier,
          picture: {
            containerName: x.image.containerName,
            fileName: x.image.fileName
          }
        })),
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        totalCount: dto.totalCount
      }))
    );
  }

  putFoil(id: string, foil: IFoilsModel): Observable<void> {
    const dto = new FoilMaterialUpdateDto({
      description: foil.description,
      imageUpdateDto: new ImageUpdateDto({
        containerName: foil.picture.containerName,
        fileName: foil.picture.fileName
      }),
      priceUpdateDto: new PriceUpdateDto({
        currencyId: foil.currencyId,
        value: foil.purchasingPrice
      }),
      thickness: foil.thickness,
      transactionMultiplier: foil.transactionPrice
    });
    return this.client.putFoilMaterial(id, dto);
  }

  deleteFoil(id: string): Observable<void> {
    return this.client.deleteFoilMaterial(id);
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
