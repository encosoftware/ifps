import { Component, OnInit } from '@angular/core';
import { IimagesSliderModel, ConfigImageSlider } from '../../shared/models/image-slider';
import { DetailsService } from '../services/details.service';
import { ActivatedRoute } from '@angular/router';
import { FurnitureUnitByWebshopDetailsViewModel, FurnitureUnitByWebshopDetailsRecomendationModel, WebshopFurnitureUnitInBasketIdsViewModel } from '../models/details';
import { tap, finalize, map, debounceTime, switchMap, take } from 'rxjs/operators';
import { BasketCreateModel } from '../../shared/models/shared';
import { BasketService } from '../../basket/services/basket.service';
import { BasketSharedService } from '../../shared/services/basket-shared.service';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../login/store/selectors/core.selector';
import { SnackbarService } from 'butor-shared-lib';

@Component({
  selector: 'app-products-details',
  templateUrl: './products-details.component.html',
  styleUrls: ['./products-details.component.scss']
})
export class ProductsDetailsComponent implements OnInit {

  images: IimagesSliderModel[] = [];
  isLoading = false;
  parentSelect = [{
    name: 'Black',
    id: 1
  }];
  quantity = 1;
  detailsList: FurnitureUnitByWebshopDetailsViewModel;
  recommendedBasketModel: WebshopFurnitureUnitInBasketIdsViewModel;
  sliderConfig: ConfigImageSlider = {
    slidesToShow: 5,
    slidesToScroll: 1,
    infinite: false
  };
  recommendation: FurnitureUnitByWebshopDetailsRecomendationModel[] = [];
  id: string;
  userId: number;
  constructor(
    private details: DetailsService,
    private route: ActivatedRoute,
    private basketService: BasketService,
    private basketSharedService: BasketSharedService,
    private store: Store<any>,
    private snackbar: SnackbarService

  ) { }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');
    this.route.params.pipe(
      tap(_ => this.isLoading = true),
      map(res => this.id = res['id']),
      switchMap(store =>
        this.store.pipe(
          select(coreLoginT),
          take(1)),
      ),
      switchMap(id => {
        let uId = +id.UserId === 0 ?  0 : +id.UserId;
        this.recommendedBasketModel = ({
          recommendedItemNum: 3,
          furnitureUnitIds: []
        });
        return this.details.getRecommendedFurnitureUnits(this.recommendedBasketModel).pipe(
          map(resp => {
            this.userId = +id.UserId;
            this.recommendation = resp;
          })
        );
      }
      ),
      switchMap(recom =>
        this.details.getFurnitureUnitByWebshopById(+this.id).pipe(
          map(resp => this.detailsList = ({
            name: resp.name,
            furnitureUnitId: resp.furnitureUnitId,
            description: resp.description,
            width: resp.width,
            height: resp.height,
            depth: resp.depth,
            price: ({
              currency: resp.price.currency,
              currencyId: resp.price.currencyId,
              value: resp.price.value,
            }),
            image: resp.image.map(img => ({
              containerName: img.containerName,
              fileName: img.fileName
            })),

          })),
          finalize(() => this.isLoading = false)
        )
      )
    ).subscribe();
  }

  createBasket() {
    let basket: BasketCreateModel = ({
      customerId: this.userId === 0 ?  null : this.userId,
      orderedFurnitureUnit: [({
        furnitureUnitId: this.detailsList.furnitureUnitId,
        quantity: this.quantity,
      })]
    });
    if (localStorage.getItem('basketId') && (this.quantity > 0)) {
      this.basketService.updateBasketItem(+localStorage.getItem('basketId'), basket).pipe(
        switchMap(create =>
          this.basketService.getBasketDetails(+localStorage.getItem('basketId')).pipe(
            tap(x => this.basketSharedService.basket.next(x)),
          )),
          finalize(()=> this.snackbar.customMessage('Add to basket'))
      ).subscribe();
    } else if ((this.quantity > 0)) {

      this.basketService.createBasket(basket)
        .pipe(
          tap(resp => localStorage.setItem('basketId', JSON.stringify(resp))),
          switchMap(create =>
            this.basketService.getBasketDetails(create).pipe(
              tap(x => this.basketSharedService.basket.next(x))
            )),
            finalize(()=> this.snackbar.customMessage('Add to basket'))
        ).subscribe();
    }
  }
}
