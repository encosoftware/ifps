import { Injectable } from '@angular/core';
import { IFurnitureUnitListByWebshopCategoryViewModel } from '../../units/models/units';
import { PagedData } from '../models/paged-data.model';
import { BehaviorSubject } from 'rxjs';
import { IGroupingCategoryWebshopViewModel } from '../../home/models/home';

@Injectable({
  providedIn: 'root'
})
export class HeaderSharedService {
  categories: BehaviorSubject<PagedData<IFurnitureUnitListByWebshopCategoryViewModel>> = new BehaviorSubject(null);
  menu: BehaviorSubject<IGroupingCategoryWebshopViewModel[]> = new BehaviorSubject(null);
  constructor() { }

}
