import { Component, OnInit } from '@angular/core';
import { IArrivedCargoViewModel } from '../../models/arrived-cargo.model';
import { ActivatedRoute } from '@angular/router';
import { CargoService } from '../../services/cargo.service';

@Component({
  selector: 'factory-arrived-cargo',
  templateUrl: './arrived-cargo.component.html',
  styleUrls: ['./arrived-cargo.component.scss']
})
export class ArrivedCargoComponent implements OnInit {

  cargo: IArrivedCargoViewModel[];

  cargoId: number;
  isArrived;

  isLoading = false;
  error: string | null = null;
  displayed = ['material', 'name', 'price', 'amount', 'missing', 'refused', 'subTotal'];

  constructor(
    private route: ActivatedRoute,
    private service: CargoService,
  ) { }

  ngOnInit() {
    this.isLoading = true;
    this.cargoId = +this.route.snapshot.paramMap.get('id');
    this.isArrived = !!this.route.snapshot.queryParamMap.get('arrived');
    this.service.getArrivedCargo(this.cargoId).subscribe(res => {
      this.cargo = res;
      if (this.isArrived) {
        this.displayed = ['material', 'name', 'price', 'amount', 'packageCode', 'packageName', 'packageSize',  'subTotal'];
      } else {
        this.displayed = ['material', 'name', 'price', 'amount', 'missing', 'refused', 'packageCode', 'packageName', 'packageSize', 'subTotal'];

      }
      this.isLoading = false;
    });
    
  }

}
