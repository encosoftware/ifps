import { Component, OnInit, Input, TemplateRef } from '@angular/core';

@Component({
  selector: 'butor-panel',
  templateUrl: './panel.component.html',
  styleUrls: ['./panel.component.scss']
})
export class PanelComponent implements OnInit {

  @Input() title: string;
  @Input() template: TemplateRef<any>;
  @Input() templateTop: TemplateRef<any>;
  @Input() templateAction: TemplateRef<any>;
  constructor() { }

  ngOnInit() {
  }

}
