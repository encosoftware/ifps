import { Injectable, Optional, Inject } from '@angular/core';
import {
  ApiOrdersOffersDetailsClient,
  ApiOrdersOffersClient,
  ApiOrdersOffersPreviewsClient,
  OfferCreateDto,
  RequiresCreateDto,
  CabinetMaterialCreateDto,
  OfferDetailsDto,
  OfferPreviewDto,
  FurnitureUnitListByPreviewDto,
  AccessoryMaterialsListByPreviewDto,
  ApplianceListByPreviewDto,
  ApiFurnitureunitsTallDropdownClient,
  ApiFurnitureunitsBaseDropdownClient,
  FurnitureUnitsForDropdownDto,
  ApiOrdersOffersOrderedfurnitureunitsClient,
  FurnitureUnitCreateByOfferDto,
  ApiAppliancesDropdownClient,
  ApiOrdersOffersAppliancesClient,
  ApplianceCreateByOfferDto,
  ApplianceMaterialsListForDropdownDto,
  FurnitureUnitDetailsByOfferDto,
  FurnitureComponentsDetailsByOfferDto,
  ApplianceDetailsByOfferDto,
  API_BASE_URL,
  ApiBoardsDropdownClient,
  ApiFoilsDropdownClient,
  BoardMaterialsForDropdownDto,
  FoilsForDropdownDto,
  ApiOrdersOffersFurnitureunitsClient,
  FurnitureUnitCreateWithQuantityByOfferDto,
  FurnitureComponentsCreateByOfferDto,
  ApiServicesDropdownClient,
  ServiceListDto,
  ApiOrdersOffersServicesClient,
  ServiceCreateByOfferDto,
  ApplianceUpdateByOfferDto,
  ApiGroupingcategoriesFlatListClient,
  GroupingCategoryEnum,
  GroupingCategoryListDto,
  PriceCreateDto,
  ApiFurnitureunitsTopDropdownClient,
  ServiceTypeEnum,
  ApiServicesShippingClient,
  ApiOrdersOffersVatClient,
} from '../../../shared/clients';
import {
  IOfferGeneralInformationModel,
  IOfferGeneralCabinetModel,
  IOfferDetailsModel,
  IOfferCabinetsModel,
  IOfferAppliancesModel,
  IOfferAccessoriesModel,
  IOfferFormPreviewModel,
  IDecorboardsViewModel,
  IFurnitureUnitDetailsModel,
  IAdditionalUnitsModel,
  IServiceDropdownModel,
} from '../models/offer.models';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { IFurnitureUnitCategoryModel } from '../../../admin/products/models/products.models';

@Injectable({
  providedIn: 'root'
})
export class OfferService {

  constructor(
    private offerDetailsClient: ApiOrdersOffersDetailsClient,
    private offerCreateClient: ApiOrdersOffersClient,
    private offerPreviewClient: ApiOrdersOffersPreviewsClient,
    private topCabinetDropdownClient: ApiFurnitureunitsTopDropdownClient,
    private baseCabinetDropdownClient: ApiFurnitureunitsBaseDropdownClient,
    private tallCabinetDropdownClient: ApiFurnitureunitsTallDropdownClient,
    private appliancesDropdownClient: ApiAppliancesDropdownClient,
    private applianceClient: ApiOrdersOffersAppliancesClient,
    private orderedFurnitureUnitsClient: ApiOrdersOffersOrderedfurnitureunitsClient,
    private appliancesUnitClient: ApiOrdersOffersAppliancesClient,
    private decorboardsDropdownClient: ApiBoardsDropdownClient,
    private foilsDropdownClient: ApiFoilsDropdownClient,
    private editFurnitureUnitClient: ApiOrdersOffersFurnitureunitsClient,
    private shippingDropdownClient: ApiServicesShippingClient,
    private serviceClient: ApiOrdersOffersServicesClient,
    private groupingCategoryClient: ApiGroupingcategoriesFlatListClient,
    private ordersOfferVatClient: ApiOrdersOffersVatClient,
    @Optional() @Inject(API_BASE_URL) private baseUrl?: string
  ) { }

