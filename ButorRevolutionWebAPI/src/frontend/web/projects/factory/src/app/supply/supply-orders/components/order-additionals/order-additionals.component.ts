import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IAdditionalsListViewModel } from '../../models/supply-material.models';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'factory-order-additionals',
  templateUrl: './order-additionals.component.html',
  styleUrls: ['./order-additionals.component.scss']
})
export class OrderAdditionalsComponent implements OnInit {

  @Input() additionals: IAdditionalsListViewModel[];
  @Output() setAdditionals = new EventEmitter<IAdditionalsListViewModel[]>();
  @Output() isValid = new EventEmitter<boolean>();
  @Output() isEmpty = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit() {
  }


  changedAmount() {
    let i = this.additionals.findIndex(a => a.underOrderAmount === undefined);
    if (i === -1) {
      this.isValid.emit(true);
    } else {
      this.isValid.emit(false);
    }
  }

  materialPackageChange(code: string, event: any, f: NgForm) {
    this.additionals.find(x => x.materialCode === code).materialPackagesSelected = event;
    if (f.valid) {
      this.isValid.emit(true);
      this.setAdditionals.emit(this.additionals);
    } else {
      this.isValid.emit(false);
      this.setAdditionals.emit(this.additionals);
    }
  }

  orderAmountChange(f: NgForm,i: number, order: number) {
    this.additionals[i].orderdAmount = ((order > 0 || order) ? order : 0);

    if (f.valid) {
      this.isValid.emit(true);
      this.setAdditionals.emit(this.additionals);
    } else {
      this.isValid.emit(false);
      this.setAdditionals.emit(this.additionals);
    }
  }
}
