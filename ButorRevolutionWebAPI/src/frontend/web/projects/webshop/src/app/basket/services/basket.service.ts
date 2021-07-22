import { Injectable } from '@angular/core';
import {
  ApiBasketsClient,
  ApiBasketsItemsClient,
  BasketCreateDto,
  BasketUpdateDto,
  OrderedFurnitureUnitCreateDto,
  OrderedFurnitureUnitUpdateDto,
  ApiServicesShippingClient,
  PriceCreateDto
} from '../../shared/clients';
import { Observable } from 'rxjs';
import { BasketDetailsViewModel, ShippingServiceListViewModel } from '../models/basket';
import { BasketCreateModel } from '../../shared/models/shared';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  constructor(
    private basketClient: ApiBasketsClient,
    private basketItemClient: ApiBasketsItemsClient,
    private basketShippingClient: ApiServicesShippingClient,
  ) { }

  getBasketDetails(id: number): Observable<BasketDetailsViewModel | null> {
    return this.basketClient.getBasketDetails(id).pipe(
      map(resp => resp ? ({
        orderedFurnitureUnits: resp.orderedFurnitureUnits.map(x => ({
          furnitureUnitId: x.furnitureUnitId,
          quantity: x.quantity,
          furnitureUnitListDto: ({
            id: x.webshopFurnitureUnitListDto.furnitureUnitId,
            code: x.webshopFurnitureUnitListDto.code,
            description: x.webshopFurnitureUnitListDto.description,
            category: x.webshopFurnitureUnitListDto.category,
            width: x.webshopFurnitureUnitListDto.width,
            height: x.webshopFurnitureUnitListDto.height,
            depth: x.webshopFurnitureUnitListDto.depth,
            sellPrice: x.webshopFurnitureUnitListDto.sellPrice,
            imageThumbnail: x.webshopFurnitureUnitListDto.imageThumbnail,
          }),
        })),
        subTotal: resp.subTotal,
        delieveryPrice: resp.delieveryPrice,
      }) : null
      )
    );

  }

  createBasket(basketCreate: BasketCreateModel): Observable<number> {
    const dto = new BasketCreateDto({
      customerId: basketCreate.customerId,
      orderedFurnitureUnits: basketCreate.orderedFurnitureUnit.map(x => new OrderedFurnitureUnitCreateDto({
        furnitureUnitId: x.furnitureUnitId,
        quantity: +x.quantity
      }))
    });
    return this.basketClient.createBasket(dto);

  }

  updateBasketItem(id: number, basketUpdateDto: BasketCreateModel, isUpdate = true): Observable<void> {
    const dto: BasketUpdateDto = new BasketUpdateDto({
      customerId: basketUpdateDto.customerId,
      orderedFurnitureUnits: basketUpdateDto.orderedFurnitureUnit.map(x => new OrderedFurnitureUnitUpdateDto({
        furnitureUnitId: x.furnitureUnitId,
        quantity: x.quantity
      })),
      deliveryPrice: new PriceCreateDto({
        value: basketUpdateDto.deliveryPrice ? basketUpdateDto.deliveryPrice.value : undefined,
        currencyId: basketUpdateDto.deliveryPrice ? basketUpdateDto.deliveryPrice.currencyId : 1
      })
    });
    return this.basketClient.updateBasket(id, dto, isUpdate);

  }

  deleteBasketItem(basketId: number, furnitureUnitId: string): Observable<void> {
    return this.basketItemClient.deleteBasketItem(basketId, furnitureUnitId);
  }

  getShippingServicesForDropdown(): Observable<ShippingServiceListViewModel[] | null> {
    return this.basketShippingClient.getShippingServicesForDropdown().pipe(
      map(res => res.map(x => ({
        id: x.id,
        description: x.description,
        price: ({
          currency: x.price.currency,
          currencyId: x.price.currencyId,
          value: x.price.value,
        })
      })
      )
      )
    );
  }

  synchronizeBaskets(basketId: number, otherBasketId: number): Observable<number> {
    return this.basketClient.synchronizeBaskets(basketId, otherBasketId);
  }
}