  createOffer(
    orderId: string,
    generalInformation: IOfferGeneralInformationModel,
    topCabinet: IOfferGeneralCabinetModel,
    baseCabinet: IOfferGeneralCabinetModel,
    tallCabinet: IOfferGeneralCabinetModel
  ): Observable<void> {
    const dto = new OfferCreateDto({
      requires: new RequiresCreateDto({
        budget: new PriceCreateDto({
          currencyId: generalInformation.budgetCurrencyId,
          value: generalInformation.budgetPrice
        }),
        description: generalInformation.description,
        isPrivatePerson: generalInformation.privatePerson
      }),
      topCabinet: new CabinetMaterialCreateDto({
        backPanelMaterialId: topCabinet.backPanelMaterialId,
        doorMaterialId: topCabinet.doorMaterialId,
        innerMaterialId: topCabinet.innerMaterialId,
        outerMaterialId: topCabinet.outerMaterialId,
        height: topCabinet.height,
        width: topCabinet.depth,
        description: topCabinet.descrpition,
      }),
      baseCabinet: new CabinetMaterialCreateDto({
        backPanelMaterialId: baseCabinet.backPanelMaterialId,
        doorMaterialId: baseCabinet.doorMaterialId,
        innerMaterialId: baseCabinet.innerMaterialId,
        outerMaterialId: baseCabinet.outerMaterialId,
        height: baseCabinet.height,
        width: baseCabinet.depth,
        description: baseCabinet.descrpition,
      }),
      tallCabinet: new CabinetMaterialCreateDto({
        backPanelMaterialId: tallCabinet.backPanelMaterialId,
        doorMaterialId: tallCabinet.doorMaterialId,
        innerMaterialId: tallCabinet.innerMaterialId,
        outerMaterialId: tallCabinet.outerMaterialId,
        height: tallCabinet.height,
        width: tallCabinet.depth,
        description: tallCabinet.descrpition
      })
    });
    return this.offerCreateClient.createOffer(orderId, dto);
  }

