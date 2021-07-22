import { IPagerModel } from '../../../shared/models/pager.model';
import {
    OrderStateEnum,
    FileExtensionTypeEnum,
    DocumentStateEnum,
    GroupingCategoryEnum,
    DocumentTypeEnum,
    DocumentGroupDto,
    DocumentFolderTypeEnum
} from '../../../shared/clients';
import { IOrderTicketModel } from './ticket.model';

export interface IOrdersListViewModel {
    id: string;
    orderId: string;
    workingNr: string;
    currentStatus: string;
    statusDeadline: Date;
    responsible: string;
    customer: string;
    sales: string;
    created: Date;
    deadline: Date;
}

export interface IOrdersFilterListViewModel {
    orderId?: string;
    workingNr?: string;
    currentStatus?: OrderStateEnum;
    statusDeadline?: Date;
    responsible?: string;
    customer?: string;
    sales?: string;
    created?: Date;
    deadline?: Date;
    pager: IPagerModel;
}

export interface INewOrderViewModel {
    customerId: number;
    orderName: string;
    countryId: number;
    postCode: number;
    city: string;
    address: string;
    salesId: number;
    deadline: Date;
}

export interface IEditOrderViewModel extends INewOrderViewModel {
    orderId: string;
    currentStatusId: number;
    statusDeadline: Date;
    responsibleId: number;
    customerName?: string;
}

export interface IOrderDetailsViewModel {
    basics: IOrderDetailsBasicInfoViewModel;
    appointments: IOrderAppointmentListViewModel[];
    messages: IOrderMessageViewModel;
    history: IOrderHistoryListViewModel;
    documents?: IOrderDocumentViewModel;
}

export interface ICountriesListViewModel {
    id: number;
    code: string;
    translation: string;
}

export interface IOrderDetailsBasicInfoViewModel {
    orderId: string;
    orderName: string;
    currentStatus: string;
    currentStatusState: OrderStateEnum;
    statusDeadline: Date;
    responsible: string;
    customer: string;
    sales: string;
    createdOn: Date;
    deadline: Date;
    divisions?: any[];
}

export interface IOrderAppointmentListViewModel {
    id: number;
    date: Date;
    from: string;
    to: string;
    type: string;
    customer: string;
    address: string;
    notes?: string;
}

export interface IOrderNewAppointmentViewModel {
    typeId: number;
    date: Date;
    from: string;
    to: string;
    customerId: number;
    participantIds: number[];
    venueId: number;
    meetingRoomId: number;
    notes: string;
}

export class IOrderHistoryListViewModel {
    detailed?: IOrderHistoryDetailedViewViewModel[];
    normal?: IOrderTicketModel[];
}

export interface IOrderHistoryDetailedViewViewModel {
    date: Date;
    action: string;
    icon?: string;
    status?: string;
}

export interface IOrderMessageViewModel {
    messageList: IOrderAllMessageListViewModel[];
    assignedPersonList: IOrderMessageAssignedPeopleViewModel[];
}

export interface IOrderCurrentMessageViewModel {
    text: string;
    sent?: Date;
    senderId: number;
}

export interface IOrderAllMessageListViewModel {
    id: number;
    sender: string;
    senderRole: string;
    lastMessage: string;
    date: Date;
}

export interface IOrderMessageAssignedPeopleViewModel {
    id: number;
    name: string;
    role: string;
}

export interface IOrderDocumentViewModel {
    versionable: IOrderDocumentFolderListViewModel[];
    documents: IOrderDocumentFolderListViewModel[];
}

export interface IOrderDocumentFolderListViewModel {
    documentGroupId: number;
    documentGroupDto: DocumentGroupDto;
    folderId: number;
    name: string;
    status: DocumentStateEnum;
    translation: string;
    type?: DocumentFolderTypeEnum;
    versions: IOrderDocumentListViewModel[];
    fileCount: number;
}

export interface IOrderDocumentListViewModel {
    versionId?: number;
    date: Date;
    status: DocumentStateEnum;
    translation: string;
    documents: IOrderDocumentVersionViewModel[];
}

export interface IOrderDocumentVersionViewModel {
    id: string;
    type: FileExtensionTypeEnum;
    name?: string;
    src?: string;
}

export interface IContractViewModel {
    producer: IContractProducerViewModel;
    customer: IContractCustomerViewModel;
    financial: IContractFinancialViewModel;
    additionalPoints: string;
    date: Date;
}

export interface IContractProducerViewModel {
    name: string;
    correspondentAddress: string;
    tax: string;
    bankAccount: string;
}

export interface IContractCustomerViewModel {
    name: string;
    correspondentAddress: string;
    shippingAddress: string;
}

export interface IContractFinancialViewModel {
    currency?: string;
    currencyid?: number;
    netCost: number;
    vat: number;
    gross: number;
    firstPaymentPrice: number;
    firstPaymentDate: Date;
    secondPaymentPrice: number;
    secondPaymentDate: Date;
    serviceCost: number;
}

export interface IFolderDropDownViewModel {
    folderId: number;
    folderName: string;
    supportedTypes: ISupportedTypeViewModel[];
}

export interface ISupportedTypeViewModel {
    typeId: number;
    translation: string;
}

export interface IOrderStateListViewModel {
    id: number;
    state: OrderStateEnum;
    translation: string;
}

export interface IOrderUploadedDocumentItem {
    fileName: string;
    containerName: string;
    originalFileName: string;
}

export interface IOrderDocumentCreateViewModel {
    folderId: number;
    versionId: number;
    typeId: number;
    documents: IOrderDocumentCreateItem[];
}

export interface IOrderDocumentCreateItem {
    fileName: string;
    containerName: string;
}

export interface IUsersListModel {
    id: number;
    name?: string;
}

export interface ICreateContractViewModel {
    currencyId: number;
    firstPayment: number;
    firstPaymentDate: Date;
    secondPayment: number;
    secondPaymentDate: Date;
    additional: string;
    contractDate: Date;
}

export interface DocumentTypeModel {
    id: number;
    documentType: DocumentTypeEnum;
    translation?: string | undefined;
}

export interface DocumentFolderTypeModel {
    folderId: number;
    folderName?: string | undefined;
    supportedTypes?: DocumentTypeModel[] | undefined;
}

export interface GroupingCategoryListViewModel {
    id: number;
    name?: string | undefined;
}
