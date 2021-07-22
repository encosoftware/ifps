import { Injectable } from '@angular/core';
import {
  ApiStatisticsFinancesClient,
  FinanceStatisticsListDto,
  ApiStatisticsOldestClient
} from '../../../shared/clients';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { IStatisticsFilterModel, IStatisticsListModel } from '../models/statistics.model';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {

  constructor(
    private statisticsFinancesClient: ApiStatisticsFinancesClient,
    private statisticsOldestClient: ApiStatisticsOldestClient
  ) { }

  getFinanceStatistics(filter: IStatisticsFilterModel): Observable<IStatisticsListModel[]> {
    return this.statisticsFinancesClient.getFinanceStatistics(
      new Date(Date.UTC(filter.year, 0, 1)),
      new Date(Date.UTC(filter.year, 11, 31))
    ).pipe(
      map((dto: FinanceStatisticsListDto[]): IStatisticsListModel[] => {
        return dto.map((x: FinanceStatisticsListDto): IStatisticsListModel => ({
          month: x.month,
          generalExpenseCost: x.generalExpenseCost,
          recurringCost: x.recurringCost,
          income: x.income,
          profit: x.income - x.recurringCost - x.generalExpenseCost,
          currency: x.currency
        }));
      })
    );
  }

  getOldestStatisticsYear(): Observable<number> {
    return this.statisticsOldestClient.getOldestYear().pipe(map(x => x.oldestYear));
  }
}