  getOfferDetails(orderId: string): Observable<IOfferDetailsModel> {
    return this.offerDetailsClient.getFurnitureUnitsListByOffer(orderId).pipe(
      map((dto: OfferDetailsDto): IOfferDetailsModel => {
        let retObj: IOfferDetailsModel = {
          generalInformation: {
            budgetCurrencyId: dto.generalInformations.requires.budget.currencyId,
            budgetPrice: dto.generalInformations.requires.budget.value,
            description: dto.generalInformations.requires.description,
            privatePerson: dto.generalInformations.requires.isPrivatePerson
          },
          prices: {
            productsPrice: dto.products.prices.products.value,
            totalCurrency: dto.products.prices.total.currency,
            totalPrice: dto.products.prices.total.value,
            vatPrice: dto.products.prices.vat.value,
            installationPrice: dto.products.prices.installation.value,
            installationCurrency: dto.products.prices.installation.currency
          },
          isVatRequired: dto.products.prices.isVatRequired
        };

        if (dto.products.prices.assembly !== undefined) {
          retObj.prices.assemblyCurrency = dto.products.prices.assembly.currency;
          retObj.prices.assemblyPrice = dto.products.prices.assembly.value;
        } else {
          retObj.prices.assemblyCurrency = '';
          retObj.prices.assemblyPrice = 0;
        }

        if (dto.products.prices.products !== undefined) {
          retObj.prices.productsCurrency = dto.products.prices.products.currency;
          retObj.prices.productsPrice = dto.products.prices.products.value;
        } else {
          retObj.prices.productsPrice = 0;
          retObj.prices.productsCurrency = '';
        }

        if (dto.products.prices.installation !== undefined) {
          retObj.prices.installationCurrency = dto.products.prices.installation.currency;
          retObj.prices.installationPrice = dto.products.prices.installation.value;
        }

        if (dto.products.prices.shipping !== undefined) {
          retObj.prices.shippingCurrency = dto.products.prices.shipping.currency;
          retObj.prices.shippingPrice = dto.products.prices.shipping.value;
        } else {
          retObj.prices.shippingCurrency = '';
          retObj.prices.shippingPrice = 0;
        }

        if (dto.products.prices.vat !== undefined) {
          retObj.prices.vatCurrency = dto.products.prices.vat.currency;
          retObj.prices.vatPrice = dto.products.prices.vat.value;
        } else {
          retObj.prices.vatCurrency = '';
          retObj.prices.vatPrice = 0;
        }

        if (dto.generalInformations.baseCabinet !== undefined) {
          retObj.baseCabinet = {
            backPanelMaterialId: dto.generalInformations.baseCabinet.backMaterialId,
            descrpition: dto.generalInformations.baseCabinet.description,
            doorMaterialId: dto.generalInformations.baseCabinet.doorMaterialId,
            height: dto.generalInformations.baseCabinet.height,
            innerMaterialId: dto.generalInformations.baseCabinet.innerMaterialId,
            outerMaterialId: dto.generalInformations.baseCabinet.outerMaterialId,
            depth: dto.generalInformations.baseCabinet.depth
          };
        } else {
          retObj.baseCabinet = {
            backPanelMaterialId: null,
            descrpition: null,
            doorMaterialId: null,
            height: null,
            innerMaterialId: null,
            outerMaterialId: null,
            depth: null
          };
        }

        if (dto.generalInformations.topCabinet !== undefined) {
          retObj.topCabinet = {
            backPanelMaterialId: dto.generalInformations.topCabinet.backMaterialId,
            depth: dto.generalInformations.topCabinet.depth,
            descrpition: dto.generalInformations.topCabinet.description,
            doorMaterialId: dto.generalInformations.topCabinet.doorMaterialId,
            height: dto.generalInformations.topCabinet.height,
            innerMaterialId: dto.generalInformations.topCabinet.innerMaterialId,
            outerMaterialId: dto.generalInformations.topCabinet.outerMaterialId
          };
        } else {
          retObj.topCabinet = {
            backPanelMaterialId: null,
            depth: null,
            descrpition: null,
            doorMaterialId: null,
            height: null,
            innerMaterialId: null,
            outerMaterialId: null
          };
        }

        if (dto.generalInformations.tallCabinet !== undefined) {
          retObj.tallCabinet = {
            backPanelMaterialId: dto.generalInformations.tallCabinet.backMaterialId,
            depth: dto.generalInformations.tallCabinet.depth,
            descrpition: dto.generalInformations.tallCabinet.description,
            doorMaterialId: dto.generalInformations.tallCabinet.doorMaterialId,
            height: dto.generalInformations.tallCabinet.height,
            innerMaterialId: dto.generalInformations.tallCabinet.innerMaterialId,
            outerMaterialId: dto.generalInformations.tallCabinet.outerMaterialId
          };
        } else {
          retObj.tallCabinet = {
            backPanelMaterialId: null,
            depth: null,
            descrpition: null,
            doorMaterialId: null,
            height: null,
            innerMaterialId: null,
            outerMaterialId: null
          };
        }

        retObj.topCabinetList = dto.products.topCabinets.map((x): IOfferCabinetsModel => ({
          code: x.code,
          depth: x.depth,
          height: x.height,
          containerName: x.image.containerName,
          fileName: x.image.fileName,
          src: this.baseUrl + '/api/images?containerName=' + x.image.containerName + '&fileName=' + x.image.fileName,
          name: x.name,
          price: x.currentPrice.value,
          priceCurrency: x.currentPrice.currency,
          quantity: x.quantity,
          subtotal: x.subTotal.value,
          subtotalCurrency: x.subTotal.currency,
          width: x.width,
          id: x.orderedFurnitureUnitId,
          unitId: x.furnitureUnitId
        }));

        retObj.baseCabinetList = dto.products.baseCabinets.map((x): IOfferCabinetsModel => ({
          code: x.code,
          depth: x.depth,
          height: x.height,
          containerName: x.image.containerName,
          fileName: x.image.fileName,
          src: this.baseUrl + '/api/images?containerName=' + x.image.containerName + '&fileName=' + x.image.fileName,
          name: x.name,
          price: x.currentPrice.value,
          priceCurrency: x.currentPrice.currency,
          quantity: x.quantity,
          subtotal: x.subTotal.value,
          subtotalCurrency: x.subTotal.currency,
          width: x.width,
          id: x.orderedFurnitureUnitId,
          unitId: x.furnitureUnitId
        }));

        retObj.tallCabinetList = dto.products.tallCabinets.map((x): IOfferCabinetsModel => ({
          code: x.code,
          depth: x.depth,
          height: x.height,
          containerName: x.image.containerName,
          fileName: x.image.fileName,
          src: this.baseUrl + '/api/images?containerName=' + x.image.containerName + '&fileName=' + x.image.fileName,
          name: x.name,
          price: x.currentPrice.value,
          priceCurrency: x.currentPrice.currency,
          quantity: x.quantity,
          subtotal: x.subTotal.value,
          subtotalCurrency: x.subTotal.currency,
          width: x.width,
          id: x.orderedFurnitureUnitId,
          unitId: x.furnitureUnitId
        }));

        retObj.appliancesList = dto.products.appliances.map((x): IOfferAppliancesModel => ({
          code: x.code,
          containerName: x.image.containerName,
          fileName: x.image.fileName,
          src: this.baseUrl + '/api/images?containerName=' + x.image.containerName + '&fileName=' + x.image.fileName,
          name: x.name,
          price: x.price.value,
          priceCurrency: x.price.currency,
          quantity: x.quantity,
          subtotal: x.subTotal.value,
          subtotalCurrency: x.subTotal.currency,
          id: x.orderedApplianceMaterialId,
          unitId: x.applianceMaterialId
        }));

        retObj.accessoriesList = dto.products.accessories.map((x): IOfferAccessoriesModel => ({
          code: x.code,
          containerName: x.image.containerName,
          fileName: x.image.fileName,
          src: this.baseUrl + '/api/images?containerName=' + x.image.containerName + '&fileName=' + x.image.fileName,
          name: x.name,
          price: x.price.value,
          priceCurrency: x.price.currency,
          quantity: x.quantity,
          subtotal: x.subTotal.value,
          subtotalCurrency: x.subTotal.currency,
        }));

        if (dto.products.shippingService !== undefined) {
          retObj.shippingService = {
            isChecked: true,
            basicFeeCurrency: dto.products.shippingService.basicFee.currency,
            basicFeePrice: dto.products.shippingService.basicFee.value,
            description: dto.products.shippingService.description,
            distanceServiceId: dto.products.shippingService.distanceServiceId,
            totalCurrency: dto.products.shippingService.total.currency,
            totalPrice: dto.products.shippingService.total.value
          };
        } else {
          retObj.shippingService = {
            isChecked: false,
            basicFeeCurrency: null,
            basicFeePrice: null,
            description: null,
            distanceServiceId: null,
            totalCurrency: null,
            totalPrice: null
          };
        }

        if (dto.products.assemblyService !== undefined) {
          retObj.assmeblyService = {
            isChecked: true,
            description: dto.products.assemblyService.description,
            serviceId: dto.products.assemblyService.assemblyServiceId
          };
        } else {
          retObj.assmeblyService = {
            isChecked: false,
            description: null,
            serviceId: null
          };
        }

        if (dto.products.installationService !== undefined) {
          retObj.installationService = {
            isChecked: true,
            description: dto.products.installationService.description,
            serviceId: dto.products.installationService.installationServiceId,
            basicFeeCurrency: dto.products.installationService.installationBasicFee.currency,
            basicFeePrice: dto.products.installationService.installationBasicFee.value
          };
        } else {
          retObj.installationService = {
            isChecked: false,
            description: null,
            serviceId: null,
            basicFeeCurrency: '',
            basicFeePrice: null
          };
        }
        return retObj;
      })
    );
  }

