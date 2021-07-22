import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { BasketCreateModel } from '../models/shared';
import { BasketDetailsViewModel } from '../../basket/models/basket';

@Injectable({
  providedIn: 'root'
})
export class BasketSharedService {
  basket: BehaviorSubject<BasketDetailsViewModel> = new BehaviorSubject(null);

  constructor() {
    if (localStorage.getItem('basket')) {
      this.basket.next(JSON.parse(localStorage.getItem('basket')));
    }
  }
}