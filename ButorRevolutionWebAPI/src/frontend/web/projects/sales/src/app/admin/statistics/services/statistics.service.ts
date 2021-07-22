import { Injectable } from '@angular/core';
import { ApiStatisticsClient, SalesPersonStatisticsDto } from '../../../shared/clients';
import { SalesPersonStatisticsModel, IStatisticsFilterViewModel } from '../models/statistics';
import { Observable, of } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { map } from 'rxjs/operators';
import { isAfter } from 'date-fns';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {

  constructor(private statisticsClient: ApiStatisticsClient) { }

  getSalesPersonsList(filter: IStatisticsFilterViewModel): Observable<SalesPersonStatisticsModel> {
    const from = filter.from ? new Date(Date.UTC(filter.from.getFullYear(), filter.from.getMonth(), filter.from.getDate())) : undefined;
    const to = filter.to ? new Date(Date.UTC(filter.to.getFullYear(), filter.to.getMonth(), filter.to.getDate())) : undefined;
    const isAfterDate = (filter.from && filter.to) ? isAfter(to,from) : false;
    return this.statisticsClient.getSalesPersonsList(
      filter.name,
      (isAfterDate || filter.from) ? filter.from : undefined,
      (isAfterDate && filter.to) ? filter.to : undefined,
      undefined,
      undefined,
      undefined
    ).pipe(
      map((dto: SalesPersonStatisticsDto) => ({
        summary: dto.summary ? ({
          numberOfOffers: dto.summary.numberOfOffers,
          numberOfContracts: dto.summary.numberOfContracts,
          effeciency: dto.summary.effeciency,
          summaryTotal: dto.summary.summaryTotal ? ({
            value: dto.summary.summaryTotal.value,
            currencyId: dto.summary.summaryTotal.currencyId,
            currency: dto.summary.summaryTotal.currency,
          }) : null,
        }) : null,
        salesPersons: dto.salesPersons ? dto.salesPersons.map(sales => ({
          name: sales.name,
          numberOfOffers: sales.numberOfOffers,
          numberOfContracts: sales.numberOfContracts,
          efficiency: sales.efficiency,
          total: sales.total ? ({
            value: sales.total.value,
            currencyId: sales.total.currencyId,
            currency: sales.total.currency,
          }) : null,
        })) : null
      }))
    )
  }
}
