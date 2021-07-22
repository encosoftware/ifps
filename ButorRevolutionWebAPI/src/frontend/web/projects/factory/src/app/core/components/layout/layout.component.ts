import { Component, OnInit } from '@angular/core';
import { MenuOpenService } from '../../services/menu-open.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'factory-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {
  isOpenMenu: boolean;

  constructor(
    private menuService: MenuOpenService,
    public translate: TranslateService,
  ) { }

  ngOnInit() {
    this.menuService.currentMenuOpen.subscribe(open => this.isOpenMenu = open);
  }
  isMenuOpen(openMenu: boolean) {
    this.menuService.changeMenuOpen(openMenu);

  }

}
