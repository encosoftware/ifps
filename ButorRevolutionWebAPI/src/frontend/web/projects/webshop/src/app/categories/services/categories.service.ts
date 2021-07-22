import { Injectable } from '@angular/core';
import { ApiGroupingcategoriesWebshopSubcategoriesClient } from '../../shared/clients';
import { Observable } from 'rxjs/internal/Observable';
import { IGroupingSubCategoryWebshopViewModel } from '../../home/models/home';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {

  constructor(private webshopSubcategoriesClient: ApiGroupingcategoriesWebshopSubcategoriesClient) { }

  getSubcategeroiesByCategory(categoryId: number): Observable<IGroupingSubCategoryWebshopViewModel[] | null> {
    return this.webshopSubcategoriesClient.getSubcategeroiesByCategory(categoryId).pipe(
      map(resp => resp.map(subCat =>
        ({
          id: subCat.id,
          name: subCat.name,
          image: subCat.image ? subCat.image : undefined
        })
      )
      )
    );
  }
}
