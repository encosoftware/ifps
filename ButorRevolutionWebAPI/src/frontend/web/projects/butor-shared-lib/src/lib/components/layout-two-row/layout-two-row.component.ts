import { Component, OnInit, Input, ContentChild } from '@angular/core';
import { LayoutPanelTopComponent } from './layout-panel-top';
import { LayoutPanelBottomComponent } from './layout-panel-bottom';

@Component({
  selector: 'butor-layout-two-row',
  templateUrl: './layout-two-row.component.html',
  styleUrls: ['./layout-two-row.component.scss']
})
export class LayoutTwoRowComponent implements OnInit {
  @Input() isLoading: boolean;

  @ContentChild(LayoutPanelTopComponent) top: LayoutPanelTopComponent;
  @ContentChild(LayoutPanelBottomComponent) bottom: LayoutPanelBottomComponent;

  constructor() { }

  ngOnInit() {
  }

}
