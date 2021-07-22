import { Component, OnInit, Input, ContentChild } from '@angular/core';
import { LayoutThreeColActionsComponent } from './layout-three-col-actions.components';
import { LayoutPanelMiddleComponent } from './layout-panel-middle';
import { LayoutPanelRightComponent, LayoutPanelLeftComponent } from 'butor-shared-lib';

@Component({
  selector: 'butor-layout-three-col',
  templateUrl: './layout-three-col.component.html',
  styleUrls: ['./layout-three-col.component.scss']
})
export class LayoutThreeColComponent implements OnInit {

  @Input() breadcrumbs: string;
  @Input() title: string;
  @Input() isLoading: boolean;

  @ContentChild(LayoutThreeColActionsComponent, {static: true}) actions: LayoutThreeColActionsComponent;
  @ContentChild(LayoutPanelRightComponent, {static: true}) right: LayoutPanelRightComponent;
  @ContentChild(LayoutPanelLeftComponent, {static: true}) left: LayoutPanelLeftComponent;
  @ContentChild(LayoutPanelMiddleComponent, {static: true}) middle: LayoutPanelMiddleComponent;

  constructor() { }

  ngOnInit() {
  }

}
