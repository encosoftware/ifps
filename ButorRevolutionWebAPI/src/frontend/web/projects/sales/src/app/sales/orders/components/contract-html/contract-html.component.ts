import { Component, OnInit, Input } from '@angular/core';
import { ControlContainer, FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'butor-contract-html',
  templateUrl: './contract-html.component.html',
  styleUrls: ['./contract-html.component.css'],
  viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class ContractHtmlComponent implements OnInit {
  @Input() data;

  constructor() { }

  ngOnInit(): void {
  }

}
