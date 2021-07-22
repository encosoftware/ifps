import { Component, OnInit, Input, InjectionToken } from '@angular/core';
import { IHamburgerMenuModel } from 'butor-shared-lib';
import { MenuPositionX, MenuPositionY, MatMenuDefaultOptions } from '@angular/material/menu';
import { Router } from '@angular/router';

@Component({
  selector: 'factory-material-menu',
  templateUrl: './material-menu.component.html',
  styleUrls: ['./material-menu.component.scss']
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
