import { Component, OnInit, Input, ChangeDetectionStrategy, InjectionToken, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { MenuPositionX, MenuPositionY, MatMenuDefaultOptions } from '@angular/material/menu';
import { IHamburgerMenuModel } from 'butor-shared-lib/public-api';

@Component({
  selector: 'butor-material-menu',
  templateUrl: './material-menu.component.html',
  styleUrls: ['./material-menu.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,

})
export class MaterialMenuComponent implements OnInit {

  @Input('links') model: IHamburgerMenuModel;
  @Input()
  xPosition: MenuPositionX;
  @Input()
  yPosition: MenuPositionY;
  MAT_MENU_DEFAULT_OPTIONS: InjectionToken<MatMenuDefaultOptions>;

  constructor(private router: Router) { }

  ngOnInit() {
  }

  clickHandler(item: IHamburgerMenuModel) {
    if (typeof item.action === 'string') {
      this.router.navigate([item.action]);
    } else if (item.action instanceof Function) {
      item.action();
    }
  }

}
