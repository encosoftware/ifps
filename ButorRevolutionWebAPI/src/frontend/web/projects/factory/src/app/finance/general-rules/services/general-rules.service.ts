import { Injectable } from '@angular/core';
import {
    ApiGeneralexpensesGeneralrulesClient,
    PagedListDtoOfGeneralExpenseRuleListDto,
    ApiGeneralexpensesRulesClient,
    ApiCurrenciesClient,
    CurrencyDto,
    GeneralExpenseRuleCreateDto,
    PriceCreateDto,
    GeneralExpenseRuleUpdateDto,
    PriceUpdateDto,
    FrequencyTypeListDto,
    ApiFrequencytypesClient
} from '../../../shared/clients';
import { Observable } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { IGeneralRulesListViewModel, IGeneralRulesListFilterViewModel, INewGeneralRuleViewModel } from '../models/general-rules.model';
import { map } from 'rxjs/operators';
import { ICurrencyListViewModel, IFrequencyListViewModel } from '../../general-expenses/models/general-expenses.model';

@Injectable({
    providedIn: 'root'
})
export class GeneralRulesService {

    constructor(
        private generalRulesClient: ApiGeneralexpensesGeneralrulesClient,
        private generalExpensesRulesClient: ApiGeneralexpensesRulesClient,
        private currencyClient: ApiCurrenciesClient,
        private frequencyClient: ApiFrequencytypesClient
    ) { }

    getGeneralRules(filter: IGeneralRulesListFilterViewModel): Observable<PagedData<IGeneralRulesListViewModel>> {
        const tempFrom = filter.startDate ? new Date(filter.startDate) : undefined;
        const tempTo = filter.startDate ? new Date() : undefined;
        let tmp1 = filter.frequencyFrom;
        let tmp2 = filter.frequencyTo;
        let tmp3 = filter.frequencyTypeId;
        if (!filter.frequencyFrom || !filter.frequencyTo || !filter.frequencyTypeId) {
            tmp1 = undefined;
            tmp2 = undefined;
            tmp3 = undefined;
        }
        return this.generalRulesClient.listGeneralExpenseRules(filter.name, filter.amount, tmp1,
            tmp2, tmp3, tempFrom, tempTo, undefined,
            filter.pager.pageIndex, filter.pager.pageSize).pipe(
                map((dto: PagedListDtoOfGeneralExpenseRuleListDto): PagedData<IGeneralRulesListViewModel> => ({
                    items: dto.data.map(x => ({
                        id: x.id,
                        name: x.name,
                        interval: x.interval,
                        frequency: x.frequency,
                        amountValue: x.amount.value,
                        amountCurrency: x.amount.currency,
                        startDate: x.startDate
                    })),
                    pageIndex: filter.pager.pageIndex,
                    pageSize: filter.pager.pageSize,
                    totalCount: dto.totalCount
                }))
            );
    }

    deleteGeneralRule(id: number): Observable<void> {
        return this.generalExpensesRulesClient.deleteGeneralExpenseRule(id);
    }

    getCurrencies(): Observable<ICurrencyListViewModel[]> {
        return this.currencyClient.getCurrencies().pipe(map(res => res.map(this.currenciesToViewModel)));
    }

    private currenciesToViewModel(dto: CurrencyDto): ICurrencyListViewModel {
        return { name: dto.name, id: dto.id };
    }

    getFrequencies(): Observable<IFrequencyListViewModel[]> {
        return this.frequencyClient.getFrequencyTypes().pipe(map(res => res.map(this.frequenciesToViewModel)));
    }

    private frequenciesToViewModel(dto: FrequencyTypeListDto): IFrequencyListViewModel {
        return { translation: dto.translation, id: dto.id };
    }

    postGeneralRule(model: INewGeneralRuleViewModel): Observable<number> {
        let dto = new GeneralExpenseRuleCreateDto({
            amount: new PriceCreateDto({
                currencyId: model.amountCurrencyId,
                value: model.amountValue
            }),
            frequencyTypeId: model.frequencyTypeId,
            interval: model.frequency,
            name: model.name,
            startDate: new Date(Date.UTC(model.startDate.getFullYear(), model.startDate.getMonth(), model.startDate.getDate())),
            isFixed: model.isFixed
        });
        return this.generalExpensesRulesClient.createGeneralExpenseRule(dto);
    }

    putGeneralRule(id: number, model: INewGeneralRuleViewModel): Observable<void> {
        let dto = new GeneralExpenseRuleUpdateDto({
            amount: new PriceUpdateDto({
                currencyId: model.amountCurrencyId,
                value: model.amountValue
            }),
            frequencyTypeId: model.frequencyTypeId,
            interval: model.frequency,
            name: model.name,
            startDate: new Date(Date.UTC(model.startDate.getFullYear(), model.startDate.getMonth(), model.startDate.getDate())),
            isFixed: model.isFixed
        });
        return this.generalExpensesRulesClient.updateGeneralExpenseRule(id, dto);
    }

    getGeneralRule(id: number): Observable<INewGeneralRuleViewModel> {
        return this.generalExpensesRulesClient.getGeneralExpenseRule(id).pipe(map(res => ({
            startDate: res.startDate,
            name: res.name,
            frequency: res.interval,
            frequencyTypeId: res.frequencyTypeId,
            amountValue: res.amount.value,
            amountCurrencyId: res.amount.currencyId,
            isFixed: res.isFixed
        })));
    }

}
