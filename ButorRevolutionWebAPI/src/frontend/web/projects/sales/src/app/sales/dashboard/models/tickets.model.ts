import { OrderStateEnum } from '../../../shared/clients';

export interface ITicketsModel {
    id: string;
    orderName?: string;
    status: OrderStateEnum;
    customer?: string;
    isExpired: boolean;
}