  getOfferFormPreview(id: string): Observable<IOfferFormPreviewModel> {
    return this.offerPreviewClient.offerPreview(id).pipe(
      map((dto: OfferPreviewDto): IOfferFormPreviewModel => {
        let retObj: IOfferFormPreviewModel = {
          offerName: dto.offerName,
          renderers: dto.renderers ? dto.renderers.map(res => ({
            containerName: res.containerName,
            fileName: res.fileName,
            src: this.baseUrl + '/api/images?containerName=' + res.containerName + '&fileName=' + res.fileName
          })) : [],
          shippingInfo: {
            producerName: dto.producer.companyName,
            headquarter: dto.producer.address.postCode + ' ' + dto.producer.address.city + ' ' + dto.producer.address.address,
            contactPerson: dto.producer.contactPersonName,
            producerEmail: dto.producer.email,
            producerPhone: dto.producer.phone,
            customerName: dto.customer.name,
            shippingAddress: dto.customer.shippingAddress.postCode +
              ' ' + dto.customer.shippingAddress.city +
              ' ' + dto.customer.shippingAddress.address,
            customerEmail: dto.customer.email,
            customerPhone: dto.customer.phone,
          },
          topCabinetList: dto.topCabinets ? dto.topCabinets.map((x: FurnitureUnitListByPreviewDto): IOfferCabinetsModel => ({
            code: x.code,
            depth: x.depth,
            height: x.height,
            containerName: x.image.containerName,
            fileName: x.image.fileName,
            src: this.baseUrl + '/api/images?containerName=' + x.image.containerName + '&fileName=' + x.image.fileName,
            name: x.name,
            price: x.currentPrice.value,
            priceCurrency: x.currentPrice.currency,
            quantity: x.quantity,
            subtotal: x.subTotal.value,
            subtotalCurrency: x.subTotal.currency,
            width: x.width,
          })) : [],
          baseCabinetList: dto.baseCabinets.map((x: FurnitureUnitListByPreviewDto): IOfferCabinetsModel => ({
            code: x.code,
            depth: x.depth,
            height: x.height,
            containerName: x.image.containerName,
            fileName: x.image.fileName,
            src: this.baseUrl + '/api/images?containerName=' + x.image.containerName + '&fileName=' + x.image.fileName,
            name: x.name,
            price: x.currentPrice.value,
            priceCurrency: x.currentPrice.currency,
            quantity: x.quantity,
            subtotal: x.subTotal.value,
            subtotalCurrency: x.subTotal.currency,
            width: x.width
          })),
          tallCabinetList: dto.tallCabinets ? dto.tallCabinets.map((x: FurnitureUnitListByPreviewDto): IOfferCabinetsModel => ({
            code: x.code,
            depth: x.depth,
            height: x.height,
            containerName: x.image.containerName,
            fileName: x.image.fileName,
            src: this.baseUrl + '/api/images?containerName=' + x.image.containerName + '&fileName=' + x.image.fileName,
            name: x.name,
            price: x.currentPrice.value,
            priceCurrency: x.currentPrice.currency,
            quantity: x.quantity,
            subtotal: x.subTotal.value,
            subtotalCurrency: x.subTotal.currency,
            width: x.width
          })) : [],
          accessoriesList: dto.accessories ? dto.accessories.map((x: AccessoryMaterialsListByPreviewDto): IOfferAccessoriesModel => ({
            code: x.code,
            containerName: x.image.containerName,
            fileName: x.image.fileName,
            src: this.baseUrl + '/api/images?containerName=' + x.image.containerName + '&fileName=' + x.image.fileName,
            name: x.name,
            price: x.price.value,
            priceCurrency: x.price.currency,
            quantity: x.quantity,
            subtotal: x.subTotal.value,
            subtotalCurrency: x.subTotal.currency
          })) : [],
          appliancesList: dto.appliances ? dto.appliances.map((x: ApplianceListByPreviewDto): IOfferAppliancesModel => ({
            code: x.code,
            containerName: x.image.containerName,
            fileName: x.image.fileName,
            src: this.baseUrl + '/api/images?containerName=' + x.image.containerName + '&fileName=' + x.image.fileName,
            name: x.name,
            price: x.price.value,
            priceCurrency: x.price.currency,
            quantity: x.quantity,
            subtotal: x.subTotal.value,
            subtotalCurrency: x.subTotal.currency
          })) : [],
          prices: {
            productsCurrency: dto.prices.products ? dto.prices.products.currency : undefined,
            productsPrice: dto.prices.products ? dto.prices.products.value : undefined,
            totalCurrency: dto.prices.total ? dto.prices.total.currency : undefined,
            totalPrice: dto.prices.total ? dto.prices.total.value : undefined,
            vatCurrency: dto.prices.vat ? dto.prices.vat.currency : undefined,
            vatPrice: dto.prices.vat ? dto.prices.vat.value : undefined,
            installationCurrency: dto.prices.installation ? dto.prices.installation.currency : undefined,
            installationPrice: dto.prices.installation ? dto.prices.installation.value : undefined
          }
        };

        if (dto.prices.assembly !== undefined) {
          retObj.prices.assemblyCurrency = dto.prices.assembly.currency;
          retObj.prices.assemblyPrice = dto.prices.assembly.value;
        } else {
          retObj.prices.assemblyCurrency = '';
          retObj.prices.assemblyPrice = 0;
        }

        if (dto.prices.shipping !== undefined) {
          retObj.prices.shippingCurrency = dto.prices.shipping.currency;
          retObj.prices.shippingPrice = dto.prices.shipping.value;
        } else {
          retObj.prices.shippingCurrency = '';
          retObj.prices.shippingPrice = 0;
        }
        return retObj;
      })
    );
  }

