import { Component, OnInit, Input, Output } from '@angular/core';

@Component({
  selector: 'butor-btn-hamburger-inside',
  template: `
    <div style="display: flex" class="hamburger-menu-list-item">
      <i [style.color]="color" class="{{ iconClass }}"></i>
      <button [style.color]="color" [disabled]="isDisabled">
        {{ text }}
      </button>
    </div>
  `,
  styles: [``]
})
export class BtnHamburgerInsideComponent implements OnInit {
  // @Input() item: IHamburgerMenuModel[];
  @Input('textColor') color = 'blue';
  @Input() iconClass: string;
  @Input('buttonText') text: string;
  @Input() isDisabled = false;

  constructor() {}

  ngOnInit() {}
}
