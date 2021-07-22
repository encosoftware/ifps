import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { Location } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { EditOrderComponent } from '../edit-order/edit-order.component';
import { IOrderDetailsBasicInfoViewModel } from '../../models/orders';
import { OrdersService } from '../../services/orders.service';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../../../core/store/selectors/core.selector';
import { take } from 'rxjs/operators';
import { ClaimPolicyEnum } from '../../../../shared/clients';

@Component({
    selector: 'butor-order-header',
    templateUrl: './order-header.component.html',
    styleUrls: ['./order-header.component.scss']
})
export class OrderHeaderComponent implements OnInit {

    @Output() reload = new EventEmitter();
    @Input() basicInfo: IOrderDetailsBasicInfoViewModel;
    dataSource = [];
    userCompany: string;
    claimPolicyEnum = ClaimPolicyEnum;
    constructor(
        public dialog: MatDialog,
        private orderService: OrdersService,
        public route: ActivatedRoute,
        private store: Store<any>,
        private location: Location
    ) { }

    ngOnInit(): void {
        this.dataSource.push(this.basicInfo);
        this.store.pipe(
            select(coreLoginT),
            take(1)
        ).subscribe(res => this.userCompany = res.CompanyId);

    }

    openEditOrder() {
        forkJoin([
            this.orderService.getOrder(this.route.snapshot.paramMap.get('id')),
            this.orderService.getCountries(),
            this.orderService.getOrderStates(),
            this.orderService.getSalesPersons()
        ]).subscribe(([data, country, status, sale]) => {
            const dialogRef = this.dialog.open(EditOrderComponent, {
                width: '52rem',
                data: {
                    order: data,
                    countries: country,
                    statuses: status,
                    sales: sale
                }
            });

            dialogRef.afterClosed().subscribe(result => {
                if (result !== undefined) {
                    this.orderService.putOrder(this.route.snapshot.paramMap.get('id'), result).subscribe(() => this.reload.emit('reload'));
                }
            });
        });
    }
    backTo() {
        this.location.back();
    }
}
