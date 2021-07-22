import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { OfferPreviwComponent } from '../../components/offer-preview/offer-preview.component';
import { ActivatedRoute } from '@angular/router';
import {
    IOfferGeneralInformationModel,
    IOfferGeneralCabinetModel,
    IOfferCabinetsModel,
    IOfferAppliancesModel,
    IOfferAccessoriesModel,
    IOfferPricesModel,
    IShippingServiceDetailsModel,
    IServiceDetailsModel,
    IServiceDropdownModel
} from '../../models/offer.models';
import { OfferService } from '../../services/offer.service';
import { IOrderDetailsBasicInfoViewModel } from '../../models/orders';
import { OrdersService } from '../../services/orders.service';
import { map, tap } from 'rxjs/operators';
import { IDropDownViewModel } from '../../../appointments/models/appointments.model';
import { forkJoin } from 'rxjs';

@Component({
    templateUrl: './offer-form.page.component.html',
    styleUrls: ['./offer-form.page.component.scss']
})
export class OfferFormPageComponent implements OnInit {

    isLoading = false;

    isShowAllButtons = true;
    orderId: string;

    generalInfo: IOfferGeneralInformationModel;
    topCabinet: IOfferGeneralCabinetModel;
    baseCabinet: IOfferGeneralCabinetModel;
    tallCabinet: IOfferGeneralCabinetModel;
    topCabinetList: IOfferCabinetsModel[];
    baseCabinetList: IOfferCabinetsModel[];
    tallCabinetList: IOfferCabinetsModel[];
    appliancesList: IOfferAppliancesModel[];
    accessoriesList: IOfferAccessoriesModel[];
    productPrices: IOfferPricesModel;
    shippingService: IShippingServiceDetailsModel;
    assemblyService: IServiceDetailsModel;
    installationService: IServiceDetailsModel;
    isVatRequired: boolean;
    submitted = false;

    orderBasics: IOrderDetailsBasicInfoViewModel;

    distanceDropDown: IServiceDropdownModel[] = [];
    companyDropDown: IDropDownViewModel[] = [];
    previewIsActive = false;

    constructor(
        private orderService: OrdersService,
        private offerService: OfferService,
        private route: ActivatedRoute,
        public dialog: MatDialog
    ) { }

    ngOnInit(): void {
        this.loadData();
    }

    loadData(): void {
        this.isLoading = true;
        this.orderId = this.route.snapshot.paramMap.get('id');
        forkJoin([this.orderService.getOrderBasics(this.orderId), this.offerService.getOfferDetails(this.orderId), this.offerService.getServices()])
            .subscribe(res => {
                this.orderBasics = res[0];
                this.generalInfo = res[1].generalInformation;
                this.topCabinet = res[1].topCabinet;
                this.baseCabinet = res[1].baseCabinet;
                this.tallCabinet = res[1].tallCabinet;
                this.topCabinetList = res[1].topCabinetList;
                this.baseCabinetList = res[1].baseCabinetList;
                this.tallCabinetList = res[1].tallCabinetList;
                this.appliancesList = res[1].appliancesList;
                this.accessoriesList = res[1].accessoriesList;
                this.productPrices = res[1].prices;
                this.shippingService = res[1].shippingService;
                this.assemblyService = res[1].assmeblyService;
                this.installationService = res[1].installationService;
                this.isVatRequired = res[1].isVatRequired;
                this.distanceDropDown = res[2];
                this.isLoading = false;
                if (this.generalInfo !== null
                    && (this.topCabinetList.length !== 0 || this.baseCabinetList.length !== 0 || this.tallCabinetList.length !== 0 || this.appliancesList.length !== 0 || this.accessoriesList.length !== 0)
                ) {
                    this.previewIsActive = true;
                }
            });
    }

    hideOrShowButtons(event: any) {
        if (event.index === 1) {
            this.isShowAllButtons = false;
        } else {
            this.isShowAllButtons = true;
        }
    }

    openPreview() {
        const dialogRef = this.dialog.open(OfferPreviwComponent, {
            panelClass: 'preview-dialog-container',
            data: {
                id: this.orderId
            }
        });

    }

    saveOffer(): void {
        this.submitted = true;
        this.isLoading = true;
        this.offerService.createOffer(this.orderId, this.generalInfo, this.topCabinet, this.baseCabinet, this.tallCabinet)
            .subscribe(() => this.isLoading = false);
    }

    getGeneralInfo(event): void {
        this.generalInfo = event;
    }

    getTopCabinet(event): void {
        this.topCabinet = event;
    }

    getBaseCabinet(event): void {
        this.baseCabinet = event;
    }

    getTallCabinet(event): void {
        this.tallCabinet = event;
    }


    getReload(event): void {
        this.reloadProduct();
    }

    reloadProduct() {
        this.offerService.getOfferDetails(this.orderId).pipe(
            map(res => {
                this.topCabinetList = res.topCabinetList;
                this.baseCabinetList = res.baseCabinetList;
                this.tallCabinetList = res.tallCabinetList;
                this.appliancesList = res.appliancesList;
                this.accessoriesList = res.accessoriesList;
                this.productPrices = res.prices;
                this.shippingService = res.shippingService;
                this.assemblyService = res.assmeblyService;
                this.installationService = res.installationService;
                this.isVatRequired = res.isVatRequired;
            }),
            tap(result => {
                if (
                    this.topCabinetList.length !== 0
                    || this.baseCabinetList.length !== 0
                    || this.tallCabinetList.length !== 0
                    || this.appliancesList.length !== 0
                    || this.accessoriesList.length !== 0
                ) {
                    this.previewIsActive = true;
                } else { this.previewIsActive = false; }
            })
        ).subscribe();
    }
}