  getTopCabinets(): Observable<IDecorboardsViewModel[]> {
    return this.topCabinetDropdownClient.getTopFurnitureUnits().pipe(
      map((dto: FurnitureUnitsForDropdownDto[]): IDecorboardsViewModel[] => {
        return dto.map((x: FurnitureUnitsForDropdownDto): IDecorboardsViewModel => ({
          id: x.id,
          code: x.code
        }));
      })
    );
  }

  getBaseCabinets(): Observable<IDecorboardsViewModel[]> {
    return this.baseCabinetDropdownClient.getBaseFurnitureUnits().pipe(
      map((dto: FurnitureUnitsForDropdownDto[]): IDecorboardsViewModel[] => {
        return dto.map((x: FurnitureUnitsForDropdownDto): IDecorboardsViewModel => ({
          id: x.id,
          code: x.code
        }));
      })
    );
  }

  getTallCabinets(): Observable<IDecorboardsViewModel[]> {
    return this.tallCabinetDropdownClient.getTallFurnitureUnits().pipe(
      map((dto: FurnitureUnitsForDropdownDto[]): IDecorboardsViewModel[] => {
        return dto.map((x: FurnitureUnitsForDropdownDto): IDecorboardsViewModel => ({
          id: x.id,
          code: x.code
        }));
      })
    );
  }

