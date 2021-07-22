import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IMaterialsListViewModel } from '../../models/supply-material.models';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'factory-supply-materials',
  templateUrl: './supply-materials.component.html',
  styleUrls: ['./supply-materials.component.scss'],

})
export class SupplyMaterialsComponent implements OnInit {

  @Input() materials: IMaterialsListViewModel[];
  @Output() materialsSet = new EventEmitter<IMaterialsListViewModel[]>();
  @Output() isValid = new EventEmitter<boolean>();
  @Output() isEmpty = new EventEmitter<boolean>();


  constructor() { }

  ngOnInit() {
  }

  materialPackageChange(code: string, event, f: NgForm): void {
    this.materials.find(x => x.materialCode === code).materialPackagesSelected = event;
    if (f.valid) {
      this.isValid.emit(true);
      this.materialsSet.emit(this.materials);
    } else {
      this.isValid.emit(false);
      this.materialsSet.emit(this.materials);

    }
  }

  orderModelChange(f: NgForm, i: number, order: number) {
    this.materials[i].orderdAmount = ((order > 0 || order) ? order : 0);
    if (f.valid) {
      this.isValid.emit(true);
      this.materialsSet.emit(this.materials);
    } else {
      this.isValid.emit(false);
      this.materialsSet.emit(this.materials);
    }
  }

}
