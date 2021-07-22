import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { map } from 'rxjs/operators';
import {
    ApiGeneralexpensesClient,
    PagedListDtoOfGeneralExpenseListDto,
    ApiCurrenciesClient,
    CurrencyDto,
    GeneralExpenseCreateDto,
    PriceCreateDto,
    GeneralExpenseUpdateDto,
    PriceUpdateDto,
    ApiGeneralexpensesRecurringClient,
    RecurrentExpenseListDto,
    RecurrentExpenseUpdateDto,
    ApiFrequencytypesClient,
    FrequencyTypeListDto
} from '../../../shared/clients';
import {
    IGeneralExpensesListFilterViewModel,
    IGeneralExpensesListViewModel,
    ICurrencyListViewModel,
    INewGeneralExpenseViewModel,
    IRecurringCostViewModel,
    IFrequencyListViewModel
} from '../models/general-expenses.model';
import { parse } from 'date-fns';

@Injectable({
    providedIn: 'root'
})
export class GeneralExpensesService {

    constructor(
        private generalExpensesClient: ApiGeneralexpensesClient,
        private currencyClient: ApiCurrenciesClient,
        private recurringClient: ApiGeneralexpensesRecurringClient,
        private frequencyClient: ApiFrequencytypesClient
    ) { }

    getGeneralExpenses(filter: IGeneralExpensesListFilterViewModel): Observable<PagedData<IGeneralExpensesListViewModel>> {
        const tempFrom = filter.paymentDate ? new Date(filter.paymentDate) : undefined;
        const tempTo = filter.paymentDate ? new Date() : undefined;
        return this.generalExpensesClient.listGeneralExpenses(filter.name, filter.amount, tempFrom, tempTo, undefined,
            filter.pager.pageIndex, filter.pager.pageSize).pipe(
                map((dto: PagedListDtoOfGeneralExpenseListDto): PagedData<IGeneralExpensesListViewModel> => ({
                    items: dto.data.map(x => ({
                        id: x.id,
                        name: x.name,
                        amountValue: x.amount.value,
                        amountCurrency: x.amount.currency,
                        paymentDate: x.paymentDate
                    })),
                    pageIndex: filter.pager.pageIndex,
                    pageSize: filter.pager.pageSize,
                    totalCount: dto.totalCount
                }))
            );
    }

    deleteGeneralExpense(id: number): Observable<void> {
        return this.generalExpensesClient.deleteGeneralExpense(id);
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

    postGeneralExpense(model: INewGeneralExpenseViewModel): Observable<number> {
        let dto = new GeneralExpenseCreateDto({
            amount: new PriceCreateDto({
                currencyId: model.amountCurrencyId,
                value: model.amountValue
            }),
            frequencyTypeId: model.frequencyTypeId,
            interval: model.interval,
            name: model.name,
            paymentDate: new Date(Date.UTC(model.paymentDate.getFullYear(), model.paymentDate.getMonth(), model.paymentDate.getDate()))
        });
        return this.generalExpensesClient.createGeneralExpense(dto);
    }

    putGeneralExpense(id: number, model: INewGeneralExpenseViewModel): Observable<void> {
        let dto = new GeneralExpenseUpdateDto({
            cost: new PriceUpdateDto({
                currencyId: model.amountCurrencyId,
                value: model.amountValue
            }),
            frequencyTypeId: model.frequencyTypeId,
            interval: model.interval,
            name: model.name,
            paymentDate: new Date(Date.UTC(model.paymentDate.getFullYear(), model.paymentDate.getMonth(), model.paymentDate.getDate()))
        });
        return this.generalExpensesClient.updateGeneralExpense(id, dto);
    }

    getGeneralExpense(id: number): Observable<INewGeneralExpenseViewModel> {
        return this.generalExpensesClient.getGeneralExpense(id).pipe(map(res => ({
            paymentDate: res.paymentDate,
            name: res.name,
            interval: res.interval,
            frequencyTypeId: res.frequencyTypeId,
            amountValue: res.amount.value,
            amountCurrencyId: res.amount.currencyId
        })));
    }

    getRecurringCosts(): Observable<IRecurringCostViewModel[]> {
        return this.recurringClient.getMonthlyRecurringCost().pipe(map(res => res.map(this.recurringCostDtoToViewModel)));
    }

    private recurringCostDtoToViewModel(dto: RecurrentExpenseListDto): IRecurringCostViewModel {
        return {
            amount: dto.amount.value,
            currency: dto.amount.currencyId,
            id: dto.id,
            name: dto.name
        };
    }

    putRecurringCosts(model: IRecurringCostViewModel[]): Observable<void> {
        let updateDtos = [];
        for (let recurringCost of model) {
            let updateDto = new RecurrentExpenseUpdateDto({
                amount: new PriceUpdateDto({
                    currencyId: recurringCost.currency,
                    value: recurringCost.amount
                }),
                id: recurringCost.id,
                name: recurringCost.name
            });
            updateDtos.push(updateDto);
        }
        return this.recurringClient.updateMonthlyRecurringCosts(updateDtos);
    }

}
