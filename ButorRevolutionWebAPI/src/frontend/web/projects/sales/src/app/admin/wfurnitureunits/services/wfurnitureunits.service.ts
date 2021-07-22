import { Injectable } from '@angular/core';
import {
  ApiWebshopfurnitureunitsClient,
  PagedListDtoOfWebshopFurnitureUnitListDto,
  WebshopFurnitureUnitListDto,
  WebshopFurnitureUnitDetailsDto,
  WebshopFurnitureUnitUpdateDto,
  FurnitureUnitsForDropdownDto,
  ApiFurnitureunitsBaseDropdownClient,
  ImageCreateDto,
  PriceCreateDto,
  ApiFurnitureunitsWfusClient,
  FurnitureUnitForWFUDto,
  ApiCurrenciesClient,
  CurrencyDto
} from '../../../shared/clients';
import { Observable } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { map } from 'rxjs/operators';
import {
  IWFUFilterModel,
  IWFUListModel,
  IWFUDetailsModel,
  IFurnitureUnitDropdownModel,
  FurnitureUnitForWebshopViewModel,
  ICurrenciesDropdownModel
} from '../models/wfurnitureunits.model';

@Injectable({
  providedIn: 'root'
})
export class WFUService {

  constructor(
    private wFurnitureUnitsClient: ApiWebshopfurnitureunitsClient,
    private furnitureUnitsClient: ApiFurnitureunitsBaseDropdownClient,
    private furnitureunitsWfusClient: ApiFurnitureunitsWfusClient,
    private currenciesClient: ApiCurrenciesClient,
  ) { }

  getFurnitureUnitForWFU(id: string): Observable<FurnitureUnitForWebshopViewModel | null> {
    return this.furnitureunitsWfusClient.getFurnitureUnitForWFU(id).pipe(
      map((dto: FurnitureUnitForWFUDto) => ({
        code: dto.code ? dto.code : undefined,
        description: dto.description ? dto.description : undefined,
        imageDetailsDto: dto.imageDetailsDto ? [({
          containerName: dto.imageDetailsDto.containerName ? dto.imageDetailsDto.containerName : undefined,
          fileName: dto.imageDetailsDto.fileName ? dto.imageDetailsDto.fileName : undefined,
        })] : undefined
      }))
    )
  }

  getWebshopFurnitureUnits(filter: IWFUFilterModel): Observable<PagedData<IWFUListModel>> {
    return this.wFurnitureUnitsClient.getWebshopFurnitureUnits(
      filter.code,
      filter.description,
      undefined,
      filter.pager.pageIndex,
      filter.pager.pageSize
    ).pipe(
      map((dto: PagedListDtoOfWebshopFurnitureUnitListDto): PagedData<IWFUListModel> => ({
        items: dto.data.map((x: WebshopFurnitureUnitListDto): IWFUListModel => ({
          id: x.id,
          furnitureUnitId: x.furnitureUnitId,
          code: x.code,
          description: x.description,
          price: x.price.value + ' ' + x.price.currency
        })),
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        totalCount: dto.totalCount
      }))
    );
  }

  getWebshopFurnitureUnitById(id: number): Observable<IWFUDetailsModel> {
    return this.wFurnitureUnitsClient.getWebshopFurnitureUnitById(id).pipe(
      map((dto: WebshopFurnitureUnitDetailsDto): IWFUDetailsModel => ({
        furnitureUnitId: dto.furnitureUnitId,
        images: dto.images ? dto.images.map(res => ({
          containerName: res.containerName,
          fileName: res.fileName
        })
        ) : undefined,
        price: dto.price ? ({
          value: dto.price.value,
          currencyId: dto.price.currencyId,
          currency: dto.price.currency
        }) : undefined

      }))
    );
  }

  createWebshopFurnitureUnit(wFurnitureUnit: IWFUDetailsModel): Observable<number> {
    const dto = new WebshopFurnitureUnitUpdateDto({
      furnitureUnitId: wFurnitureUnit.furnitureUnitId,
      images: wFurnitureUnit.images ? wFurnitureUnit.images.map(res => new ImageCreateDto({
        containerName: res.containerName,
        fileName: res.fileName
      })
      ) : undefined,
      price: wFurnitureUnit.price ? new PriceCreateDto({
        value: wFurnitureUnit.price.value,
        currencyId: wFurnitureUnit.price.currencyId,
      }) : undefined
    });
    return this.wFurnitureUnitsClient.createWebshopFurnitureUnit(dto);
  }

  updateWebshopFurnitureUnit(id: number, wFurnitureUnit: IWFUDetailsModel): Observable<void> {
    const dto = new WebshopFurnitureUnitUpdateDto({
      furnitureUnitId: wFurnitureUnit.furnitureUnitId,
      images: wFurnitureUnit.images ? wFurnitureUnit.images.map(res => new ImageCreateDto({
        containerName: res.containerName,
        fileName: res.fileName
      })
      ) : undefined,
      price: wFurnitureUnit.price ? new PriceCreateDto({
        value: wFurnitureUnit.price.value,
        currencyId: wFurnitureUnit.price.currencyId,
      }) : undefined
    });
    return this.wFurnitureUnitsClient.updateWebshopFurnitureUnit(id, dto);
  }

  deleteWFurnitureUnit(id: number): Observable<void> {
    return this.wFurnitureUnitsClient.deleteWebshopFurnitureUnit(id);
  }

  getBaseFurnitureUnits(): Observable<IFurnitureUnitDropdownModel[]> {
    return this.furnitureUnitsClient.getBaseFurnitureUnits().pipe(
      map((dto: FurnitureUnitsForDropdownDto[]): IFurnitureUnitDropdownModel[] => {
        return dto.map((x: FurnitureUnitsForDropdownDto): IFurnitureUnitDropdownModel => ({
          id: x.id,
          code: x.code
        }));
      })
    );
  }
  getCurrencies(): Observable<ICurrenciesDropdownModel[] | null> {
    return this.currenciesClient.getCurrencies().pipe(
      map(dto => dto.map(x => ({
        id: x.id,
        name: x.name
      })))
    )
  }
}
