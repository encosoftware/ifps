import { IPagerModel } from '../../../shared/models/pager.model';
import { OrderStateEnum } from '../../../shared/clients';

export interface IOrdersListViewModel {
    id: string;
    orderId: string;
    workingNumber: string;
    currentStatus: string;
    statusDeadline: Date;
    deadline: Date;
    amount: PriceListModel;
}

export interface IOrdersFilterListViewModel {
    orderId?: string;
    currentStatus?: string;
    workingNr?: string;
    //currentStatus?: OrderStateEnum | string;
    statusDeadline?: Date;
    deadline?: Date;
    amount?: number;
    pager: IPagerModel;
}

export interface ICustomerOrderViewModel {
    currentStatusId: number;
    statusDeadline: Date;
}

export interface ICustomerOrderAppointmentListViewModel {
    id: number;
    date: Date;
    from: string;
    to: string;
    type: string;
    address: string;
    notes?: string;
}

export interface ICustomerOrderMessageViewModel {
    messageList: ICustomerOrderAllMessageListViewModel[];
    assignedPersonList: ICustomerOrderMessageAssignedPeopleViewModel[];
    // currentMessage: IOrderCurrentMessageListViewModel;
}

export interface ICustomerOrderCurrentMessageListViewModel {
    sender: string;
    senderRole: string;
    messages: ICustomerOrderCurrentMessageViewModel[];
}

export interface ICustomerOrderCurrentMessageViewModel {
    type: string;
    text: string;
    date?: Date;
}

export interface IOrderDetailsBasicInfoViewModel {
    orderId: string | undefined;
    currentStatus: string;
    statusDeadline: Date | undefined;
    responsible: string;
    createdOn: Date;
    deadline: Date;
    workingNr: string | undefined;
}

export interface ICustomerOrderAllMessageListViewModel {
    id: number;
    sender: string;
    senderRole: string;
    lastMessage: string;
    date: Date;
}

export interface ICustomerOrderMessageAssignedPeopleViewModel {
    id: number;
    name: string;
    role: string;
}

export class ICustomerOrderHistoryListViewModel {
    detailed?: ICustomerOrderHistoryDetailedViewViewModel[];
    normal?: ICustomerOrderHistoryViewModel[];
}

export interface ICustomerOrderHistoryDetailedViewViewModel {
    date: Date;
    action: string;
    icon?: string;
    status?: string;
}

export interface ICustomerOrderHistoryViewModel {
    status: string;
    doneBy: string;
    closeingDate: Date;
    deadline: Date;
}

export interface IOrderStateListViewModel {
    id: number;
    // state: OrderStateEnum;
    state: string;
    translation: string;
}

// PriceListDto
export interface PriceListModel {
    value?: number;
    currencyId?: number;
    currency?: string;
}
export interface IFinancesOrderViewModel {

    orderId: string | undefined;
    currentStatus: string;
    statusDeadline: Date | undefined;
    responsible: string;
    createdOn: Date;
    deadline: Date;
    workingNr: string | undefined;
    finances: OrderFinanceDetailsModel;
}

export interface OrderFinanceDetailsModel {
    firstPaymentDate: Date;
    firstPaymentBool: boolean;
    secondPaymentDate: Date;
    secondPaymentBool: boolean;
    firstPaymentAmount: PriceListModel;
    secondPaymentAmount: PriceListModel;
}

export interface UserModel {
    id: number;
    name?: string | undefined;
    phoneNumber?: string | undefined;
    email?: string | undefined;
    image?: ImageThumbnailListModel | undefined;
}
export class ImageThumbnailListModel {
    containerName?: string | undefined;
    fileName?: string | undefined;
}

export interface IOrderTicketModel {
    state: string;
    assignedTo: string;
    closedOn: string;
    deadline: string;
}

