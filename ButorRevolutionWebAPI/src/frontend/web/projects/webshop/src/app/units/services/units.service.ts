import { Injectable } from '@angular/core';
import { OrderingDto, ApiWebshopfurnitureunitsSubcategoryClient, WebshopMaximumpriceClient, WebshopFurnitureUnitCategoryIdsDto } from '../../shared/clients';
import { Observable } from 'rxjs/internal/Observable';
import { IFurnitureUnitListByWebshopCategoryViewModel, IFurnitureUnitListByWebshopCategoryFilterViewModel } from '../models/units';
import { PagedData } from '../../shared/models/paged-data.model';
import { map } from 'rxjs/operators';
import { PriceListModel } from '../../shared/models/shared';


@Injectable({
  providedIn: 'root'
})
export class UnitsService {

  constructor(
    private webshopFurnitureunitsClient: ApiWebshopfurnitureunitsSubcategoryClient,
    private webshopClient: WebshopMaximumpriceClient,
  ) { }

  getFurnitureUnitsByWebshopCategory(
    filter: IFurnitureUnitListByWebshopCategoryFilterViewModel): Observable<PagedData<IFurnitureUnitListByWebshopCategoryViewModel>> {
    const orderings = filter.Orderings ? filter.Orderings.map(resp => new OrderingDto({
      column: resp.column,
      isDescending: resp.isDescending
    })) : undefined;
    return this.webshopFurnitureunitsClient.getWebshopFurnitureUnitsByWebshopCategory(
      filter.subcategoryId,
      filter.unitType,
      filter.minimumPrice,
      filter.maximumPrice,
      orderings,
      filter.pageIndex,
      filter.pageSize
    ).pipe(
      map(resp => ({
        pageIndex: resp.pageIndex,
        pageSize: resp.pageSize,
        totalCount: resp.totalCount,
        items: resp.data.map(cat => ({
          furnitureUnitId: cat.webshopFurnitureUnitId,
          categoryId: cat.categoryId,
          code: cat.code,
          description: cat.description,
          width: cat.width,
          height: cat.height,
          depth: cat.depth,
          image: cat.image ? cat.image : undefined,
          price: cat.price ? cat.price : undefined,
        }))
      })
      )
    )
  }

  getMaximumPriceFromWFUList(categoryId: number[]): Observable<PriceListModel | null> {
    const id = new WebshopFurnitureUnitCategoryIdsDto({
      categoryIds: [...categoryId],
    });
    return this.webshopClient.getMaximumPriceFromWFUList(id).pipe(
      map(res => ({
        value: res.value,
        currencyId: res.currencyId,
        currency: res.currency
      }))
    )
  }
}
