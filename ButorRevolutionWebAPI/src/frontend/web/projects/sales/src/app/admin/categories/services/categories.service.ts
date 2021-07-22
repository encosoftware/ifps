import { Injectable } from '@angular/core';
import { EditCategory, IViewCategory } from '../models/categories.models';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  ApiGroupingcategoriesClient,
  GroupingCategoryListDto,
  ApiGroupingcategoriesHierarchicalListClient,
  LanguageTypeEnum,
  GroupingCategoryEnum,
  GroupingCategoryDetailsDto,
  GroupingCategoryUpdateDto,
  GroupingCategoryCreateDto,
  GroupingCategoryTranslationUpdateDto,
  ApiGroupingcategoriesFlatListClient,
  GroupingCategoryTranslationCreateDto
} from '../../../shared/clients';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {

  constructor(
    private categoriesListClient: ApiGroupingcategoriesFlatListClient,
    private categoriesClient: ApiGroupingcategoriesClient,
    private categoriesHierarchical: ApiGroupingcategoriesHierarchicalListClient
  ) { }


  getRootCategories(
    categoryType?: GroupingCategoryEnum,
    language?: LanguageTypeEnum,
    loadOnlyRootObjects: boolean = true): Observable<IViewCategory[] | null> {
    return this.categoriesListClient.getRootCategories(categoryType, loadOnlyRootObjects).pipe(
      map((dto: GroupingCategoryListDto[]) => {
        return dto.map((resp: GroupingCategoryListDto): IViewCategory => ({
          id: resp.id,
          parentId: resp.parentId,
          name: resp.name,
          groupingCategoryType: resp.groupingCategoryType,
          children: this.getChildren(resp.children)
        }));
      })
    );
  }

  getChildren(exp: GroupingCategoryListDto[]): IViewCategory[] {
    if (exp) {
      return exp.map((resp: GroupingCategoryListDto): IViewCategory => ({
        id: resp.id,
        parentId: resp.parentId,
        name: resp.name,
        groupingCategoryType: resp.groupingCategoryType,
        children: this.getChildren(resp.children)
      }));
    }
    return null;
  }

  getHierarchicalCategories(categoryType: GroupingCategoryEnum, language?: LanguageTypeEnum): Observable<IViewCategory[] | null> {
    return this.categoriesHierarchical.getHierarchicalCategories(categoryType, undefined).pipe(
      map((dto: GroupingCategoryListDto[]) => {
        return dto.map((resp: GroupingCategoryListDto): IViewCategory => ({
          id: resp.id,
          parentId: resp.parentId,
          name: resp.name,
          groupingCategoryType: resp.groupingCategoryType,
          children: this.getChildren(resp.children),
        }));
      })
    );
  }
  getById(id: number): Observable<EditCategory | null> {
    return this.categoriesClient.getById(id).pipe(
      map((resp: GroupingCategoryDetailsDto) => ({
        id: resp.id,
        translation: resp.translations,
        parentId: resp.parentId,
        type: resp.groupingCategoryType,
      })
      )
    );
  }
  updateGroupingCategory(id: number, updateDto: EditCategory | null): Observable<void> {
    const dto = new GroupingCategoryUpdateDto({
      translations: updateDto.translation.map((resp) => new GroupingCategoryTranslationUpdateDto({
        id: resp.id,
        name: resp.name,
        language: resp.language,
      })),
      parentGroupId: updateDto.parentId
    });
    return this.categoriesClient.updateGroupingCategory(id, dto);
  }

  deleteGroupingCategory(id: number): Observable<void> {
    return this.categoriesClient.deleteGroupingCategory(id);
  }

  post(createDto: EditCategory | null): Observable<number> {
    const dto = new GroupingCategoryCreateDto({
      translations: createDto.translation.map((resp) => new GroupingCategoryTranslationCreateDto({
        name: resp.name,
        language: resp.language,
      })),
      parentId: createDto.parentId
    });
    return this.categoriesClient.post(dto);
  }
}
