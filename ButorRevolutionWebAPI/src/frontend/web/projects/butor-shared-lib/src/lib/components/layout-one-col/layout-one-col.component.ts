import { Component, OnInit, Input, ContentChild } from '@angular/core';
import { LayoutOneColActionsComponent } from './layout-one-col-actions';
import { LayoutPanelCenterComponent } from './layout-panel-center';

@Component({
  selector: 'butor-layout-one-col',
  templateUrl: './layout-one-col.component.html',
  styleUrls: ['./layout-one-col.component.scss']
})
export class LayoutOneColComponent implements OnInit {
  @Input() breadcrumbs: string;
  @Input() title: string;
  @Input() isLoading: boolean;

  @ContentChild(LayoutOneColActionsComponent) actions: LayoutOneColActionsComponent;
  @ContentChild(LayoutPanelCenterComponent) center: LayoutPanelCenterComponent;
  constructor() { }

  ngOnInit() {
  }

}
