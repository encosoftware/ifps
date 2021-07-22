import { Injectable } from '@angular/core';
import { ApiBasketsClient, ApiWebshopfurnitureunitsWebshopClient, ApiCustomersClient, ApiWebshopfurnitureunitsRecommendationClient, WebshopFurnitureUnitInBasketIdsDto } from '../../shared/clients';
import { Observable } from 'rxjs';
import { FurnitureUnitByWebshopDetailsViewModel, FurnitureUnitByWebshopDetailsRecomendationModel, WebshopFurnitureUnitInBasketIdsViewModel } from '../models/details';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DetailsService {

  constructor(
    private furnitureunitsWebshopClient: ApiWebshopfurnitureunitsWebshopClient,
    private basket: ApiBasketsClient,
    private webshopfurnitureunitsRecommendationClient: ApiWebshopfurnitureunitsRecommendationClient,
    private customerClient: ApiCustomersClient
  ) { }

  getFurnitureUnitByWebshopById(furnitureunitId: number): Observable<FurnitureUnitByWebshopDetailsViewModel | null> {
    return this.furnitureunitsWebshopClient.getFurnitureUnitByWebshopById(furnitureunitId).pipe(
      map(resp => ({
        name: resp.name,
        furnitureUnitId: resp.furnitureUnitId,
        description: resp.description,
        width: resp.width,
        height: resp.height,
        depth: resp.depth,
        price: resp.price ? ({
          value: resp.price.value,
          currencyId: resp.price.currencyId,
          currency: resp.price.currency,
        }) : undefined,
        image: resp.images ? resp.images.map(img => ({
          containerName: img.containerName,
          fileName: img.fileName
        })) : undefined,
      }))
    );
  }

  getRecommendedFurnitureUnits(recommendedBasketModel: WebshopFurnitureUnitInBasketIdsViewModel): Observable<FurnitureUnitByWebshopDetailsRecomendationModel[] | null> {
    let dto = new WebshopFurnitureUnitInBasketIdsDto({
      furnitureUnitIds: null,
      recommendedItemNum: recommendedBasketModel.recommendedItemNum
    });
    return this.webshopfurnitureunitsRecommendationClient.getRecommendedFurnitureUnits(dto).pipe(
      map(resp => resp.map(ins => ({
        webshopFurnitureUnitId: ins.webshopFurnitureUnitId,
        name: ins.code,
        description: ins.description,
        width: ins.width,
        height: ins.height,
        depth: ins.depth,
        price: ins.price ? ({
          value: ins.price.value,
          currencyId: ins.price.currencyId,
          currency: ins.price.currency,
        }) : undefined,
        image: ins.image ?
          ({
            containerName: ins.image.containerName,
            fileName: ins.image.fileName
          }) : undefined
      })
      )
      )
    );
  }

  getFurnitureRecommendationsForCustomer(customerId: number): Observable<FurnitureUnitByWebshopDetailsRecomendationModel[] | null> {
    return this.customerClient.getFurnitureRecommendationsForCustomer(customerId).pipe(
      map(resp => resp.map(ins => ({
        webshopFurnitureUnitId: ins.webshopFurnitureUnitId,
        name: ins.code,
        description: ins.description,
        width: ins.width,
        height: ins.height,
        depth: ins.depth,
        price: ins.price ? ({
          value: ins.price.value,
          currencyId: ins.price.currencyId,
          currency: ins.price.currency,
        }) : undefined,
        image: ins.image ?
          ({
            containerName: ins.image.containerName,
            fileName: ins.image.fileName
          }) : undefined
      })
      )
      )
    );
  }
}