  getAppliances(): Observable<IDecorboardsViewModel[]> {
    return this.appliancesDropdownClient.getApplianceMaterilasForDropdown().pipe(
      map((dto: ApplianceMaterialsListForDropdownDto[]): IDecorboardsViewModel[] => {
        return dto.map((x: ApplianceMaterialsListForDropdownDto): IDecorboardsViewModel => ({
          id: x.id,
          code: x.name
        }));
      })
    );
  }

  addFurnitureUnit(orderId: string, quantity: number, furnitureUnitId: string): Observable<void> {
    const dto = new FurnitureUnitCreateByOfferDto({
      furnitureUnitId,
      quantity
    });
    return this.orderedFurnitureUnitsClient.addFurnitureUnit(orderId, dto);
  }

  addApliance(orderId: string, quantity: number, applianceMaterialId: string): Observable<void> {
    const dto = new ApplianceCreateByOfferDto({
      applianceMaterialId,
      quantity
    });
    return this.applianceClient.addAppliance(orderId, dto);
  }

  deleteFurnitureUnit(orderId: string, furnitureUnitId: string): Observable<void> {
    return this.orderedFurnitureUnitsClient.deleteOrderedFurnitureUnits(orderId, furnitureUnitId);
  }

  deleteAppliance(orderId: string, appliancMaterialid: number): Observable<void> {
    return this.applianceClient.deleteApplianceFromAplliancesList(orderId, appliancMaterialid);
  }

