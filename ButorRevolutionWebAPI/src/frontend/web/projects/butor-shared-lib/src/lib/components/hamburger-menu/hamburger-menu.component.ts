import { Component, Input, ChangeDetectionStrategy } from '@angular/core';
import { Router } from '@angular/router';

import { IHamburgerMenuModel } from '../../models/hamburger-menu.model';


@Component({
    selector: 'butor-hamburger-menu',
    templateUrl: './hamburger-menu.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class HamburgerMenuComponent {

    @Input('links')
    model: IHamburgerMenuModel;

    constructor(private router: Router) { }

    clickHandler(item: IHamburgerMenuModel) {
        if (typeof item.action === 'string') {
            this.router.navigate([item.action]);
        } else if (item.action instanceof Function) {
            item.action();
        }
    }

}
