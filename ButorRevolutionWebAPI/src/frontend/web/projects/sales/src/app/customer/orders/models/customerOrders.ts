import { OrderStateEnum, DocumentStateEnum, FileExtensionTypeEnum } from '../../../shared/clients';
import { IPagerModel } from '../../../shared/models/pager.model';

export interface ICustomerOrdersListViewModel {
    id: string;
    orderId: string;
    workingNumber: string;
    currentStatus: OrderStateEnum | string;
    statusDeadline: Date | string;
    responsibleName: string;
    customerName: string;
    salesName: string;
    createdOnFrom: Date | string;
    deadline: Date | string;
}

export interface ICustomerOrdersFilterListViewModel {
    id?: string;
    orderId?: string;
    workingNumber?: string;
    currentStatus?: OrderStateEnum | number;
    statusDeadline?: Date;
    responsibleName?: string;
    customerName?: string;
    salesName?: string;
    createdOnFrom?: Date;
    deadline?: Date;
    pager: IPagerModel;
}

export interface ICustomerOrderAppointmentListViewModel {
    id: number;
    date: Date;
    from: string;
    to: string;
    type: string;
    customer: string;
    address: string;
    notes?: string;
}

export interface ICustomerOrderMessageViewModel {
    messageList: ICustomerOrderAllMessageListViewModel[];
    assignedPersonList: ICustomerOrderMessageAssignedPeopleViewModel[];
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

export interface ICustomerOrderStateViewModel {
    id: number;
    state: OrderStateEnum;
    translation: string;
}

export interface ICustomerDeadlineFilterModel {
    name: string;
    toDate: string | Date;
}

export interface ICustomerSelectSalesModel {
    id: number;
    name?: string;
}

export interface IDocumentGroupModel {
    documentGroupId: number;
    isVersionable: boolean;
    folderId: number;
    folderName?: string | undefined;
    versions?: IDocumentGroupVersionModel[] | undefined;
}

export interface IDocumentGroupVersionModel {
    id: number;
    lastModifiedOn?: Date | undefined;
    state: DocumentStateEnum;
    documents?: IDocumentListModel[] | undefined;
}

export interface IDocumentListModel {
    id: number;
    fileExtensionType: FileExtensionTypeEnum;
    displayName?: string | undefined;
}

export interface IDocumentViewGroupModel {
    versionable: IDocumentGroupModel[];
    documents: IDocumentGroupModel[];
}