  putAppliances(orderId: string, applianceMaterialId: number, quantity: number): Observable<void> {
    const dto = new ApplianceUpdateByOfferDto({
      quantity
    });
    return this.appliancesUnitClient.updateAppliance(orderId, applianceMaterialId, dto);
  }

  getFurnitureUnit(orderId: string, id: number): Observable<IFurnitureUnitDetailsModel> {
    return this.orderedFurnitureUnitsClient.getOrderedFurnitureUnit(orderId, id).pipe(
      map((dto: FurnitureUnitDetailsByOfferDto): IFurnitureUnitDetailsModel => ({
        code: dto.code,
        depth: dto.depth,
        description: dto.description,
        height: dto.height,
        width: dto.width,
        category: dto.category.name,
        categoryId: dto.category.id,
        src:
          this.baseUrl
          + '/api/images?containerName='
          + dto.image.containerName
          + '&fileName='
          + dto.image.fileName,
        id,
        quiantity: dto.quantity,
        totalPrice: dto.total.value,
        totalCurrency: dto.total.currency,
        fronts: dto.fronts.map((x: FurnitureComponentsDetailsByOfferDto): IAdditionalUnitsModel => ({
          amount: x.amount,
          boardMaterialId: x.boardMaterialId,
          containerName: x.image.containerName,
          fileName: x.image.fileName,
          height: x.height,
          id: x.id,
          bottomFoilId: x.bottomFoilId,
          leftFoilId: x.leftFoilId,
          rightFoilId: x.rightFoilId,
          topFoilId: x.topFoilId,
          name: x.name,
          bottomFoil: x.bottomFoilName,
          leftFoil: x.leftFoilName,
          rightFoil: x.rightFoilName,
          topFoil: x.topFoilName,
          width: x.width
        })),
        corpuses: dto.corpuses.map((x: FurnitureComponentsDetailsByOfferDto): IAdditionalUnitsModel => ({
          amount: x.amount,
          boardMaterialId: x.boardMaterialId,
          bottomFoilId: x.bottomFoilId,
          containerName: x.image.containerName,
          fileName: x.image.fileName,
          height: x.height,
          id: x.id,
          leftFoilId: x.leftFoilId,
          name: x.name,
          rightFoilId: x.rightFoilId,
          topFoilId: x.topFoilId,
          bottomFoil: x.bottomFoilName,
          leftFoil: x.leftFoilName,
          rightFoil: x.rightFoilName,
          topFoil: x.topFoilName,
          width: x.width
        }))
      }))
    );
  }

