import { Injectable } from '@angular/core';
import {
  ApiMaterialsDropdownClient,
  ApiStatisticsStocksClient,
  MaterialListForDropdownDto,
  StockStatisticsDetailsDto,
  StockStatisticsQuantityDto,
  ApiGroupingcategoriesFlatListClient,
  GroupingCategoryEnum,
  GroupingCategoryListDto,
  ApiStatisticsOldestClient,
  ApiGroupingcategoriesHierarchicalListClient
} from '../../../shared/clients';
import { IMaterialsDropdown, IGroupingMaterialsDropdown } from '../models/material.model';
import { IStockStatistics, IStatisticsData } from '../models/statistics.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { format, parse } from 'date-fns';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  constructor(
    private materialDropdownClient: ApiMaterialsDropdownClient,
    private groupingCategoryDropdownClient: ApiGroupingcategoriesHierarchicalListClient,
    private statisticsClient: ApiStatisticsStocksClient,
    private statisticsOldestClient: ApiStatisticsOldestClient
  ) { }


  getMaterialGroup(): Observable<IGroupingMaterialsDropdown[]> {
    return this.groupingCategoryDropdownClient.getHierarchicalCategories(GroupingCategoryEnum.MaterialType, true).pipe(
      map((dto: GroupingCategoryListDto[]): IGroupingMaterialsDropdown[] => {
        return dto[0].children.map((x: GroupingCategoryListDto): IGroupingMaterialsDropdown => ({
          id: x.id,
          name: x.name
        }));
      })
    );
  }

  getMaterials(id: number): Observable<IMaterialsDropdown[]> {
    return this.materialDropdownClient.getMaterialsForDropdownByCategory(id).pipe(
      map((dto: MaterialListForDropdownDto[]): IMaterialsDropdown[] => {
        return dto.map((x: MaterialListForDropdownDto): IMaterialsDropdown => ({
          id: x.id,
          name: x.materialCode
        }));
      })
    );
  }

  getStatistics(materialId: string, year: number): Observable<IStockStatistics> {
    return this.statisticsClient.getStockStatistics(materialId, new Date(Date.UTC(year, 0, 1)), new Date(Date.UTC(year, 11, 31))).pipe(
      map((dto: StockStatisticsDetailsDto): IStockStatistics => {
        let retObj: IStockStatistics;
        if (!dto) {
          retObj = {
            materialCode: '',
            data: []
          };
        } else {
          retObj = {
            materialCode: dto.materialCode,
            data: dto.quantities.map((x: StockStatisticsQuantityDto): IStatisticsData => ({
              weekNumber: x.weekNumber,
              quantity: x.quantity
            }))
          };
        }
        return retObj;
      })
    );
  }

  getOldestStatisticsYear(): Observable<number> {
    return this.statisticsOldestClient.getOldestYear().pipe(map(x => x.oldestYear));
  }
}
