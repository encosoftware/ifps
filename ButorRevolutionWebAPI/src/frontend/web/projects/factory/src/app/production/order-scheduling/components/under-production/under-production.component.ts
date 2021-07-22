import { Component, OnInit, Input, EventEmitter, Output, OnDestroy } from '@angular/core';
import { IProductionStatusDetailsViewModel } from '../../models/order-scheduling.model';
import { OrderSchedulingService } from '../../services/order-scheduling.service';
import { map, tap } from 'rxjs/operators';

@Component({
  selector: 'factory-under-production',
  templateUrl: './under-production.component.html',
  styleUrls: ['./under-production.component.scss']
})
export class UnderProductionComponent implements OnInit, OnDestroy {
  @Input() orderId: string;
  @Output() deleteItem = new EventEmitter();
  cnc: number;
  cutting: number;
  edgbanding: number;
  productionStatus: IProductionStatusDetailsViewModel;
  constructor(
    private orderSchedulingService: OrderSchedulingService
  ) { }

  ngOnInit() {
    this.orderSchedulingService.getProductionStatusByOrderId(this.orderId).subscribe(res => {
      this.cnc = res.cncStatus;
      this.cutting = res.cuttingsStatus;
      this.edgbanding = res.edgebandingStatus;
    });
  }

  ngOnDestroy(): void {
    this.deleteItem.emit(this.orderId);
  }

}
