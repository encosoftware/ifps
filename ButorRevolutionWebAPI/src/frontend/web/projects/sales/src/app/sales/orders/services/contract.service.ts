import { Injectable } from '@angular/core';
import { ApiOrdersContractClient, ContractDetailsDto, PriceCreateDto, ContractCreateDto } from '../../../shared/clients';
import { Observable } from 'rxjs';
import { IContractViewModel, ICreateContractViewModel } from '../models/orders';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class ContractService {

    constructor(
        private contractClient: ApiOrdersContractClient
    ) { }

    getContract(id: string): Observable<IContractViewModel> {
        return this.contractClient.getContract(id).pipe(map((res: ContractDetailsDto): IContractViewModel => ({
            producer: {
                name: res.producer.name,
                correspondentAddress: res.producer.correspondentAddress.postCode + ' '
                    + res.producer.correspondentAddress.city + ', '
                    + res.producer.correspondentAddress.address,
                tax: res.producer.taxNumber,
                bankAccount: res.producer.bankAccount
            },
            customer: {
                name: res.customer.name,
                correspondentAddress: res.customer.correspondentAddress.postCode + ' '
                    + res.customer.correspondentAddress.city + ' '
                    + res.customer.correspondentAddress.address,
                shippingAddress: res.customer.shippingAddress.postCode + ' '
                    + res.customer.shippingAddress.city + ', '
                    + res.customer.shippingAddress.address
            },
            financial: {
                currency: res.financial.paymentDetails.vat ? res.financial.paymentDetails.vat.currency : null,
                currencyid: res.financial.paymentDetails.vat ? res.financial.paymentDetails.vat.currencyId : null,
                firstPaymentDate: res.financial.firstPaymentDate,
                firstPaymentPrice: res.financial.firstPayment.value,
                secondPaymentDate: res.financial.secondPaymentDate,
                secondPaymentPrice: res.financial.secondPayment.value,
                gross: res.financial.paymentDetails.total ? res.financial.paymentDetails.total.value : null,
                vat: res.financial.paymentDetails.vat ? res.financial.paymentDetails.vat.value : null,
                netCost: res.financial.paymentDetails.products.value ? res.financial.paymentDetails.products.value : null,
                serviceCost: res.financial.paymentDetails.assembly.value +
                    res.financial.paymentDetails.installation.value +
                    res.financial.paymentDetails.shipping.value
            },
            date: res.contractDate,
            additionalPoints: res.additional
        })));
    }

    postContract(id: string, model: ICreateContractViewModel): Observable<void> {
        let dto = new ContractCreateDto({
            firstPayment: new PriceCreateDto({
                currencyId: model.currencyId,
                value: model.firstPayment
            }),
            secondPayment: new PriceCreateDto({
                currencyId: model.currencyId,
                value: model.secondPayment
            }),
            contractDate: model.contractDate,
            firstPaymentDate: new Date(Date.UTC(
                model.firstPaymentDate.getFullYear(),
                model.firstPaymentDate.getMonth(),
                model.firstPaymentDate.getDate())
            ),
            secondPaymentDate: new Date(Date.UTC(
                model.secondPaymentDate.getFullYear(),
                model.secondPaymentDate.getMonth(),
                model.secondPaymentDate.getDate())
            ),
            additional: model.additional
        });
        return this.contractClient.createContract(id, dto);
    }


}
