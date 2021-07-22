import { Injectable } from '@angular/core';
import {  ApiGroupingcategoriesWebshopClient, ApiWebshopfurnitureunitsWebshopSearchClient } from '../../shared/clients';
import { Observable } from 'rxjs/internal/Observable';
import { PagedData } from '../../shared/models/paged-data.model';
import { map } from 'rxjs/internal/operators/map';
import {  IFurnitureUnitListByWebshopCategoryViewModel } from '../../units/models/units';
import { IGroupingCategoryWebshopViewModel } from '../../home/models/home';

@Injectable({
  providedIn: 'root'
})
export class HeaderService {

  constructor(
    private search: ApiWebshopfurnitureunitsWebshopSearchClient,
    private webShopClient: ApiGroupingcategoriesWebshopClient
    ) { }

  searchFurnitureUnitInWebshop(filter: string): Observable<PagedData<IFurnitureUnitListByWebshopCategoryViewModel>> {
    return this.search.searchWebshopFurnitureUnitInWebshop(
      filter,
      undefined,
      undefined,
      undefined,
      undefined,
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
  getCategoriesByWebshop(): Observable<IGroupingCategoryWebshopViewModel[] | null> {
    return this.webShopClient.getCategoriesByWebshop().pipe(
      map(resp => resp.map(webMenu => ({
        id: webMenu.id,
        name: webMenu.name,
        image: webMenu.image ? webMenu.image : undefined,
        subCategories: webMenu.subCategories ? webMenu.subCategories.map(subCat =>
          ({
            id: subCat.id,
            name: subCat.name,
            image: subCat.image ? subCat.image : undefined
          })
        ) : undefined
      })
      )
      )
    )
  }
}
