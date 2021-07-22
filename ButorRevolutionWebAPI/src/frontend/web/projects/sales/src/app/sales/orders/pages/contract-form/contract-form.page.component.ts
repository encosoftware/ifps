import { Component, OnInit } from '@angular/core';
import { OrdersService } from '../../services/orders.service';
import { IContractViewModel, IOrderDetailsBasicInfoViewModel } from '../../models/orders';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ContractService } from '../../services/contract.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
    templateUrl: './contract-form.page.component.html',
    styleUrls: ['./contract-form.page.component.scss']
})
export class ContractFormPageComponent implements OnInit {

    orderId: string;
    contract: IContractViewModel;
    orderBasics: IOrderDetailsBasicInfoViewModel;

    constructor(
        private orderService: OrdersService,
        private contractServive: ContractService,
        private route: ActivatedRoute,
        private router: Router,
        private translate: TranslateService,
    ) { }

    ngOnInit(): void {
        this.orderId = this.route.snapshot.paramMap.get('id');
        forkJoin([
            this.orderService.getOrderBasics(this.orderId),
            this.contractServive.getContract(this.orderId)
        ]).pipe(
            map(([first, second]) => {
                this.orderBasics = first;
                this.contract = second;
            }),
            catchError(err => {
                return of(this.router.navigate(['sales/orders', this.orderId]));
            })
        ).subscribe();
    }

}
