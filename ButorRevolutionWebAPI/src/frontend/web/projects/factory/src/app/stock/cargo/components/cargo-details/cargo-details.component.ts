import { Component, OnInit, Input } from '@angular/core';
import { CargoService } from '../../services/cargo.service';
import { ICargoDetailsViewModel } from '../../models/cargo.model';
import { ICellDropDownViewModel } from '../../../stocked-items/models/stocked-items.model';
import { Router, ActivatedRoute } from '@angular/router';
import { SnackbarService } from 'butor-shared-lib';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'factory-stock-cargo-details',
  templateUrl: './cargo-details.component.html',
  styleUrls: ['./cargo-details.component.scss']
})
export class CargoDetailsComponent implements OnInit {

  cells: ICellDropDownViewModel[];

  id: number;

  @Input() details: ICargoDetailsViewModel;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private cargoService: CargoService,
    private snackBar: SnackbarService,
    private translate: TranslateService
  ) { }

  ngOnInit(): void {
    this.id = +this.route.snapshot.paramMap.get('id');
    this.cargoService.getCells().subscribe(res => this.cells = res);
  }

  isEmptyInputField(): boolean {
    if (this.details.products.findIndex(x => (x.cellId === undefined || x.cellId === 0)) > -1) {
      return true;
    } else {
      return false;
    }
  }

  submit() {
    this.cargoService.putCargoStockDetails(this.id, this.details.products).subscribe(res => {
      this.snackBar.customMessage(this.translate.instant('snackbar.success'));
      this.router.navigate(['/stock/cargo']);
    });
  }

  cancel() {
    this.router.navigate(['/stock/cargo']);
  }

}
