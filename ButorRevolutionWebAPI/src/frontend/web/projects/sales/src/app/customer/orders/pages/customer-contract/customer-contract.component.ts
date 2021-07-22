import { Component, OnInit } from '@angular/core';
import { IContractViewModel } from '../../../../sales/orders/models/orders';
import { ContractService } from '../../../../sales/orders/services/contract.service';

@Component({
  selector: 'butor-customer-contract',
  templateUrl: './customer-contract.component.html',
  styleUrls: ['./customer-contract.component.scss']
})
export class CustomerContractComponent implements OnInit {

  contract: IContractViewModel;
  isLoading = true;
  constructor(
    private orderService: ContractService
  ) { }

  ngOnInit(): void {

    // TODO kijavítani az id-t
    /*this.orderService.getContract().subscribe(res => {
      this.contract = res;
      this.isLoading = !this.isLoading;
    });*/
  }


}
