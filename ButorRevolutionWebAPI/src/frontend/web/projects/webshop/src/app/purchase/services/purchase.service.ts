import { Injectable } from '@angular/core';
import { ApiBasketsPurchaseClient, BasketPurchaseDto, AddressCreateDto, ApiCountriesClient } from '../../shared/clients';
import { Observable } from 'rxjs';
import { BasketPurchaseViewModel, CountryListSelectModel } from '../models/purchase';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PurchaseService {

  constructor(
    private basketsPurchaseClient: ApiBasketsPurchaseClient,
    private countries: ApiCountriesClient
    ) { }

  purchaseBasket(id: number, basketPurchaseDto: BasketPurchaseViewModel | null): Observable<void> {
    const create = new BasketPurchaseDto({
      customerId: basketPurchaseDto.customerId,
      name: basketPurchaseDto.name,
      emailAddress: basketPurchaseDto.emailAddress,
      note: basketPurchaseDto.note,
      taxNumber: basketPurchaseDto.taxNumber,
      gaveEmailConsent: basketPurchaseDto.gaveEmailConsent,
      delieveryAddress: new AddressCreateDto({
        address: basketPurchaseDto.delieveryAddress.address,
        postCode: basketPurchaseDto.delieveryAddress.postCode,
        city: basketPurchaseDto.delieveryAddress.city,
        countryId: basketPurchaseDto.delieveryAddress.countryId,
      }
      ),
      billingAddress: new AddressCreateDto({
        address: basketPurchaseDto.billingAddress.address,
        postCode: basketPurchaseDto.billingAddress.postCode,
        city: basketPurchaseDto.billingAddress.city,
        countryId: basketPurchaseDto.billingAddress.countryId,
      }),
    });
    return this.basketsPurchaseClient.purchaseBasket(id, create);

  }

  getCountries(): Observable<CountryListSelectModel[]> {
    return this.countries.getCountries().pipe(
      map((cy) => cy.map((c) =>
        ({
          id: c.id,
          name: c.translation
        })))
    );
  }
}
