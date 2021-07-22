import { Component, OnInit } from '@angular/core';
import { CargoService } from '../../services/cargo.service';
import { ICargoEditViewModel } from '../../models/cargo.model';
import { ActivatedRoute } from '@angular/router';

@Component({
    templateUrl: './edit-cargo.page.component.html',
    styleUrls: ['./edit-cargo.page.component.scss']
})
export class StockCargoPageComponent implements OnInit {

    isLoading = false;

    cargo: ICargoEditViewModel;

    id: number;

    constructor(
        private cargoService: CargoService,
        private route: ActivatedRoute,
    ) { }

    ngOnInit(): void {
        this.id = +this.route.snapshot.paramMap.get('id');
        this.isLoading = true;
        this.cargoService.getCargo(this.id).subscribe(res => {
            this.cargo = res;
        },
        () => {},
        () => this.isLoading = false);
    }

}
