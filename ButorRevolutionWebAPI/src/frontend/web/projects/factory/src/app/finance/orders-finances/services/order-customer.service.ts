import { Injectable } from '@angular/core';
import {
  IOrdersFilterListViewModel,
  IOrdersListViewModel,
  IFinancesOrderViewModel,
  IOrderTicketModel
} from '../models/customerOrders';
import { Observable } from 'rxjs';
import { PagedData } from '../../../shared/models/paged-data.model';
import { map } from 'rxjs/operators';
import {
  ApiOrdersFinancesClient,
  PagedListDtoOfOrderFinanceListDto,
  ApiOrdersClient,
  OrderDetailsDto,
  ApiTicketsOrdersClient,
  TicketByOrderListDto,
  OrderFinanceCreateDto,
  OrderStateEnum,
  OrderStateTypeDropdownListDto,
  ApiOrderstatustypesStatusesFinancesClient
} from '../../../shared/clients';
import { parse, format } from 'date-fns';
import { IOrderSchedulingStatusListViewModel } from '../../../production/order-scheduling/models/order-scheduling.model';

@Injectable({
  providedIn: 'root'
})
export class OrderFinancesService {

  constructor(
    private orderClient: ApiOrdersFinancesClient,
    private orderViewClient: ApiOrdersClient,
    private ticketHistory: ApiTicketsOrdersClient,
    private statusClient: ApiOrderstatustypesStatusesFinancesClient
  ) { }

  getOrders = (companyId, filter: IOrdersFilterListViewModel): Observable<PagedData<IOrdersListViewModel>> => {
    const tempTo = filter.statusDeadline ? new Date(filter.statusDeadline) : undefined;
    const tempFrom = filter.statusDeadline ? new Date() : undefined;
    const tempDTo = filter.deadline ? new Date(filter.deadline) : undefined;
    const tempDFrom = filter.deadline ? new Date() : undefined;
    return this.orderClient.getOrdersByCompany(companyId, filter.orderId, filter.workingNr,
      OrderStateEnum[filter.currentStatus], tempTo, tempFrom, tempDFrom, tempDTo,
      undefined, undefined, filter.pager.pageIndex, filter.pager.pageSize
    ).pipe(
      map((dto: PagedListDtoOfOrderFinanceListDto): PagedData<IOrdersListViewModel> => ({
        items: dto.data.map(x => ({
          id: x.id,
          orderId: x.orderId,
          workingNumber: x.workingNumber,
          currentStatus: x.currentStatus,
          statusDeadline: x.statusDeadline,
          deadline: x.deadline,
          responsible: x.responsible.name,
          amount: x.amount
        })),
        pageIndex: filter.pager.pageIndex,
        pageSize: filter.pager.pageSize,
        totalCount: dto.totalCount
      }))
    );
  }

  getOrder(id: string): Observable<IFinancesOrderViewModel> {
    return this.orderViewClient.getById(id).pipe(
      map((x: OrderDetailsDto): IFinancesOrderViewModel =>
        ({
          orderId: x.orderId,
          currentStatus: x.currentStatus.translation,
          statusDeadline: x.statusDeadline,
          responsible: x.responsible.name,
          createdOn: x.createdOn,
          deadline: x.deadline,
          workingNr: x.workingNumber,
          finances: ({
            firstPaymentDate: x.finances.firstPaymentDate,
            secondPaymentDate: x.finances.secondPaymentDate,
            firstPaymentBool: x.finances.firstPaymentSet,
            secondPaymentBool: x.finances.secondPaymentSet,
            firstPaymentAmount: ({
              value: x.finances.firstPaymentAmount.value,
              currencyId: x.finances.firstPaymentAmount.currencyId,
              currency: x.finances.firstPaymentAmount.currency,
            }),
            secondPaymentAmount: ({
              value: x.finances.secondPaymentAmount.value,
              currencyId: x.finances.secondPaymentAmount.currencyId,
              currency: x.finances.secondPaymentAmount.currency,
            }),
          }),
        })
      )
    );
  }

  getOrderTicket(orderId: string): Observable<IOrderTicketModel[]> {
    return this.ticketHistory.getTicketsByOrder(orderId).pipe(
      map((dto: TicketByOrderListDto[]): IOrderTicketModel[] => {
        return dto.map((x: TicketByOrderListDto): IOrderTicketModel => ({
          assignedTo: x.assignedTo,
          closedOn: format(x.closedOn, 'yyyy. MM. dd.'),
          deadline: format(x.deadline, 'yyyy. MM. dd.'),
          state: x.state
        }));
      })
    );
  }

  addOrderPayment(id: string, index: number, date: Date): Observable<void> {
    const dto = new OrderFinanceCreateDto({
      paymentDate: date,
      paymentIndex: index
    });
    return this.orderClient.addOrderPayment(id, dto);
  }


  getOrderFinanceStatuses(): Observable<IOrderSchedulingStatusListViewModel[]> {
    return this.statusClient.getFinanceOrderStates().pipe(map(res => res.map(this.orderSchedulingStatusesToViewModel)));
  }

  private orderSchedulingStatusesToViewModel(dto: OrderStateTypeDropdownListDto): IOrderSchedulingStatusListViewModel {
    return { name: dto.name, id: dto.id };
  }
}
