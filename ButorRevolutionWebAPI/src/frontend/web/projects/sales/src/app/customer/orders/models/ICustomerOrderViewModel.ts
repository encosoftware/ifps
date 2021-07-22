import { OrderStateEnum } from '../../../shared/clients';
import { ICustomerSelectSalesModel } from './customerOrders';

export interface ICustomerOrderViewModel {
    customerId: number;
    orderId: string;
    countryId: number;
    postCode: number;
    city: string;
    address: string;
    salesId: number;
    deadline: Date;
}
export interface IorderSalesHeader {
    orderId?: string | undefined;
    currentStatus?: IorderStateList | string | undefined;
    statusDeadline?: Date;
    responsible?: ICustomerSelectSalesModel | undefined | string;
    customer?: ICustomerSelectSalesModel | undefined | string;
    sales?: ICustomerSelectSalesModel | undefined | string;
    createdOn?: Date;
    deadline?: Date;
}

export class IorderStateList {
    id: number;
    state: OrderStateEnum;
    translation: string | undefined;
}