import { Component, OnInit, Inject } from '@angular/core';
import { ICargoPreviewModel } from '../../models/cargo-preview.model';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SupplyService } from '../../services/supply.service';

@Component({
  selector: 'factory-cargo-preview',
  templateUrl: './cargo-preview.component.html',
  styleUrls: ['./cargo-preview.component.scss']
})
export class CargoPreviewComponent implements OnInit {

  cargo: ICargoPreviewModel;
  isLoading = false;
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private service: SupplyService,
  ) { }

  ngOnInit() {
    this.isLoading = true;
    this.service.getPreview(this.data.id).subscribe(res => {
      this.cargo = res;
      this.isLoading = false;
    });
  }

}
