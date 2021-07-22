import { Injectable } from '@angular/core';
import { ApiWebshopordersClient, ApiWebshopordersOrderedFurnitureUnitsClient } from '../../shared/clients';
import { Observable } from 'rxjs';
import { WebshopOrdersListViewModel, WebshopOrdersDetailsViewModel } from '../models/orders';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {

  constructor(
    private webshopClient: ApiWebshopordersClient,
    private detailsWebshopOrder: ApiWebshopordersOrderedFurnitureUnitsClient) { }

  getOrdersByCustomerIdByWebshop(customerId: number): Observable<WebshopOrdersListViewModel[] | null> {
    return this.webshopClient.getOrdersByCustomerIdByWebshop(customerId).pipe(
      map(resp => resp.map(ins => ({
        id: ins.id,
        orderName: ins.orderName,
        date: ins.date,
        total: ({
          value: ins.subTotal.value + ins.delieveryPrice.value,
          currencyId: ins.subTotal.currencyId,
          currency: ins.subTotal.currency
        })
      })))
    );
  }

  getOrderedFurnitureUnitsDetails(webshopOrderId: string): Observable<WebshopOrdersDetailsViewModel | null> {
    return this.detailsWebshopOrder.getOrderedFurnitureUnitsDetails(webshopOrderId).pipe(
      map(res => ({
        orderedFurnitureUnits: res.orderedFurnitureUnits.map(furUnit => ({
          name: furUnit.name ? furUnit.name : undefined,
          description: furUnit.description ? furUnit.description : undefined,
          width: furUnit.width,
          height: furUnit.height,
          depth: furUnit.depth,
          quantity: furUnit.quantity,
          unitPrice: furUnit.unitPrice ? ({
            value: furUnit.unitPrice.value ? furUnit.unitPrice.value : undefined,
            currencyId: furUnit.unitPrice.currencyId ? furUnit.unitPrice.currencyId : undefined,
            currency: furUnit.unitPrice.currency ? furUnit.unitPrice.currency : undefined
          }) : undefined,
          subTotal: furUnit.subTotal ? ({
            value: furUnit.subTotal.value ? furUnit.subTotal.value : undefined,
            currencyId: furUnit.subTotal.currencyId ? furUnit.subTotal.currencyId : undefined,
            currency: furUnit.subTotal.currency ? furUnit.subTotal.currency : undefined
          }) : undefined,
          image: furUnit.image ? ({
            containerName: furUnit.image.containerName,
            fileName: furUnit.image.fileName
          }) : undefined
        })),
        total: ({
          subtotal: res.total.subtotal ? ({
            value: res.total.subtotal.value ? res.total.subtotal.value : undefined,
            currencyId: res.total.subtotal.currencyId ? res.total.subtotal.currencyId : undefined,
            currency: res.total.subtotal.currency ? res.total.subtotal.currency : undefined
          }) : undefined,
          deliveryPrice: res.total.deliveryPrice ? ({
            value: res.total.deliveryPrice.value ? res.total.deliveryPrice.value : undefined,
            currencyId: res.total.deliveryPrice.currencyId ? res.total.deliveryPrice.currencyId : undefined,
            currency: res.total.deliveryPrice.currency ? res.total.deliveryPrice.currency : undefined
          }) : undefined,
        })
      }))
    );
  }
}