  editFunitureUnit(orderId: string, furnitureUnit: IFurnitureUnitDetailsModel, unitId: string): Observable<string> {
    const dto = new FurnitureUnitCreateWithQuantityByOfferDto({
      code: furnitureUnit.code,
      depth: furnitureUnit.depth,
      description: furnitureUnit.description,
      height: furnitureUnit.height,
      quantity: furnitureUnit.quiantity,
      width: furnitureUnit.width,
      categoryId: furnitureUnit.categoryId,
      corpuses: furnitureUnit.corpuses.map((x: IAdditionalUnitsModel): FurnitureComponentsCreateByOfferDto => {
        return new FurnitureComponentsCreateByOfferDto({
          amount: x.amount,
          boardMaterialId: x.boardMaterialId,
          bottomFoilId: x.bottomFoilId,
          height: x.height,
          id: x.id,
          leftFoilId: x.leftFoilId,
          name: x.name,
          rightFoilId: x.rightFoilId,
          topFoilId: x.topFoilId,
          width: x.width
        });
      }),
      fronts: furnitureUnit.fronts.map((x: IAdditionalUnitsModel): FurnitureComponentsCreateByOfferDto => {
        return new FurnitureComponentsCreateByOfferDto({
          amount: x.amount,
          boardMaterialId: x.boardMaterialId,
          bottomFoilId: x.bottomFoilId,
          height: x.height,
          id: x.id,
          leftFoilId: x.leftFoilId,
          name: x.name,
          rightFoilId: x.rightFoilId,
          topFoilId: x.topFoilId,
          width: x.width
        });
      })
    });
    return this.editFurnitureUnitClient.createWallCabinetByOffer(orderId, unitId, dto);
  }

  getDecorboardsDropdown(): Observable<IDecorboardsViewModel[]> {
    return this.decorboardsDropdownClient.getBoardMaterials().pipe(
      map((dto: BoardMaterialsForDropdownDto[]): IDecorboardsViewModel[] => {
        return dto.map((x: BoardMaterialsForDropdownDto): IDecorboardsViewModel => ({
          id: x.id,
          code: x.name
        }));
      })
    );
  }

  getFoilsDropdown(): Observable<IDecorboardsViewModel[]> {
    return this.foilsDropdownClient.getFoilsForDropdown().pipe(
      map((dto: FoilsForDropdownDto[]): IDecorboardsViewModel[] => {
        return dto.map((x: FoilsForDropdownDto): IDecorboardsViewModel => ({
          id: x.id,
          code: x.code
        }));
      })
    );
  }

  getApplianceUnit(orderId: string, id: number): Observable<number> {
    return this.appliancesUnitClient.getAppliance(orderId, id).pipe(
      map((dto: ApplianceDetailsByOfferDto): number => {
        return dto.quantity;
      })
    );
  }

  getServices(): Observable<IServiceDropdownModel[]> {
    return this.shippingDropdownClient.getShippingServicesForDropdown().pipe(
      map((dto: ServiceListDto[]): IServiceDropdownModel[] => {
        return dto.map((x: ServiceListDto): IServiceDropdownModel => ({
          id: x.id,
          description: x.description + ' (' + x.price.value.toString() + ' ' + x.price.currency + ')',
          price: x.price.value,
          currency: x.price.currency,
          type: x.type
        }));
      })
    );
  }

  getFurnitureUnitCategories(): Observable<IFurnitureUnitCategoryModel[]> {
    return this.groupingCategoryClient.getRootCategories(GroupingCategoryEnum.FurnitureUnitType, undefined).pipe(
      map((dto: GroupingCategoryListDto[]): IFurnitureUnitCategoryModel[] => {
        return dto.map((x: GroupingCategoryListDto): IFurnitureUnitCategoryModel => ({
          id: x.id,
          name: x.name
        }));
      })
    );
  }

  checkService(orderId: string, checkboxValue: boolean, type: ServiceTypeEnum, priceId?: number): Observable<void> {
    const dto = new ServiceCreateByOfferDto({
      isAdded: checkboxValue,
      serviceType: type,
      shippingServicePriceId: priceId
    });
    return this.serviceClient.addService(orderId, dto);
  }

  setVat(orderId: string, isVat: boolean): Observable<void> {
    return this.ordersOfferVatClient.setVat(orderId, isVat);
  }
}
