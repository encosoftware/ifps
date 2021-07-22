import { Component, OnInit, Input, ContentChild } from '@angular/core';
import { LayoutTwoColActionsComponent } from './layout-two-col-actions.component';
import { LayoutPanelRightComponent } from './layout-panel-right';
import { LayoutPanelLeftComponent } from './layout-panel-left';

@Component({
  selector: 'butor-layout-two-col',
  templateUrl: './layout-two-col.component.html',
  styleUrls: ['./layout-two-col.component.scss']
})
export class LayoutTwoColComponent implements OnInit {

  @Input() breadcrumbs: string;
  @Input() title: string;
  @Input() isLoading: boolean;

  @ContentChild(LayoutTwoColActionsComponent) actions: LayoutTwoColActionsComponent;
  @ContentChild(LayoutPanelRightComponent) right: LayoutPanelRightComponent;
  @ContentChild(LayoutPanelLeftComponent) left: LayoutPanelLeftComponent;

  constructor() { }

  ngOnInit() {
  }

}
