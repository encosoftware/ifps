import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { OrderFinancesService } from '../../services/order-customer.service';
import { map, finalize, switchMap } from 'rxjs/operators';
import { IOrderDetailsBasicInfoViewModel, OrderFinanceDetailsModel, IOrderTicketModel } from '../../models/customerOrders';
import { LanguageSetService } from '../../../../core/services/language-set.service';
import { DateAdapter } from '@angular/material/core';

@Component({
  selector: 'factory-orders-view',
  templateUrl: './orders-view.component.html',
  styleUrls: ['./orders-view.component.scss']
})
export class OrdersViewComponent implements OnInit {
  isLoading = false;
  subscription$: Subscription;
  id: string;
  order: IOrderDetailsBasicInfoViewModel[];
  payment: OrderFinanceDetailsModel;
  orderHistory: IOrderTicketModel[];
  dateFirst: Date;
  dateSecond: Date;
  lng: string;
  constructor(
    private route: ActivatedRoute,
    private service: OrderFinancesService,
    private lngService: LanguageSetService,
    private dateAdapter: DateAdapter<any>
  ) { }

  ngOnInit(): void {
    this.isLoading = true;
    this.id = this.route.snapshot.paramMap.get('id');

    this.lng = this.lngService.getLocalLanguageStorage();
    this.dateAdapter.setLocale(this.lng);

    this.service.getOrder(this.id).pipe(
      map((order) => {
        this.order = [({
          orderId: order.orderId,
          currentStatus: order.currentStatus,
          statusDeadline: order.statusDeadline,
          responsible: order.responsible,
          createdOn: order.createdOn,
          deadline: order.deadline,
          workingNr: order.workingNr
        })];
        this.payment = order.finances;
      }),
      switchMap((orderGet) =>
        this.service.getOrderTicket(this.id).pipe(
          map((orderHistory) => {
            this.orderHistory = orderHistory;
          })
        )
      ),
      finalize(() => this.isLoading = false)
    ).subscribe();
  }

  paymentChoose(id: number) {
    console.log('choosen:', id);
  }

  addOrderPayment(id: string, index: number, date: Date) {
    this.service.addOrderPayment(id, index, date).pipe(
      map(res => res),
      switchMap(() =>
        this.service.getOrder(this.id).pipe(
        map((order) => {
          this.order = [({
            orderId: order.orderId,
            currentStatus: order.currentStatus,
            statusDeadline: order.statusDeadline,
            responsible: order.responsible,
            createdOn: order.createdOn,
            deadline: order.deadline,
            workingNr: order.workingNr
          })];
          this.payment = order.finances;
        }),
        switchMap((orderGet) =>
          this.service.getOrderTicket(this.id).pipe(
            map((orderHistory) => {
              this.orderHistory = orderHistory;
            })
          )
        ),
        finalize(() => this.isLoading = false)
      ))
    ).subscribe();
  }
}
