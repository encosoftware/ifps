import { Component, OnInit } from '@angular/core';
import { ICargoDetailsViewModel } from '../../models/cargo.model';
import { ActivatedRoute, Router } from '@angular/router';
import { CargoService } from '../../services/cargo.service';

@Component({
  selector: 'factory-confirmation-cargo',
  templateUrl: './confirmation-cargo.component.html',
  styleUrls: ['./confirmation-cargo.component.scss']
})
export class ConfirmationCargoComponent implements OnInit {

  cargo: ICargoDetailsViewModel[];
  cargoId: number;
  confirmationDisable = true;

  isLoading = false;
  error: string | null = null;
  constructor(
    private route: ActivatedRoute,
    private service: CargoService,
    private router: Router,
  ) { }

  ngOnInit() {
    this.isLoading = true;
    this.cargoId = +this.route.snapshot.paramMap.get('id');
    this.service.getConfirmationCargo(this.cargoId).subscribe(res => {
      this.cargo = res;
      this.isLoading = false;
    });
  }

  checkProduct(id: number): void {
    this.cargo[0].productList.find(x => x.id === id).isChecked = true;
    this.checkStatuses();
  }

  confirm(): void {
    this.isLoading = true;
    this.cargo[0].isArrived = true;
    this.service.updateCargoConfirmation(this.cargoId, this.cargo).subscribe(res => this.router.navigate(['/supply/cargo']));
  }

  checkStatuses(): void {
    // tslint:disable-next-line:max-line-length
    if (this.cargo[0].productList.find(x => x.missing !== 0 || x.refused !== 0 || (x.isChecked === true && x.missing === 0 && x.refused === 0)) !== undefined) {
      this.confirmationDisable = false;
    } else {
      this.confirmationDisable = true;
    }
  }

}
