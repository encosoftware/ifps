import { Component, OnInit, ViewChildren, HostListener } from '@angular/core';
import { Options } from 'ng5-slider';
import { UnitsService } from '../services/units.service';
import { IFurnitureUnitListByWebshopCategoryFilterViewModel, IFurnitureUnitListByWebshopCategoryViewModel } from '../models/units';
import { ActivatedRoute } from '@angular/router';
import { PagedData } from '../../shared/models/paged-data.model';
import { map, tap, debounceTime, distinctUntilChanged, switchMap, filter } from 'rxjs/operators';
import { HeaderSharedService } from '../../shared/services/header-shared.service';

@Component({
  selector: 'app-units',
  templateUrl: './units.component.html',
  styleUrls: ['./units.component.scss']
})
export class UnitsComponent implements OnInit {

  @ViewChildren('myanchor') anchors;
  openMenuMat = false;
  openMenuPrice = false;
  openMenuSortby = false;
  isLoading = false;
  choose: string;
  minValue = 1;
  maxValue = 500;
  search = false;
  priceLoaded = false;
  options: Options = {
    floor: 1,
    ceil: 500,
    noSwitching: true,
    getPointerColor: (value: number): string => {
      return 'rgba(77, 65, 255, 1)';
    }
  };
  filter: IFurnitureUnitListByWebshopCategoryFilterViewModel = {
    subcategoryId: undefined,
    Orderings: [],
    pageIndex: undefined,
    pageSize: 8,
  };
  unitsList: PagedData<IFurnitureUnitListByWebshopCategoryViewModel> = { items: [], pageIndex: 0, pageSize: 8 };
  recordsPerPage = 4;
  next = false;
  scrollTo = false;
  loadMoreIsShow = false;
  showedItemsCount = 0;
  @HostListener('document:scroll')
  element() {
    if (this.anchors) {
      this.scrollTo = this.inView(this.anchors);
    }
  }

  constructor(
    private units: UnitsService,
    private route: ActivatedRoute,
    private header: HeaderSharedService,
  ) { }

  ngOnInit() {
    // this.filter.subcategoryId = +this.route.snapshot.paramMap.get('id');
    this.header.categories.pipe(
      tap(_ => this.isLoading = true),
      map(x => this.unitsList = x),
      filter(x => x !== null),
      tap(units => {
        this.filter.pageIndex = units.pageIndex;
        this.filter.totalCount = units.totalCount;
        this.showedItemsCount = units.totalCount > 8 ? this.filter.pageSize : units.totalCount;
        this.loadMoreIsShow = units.totalCount > this.filter.pageSize ? true : false;
      }),
      switchMap((param) => {
        const numArr: number[] = param.items.map(x => x.categoryId);
        return this.units.getMaximumPriceFromWFUList(numArr).pipe(
          map(resp => {
            this.options.ceil = resp.value;
            this.maxValue = resp.value;
          }),
        );
      }
      ),
    ).subscribe(() => this.isLoading = false);

    this.route.params.pipe(
      map(res => this.filter.subcategoryId = res['id']),
      filter(x => x !== undefined),
      tap(() => this.search = true),
      tap(() => this.filter.pageSize = 8),
      switchMap(maxP =>
        this.units.getFurnitureUnitsByWebshopCategory(this.filter).pipe(
          map(units => this.header.categories.next(units)),
        )),
    ).subscribe();

  }

  toggleMaterial() {
    this.openMenuMat = !this.openMenuMat;
  }

  togglePrice() {
    this.openMenuPrice = !this.openMenuPrice;
  }

  toggleSortby() {
    this.openMenuSortby = !this.openMenuSortby;
  }

  setValueFilter() {
    this.filter.maximumPrice = this.maxValue;
    this.filter.minimumPrice = this.minValue;
    this.units.getFurnitureUnitsByWebshopCategory(this.filter).pipe(
      debounceTime(1000),
      distinctUntilChanged((x, y) => x === y),
      map(units => this.header.categories.next(units)),
    ).subscribe();
  }

  sortby(sortFilter: string) {
    this.choose = sortFilter;
    this.filter.Orderings = [
      ({
        column: 'MinimumPrice',
        isDescending: false,
      })
    ];

    if (sortFilter === 'height') {
      this.filter.Orderings[0].isDescending = false;
    } else if (sortFilter === 'low') {
      this.filter.Orderings[0].isDescending = true;
    } else {
      this.filter.Orderings = undefined;
    }
    this.units.getFurnitureUnitsByWebshopCategory(this.filter).pipe(
      debounceTime(1000),
      distinctUntilChanged((x, y) => x === y),
      map(units => this.header.categories.next(units)),
    ).subscribe();

  }

  loadMore() {
    const maxPage = Math.ceil((this.filter.totalCount - this.filter.pageSize) / this.recordsPerPage);
    if ((maxPage > 0) && !(maxPage === 1)) {
      this.filter.pageSize += 4;
    } else if ((maxPage === 1)) {
      this.filter.pageSize += this.filter.totalCount % this.filter.pageSize;
      this.next = true;
    }
    this.units.getFurnitureUnitsByWebshopCategory(this.filter).pipe(
      debounceTime(1000),
      distinctUntilChanged((x, y) => x === y),
      map(units => this.header.categories.next(units)),
    ).subscribe();
  }

  scroll() {
    window.scrollTo(0, 0);
  }

  inView(element): boolean {
    if (element && element.first && element.first.nativeElement) {
      let el = element.first.nativeElement;
      let top = el.offsetTop;
      let left = el.offsetLeft;
      let width = el.offsetWidth;
      let height = el.offsetHeight;

      while (el.offsetParent) {
        el = el.offsetParent;
        top += el.offsetTop;
        left += el.offsetLeft;
      }

      return (
        top >= window.pageYOffset &&
        left >= window.pageXOffset &&
        (top + height) <= (window.pageYOffset + window.innerHeight) &&
        (left + width) <= (window.pageXOffset + window.innerWidth)
      );
    }
    return false;
  }
}
