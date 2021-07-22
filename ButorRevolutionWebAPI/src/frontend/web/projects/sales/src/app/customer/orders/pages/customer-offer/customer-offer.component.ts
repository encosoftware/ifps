import { Component, OnInit } from '@angular/core';
import { OfferService } from '../../../../sales/orders/services/offer.service';
import {
  IOfferFormPreviewModel,
  IOfferCabinetsModel,
  IOfferShippingInformationModel
} from '../../../../sales/orders/models/offer.models';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'butor-customer-offer',
  templateUrl: './customer-offer.component.html',
  styleUrls: ['./customer-offer.component.scss']
})
export class CustomerOfferComponent implements OnInit {

  isLoading = false;
  orderId: string;
  offerName: string;
  shippingInfo: IOfferShippingInformationModel;
  topCabinetDataSource: IOfferCabinetsModel[];
  baseCabinetDataSource: IOfferCabinetsModel[];
  tallCabinetDataSource: IOfferCabinetsModel[];
  appliancesDataSource: IOfferCabinetsModel[];
  accessoriesDataSource: IOfferCabinetsModel[];
  preview: IOfferFormPreviewModel;

  constructor(
    private service: OfferService,
    private route: ActivatedRoute,

  ) { }

  ngOnInit() {
    this.isLoading = true;
    this.orderId = this.route.snapshot.paramMap.get('id');

    this.service.getOfferFormPreview(this.orderId).subscribe(res => {
      this.preview = res;
      this.isLoading = false;
    });
  }

}
