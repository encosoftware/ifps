import { Injectable } from '@angular/core';
import {
    IOrdersListViewModel,
    IOrdersFilterListViewModel,
    INewOrderViewModel,
    IEditOrderViewModel,
    ICountriesListViewModel,
    IOrderStateListViewModel,
    IOrderAppointmentListViewModel,
    IOrderDocumentViewModel,
    IOrderDetailsBasicInfoViewModel,
    IFolderDropDownViewModel,
    IOrderDocumentCreateViewModel,
    IUsersListModel,
    GroupingCategoryListViewModel
} from '../models/orders';
import { PagedData } from '../../../shared/models/paged-data.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {
    ApiOrdersClient,
    PagedListDtoOfOrderListDto,
    OrderCreateDto,
    AddressCreateDto,
    CountryListDto,
    ApiCountriesClient,
    ApiOrderstatesClient,
    OrderStateDto,
    ApiCurrenciesClient,
    CurrencyDto,
    OrderEditDto,
    ApiAppointmentsClient,
    AppointmentCreateDto,
    AppointmentUpdateDto,
    AddressUpdateDto,
    AppointmentDetailsDto,
    AppointmentListDto,
    ApiAppointmentsOrdersClient,
    ApiOrdersDocumentsClient,
    DocumentGroupDto,
    ApiUsersSearchClient,
    ApiUsersCompanyClient,
    UserDto,
    UserNameDto,
    ApiDocumentsFoldersClient,
    DocumentFolderTypeDto,
    DocumentUploadDto,
    DocumentCreateDto,
    OrderSalesHeaderDto,
    ApiTicketsOrdersClient,
    TicketByOrderListDto,
    DivisionTypeEnum,
    DecorBoardMaterialCodesDto,
    ApiOrdersSalesOrdersClient,
    ApiDecorboardsCodesClient,
    ApiDocumentsClient,
    DocumentStateEnum,
    ApiOrdersCustomersClient,
    ApiUsersDivisionsClient,
    ApiOrdersPartnerClient,
    ApiGroupingcategoriesDropdownClient,
    ApiOrdersStatesShippingClient,
    ApiOrdersStatesInstallationClient,
    ApiGroupingcategoriesDecorboardsDropdownClient,
    ApiOrdersStatesOnsitesurveyClient,
} from '../../../shared/clients';
import {
    IPriceViewModel, IDecorboardsViewModel
} from '../models/offer.models';
import { IAppointmentDetailViewModel, IPersonViewModel } from '../../appointments/models/appointments.model';
import { format, parse } from 'date-fns';
import { IOrderTicketModel } from '../models/ticket.model';
import { Store, select } from '@ngrx/store';
import { coreLoginT } from '../../../core/store/selectors/core.selector';
import { TokenLoginModel } from '../../../core/models/auth';



@Injectable({
    providedIn: 'root'
})
export class OrdersService {
    constructor(
        private ordersClient: ApiOrdersClient,
        private countryClient: ApiCountriesClient,
        private stateClient: ApiOrderstatesClient,
        private currencyClient: ApiCurrenciesClient,
        private decorboardsClient: ApiDecorboardsCodesClient,
        private appointmentsClient: ApiAppointmentsClient,
        private orderAppointmentsClient: ApiAppointmentsOrdersClient,
        private orderDocumentsClient: ApiOrdersDocumentsClient,
        private userSerachClient: ApiUsersSearchClient,
        private userParticipantClient: ApiUsersCompanyClient,
        private documentTypeClient: ApiDocumentsFoldersClient,
        private orderTicketClient: ApiTicketsOrdersClient,
        private userSearchClient: ApiUsersSearchClient,
        private documentClient: ApiDocumentsClient,
        private orderSalesClient: ApiOrdersSalesOrdersClient,
        private orderCustomerClient: ApiOrdersCustomersClient,
        private orderPartnerClient: ApiOrdersPartnerClient,
        private userDivisionsClient: ApiUsersDivisionsClient,
        private shippingStateClient: ApiOrdersStatesShippingClient,
        private installationClient: ApiOrdersStatesInstallationClient,
        private store: Store<any>,
        private decorboardDropdownClient: ApiGroupingcategoriesDecorboardsDropdownClient,
        private onsiteSurveyClient: ApiOrdersStatesOnsitesurveyClient
    ) {
        this.store.pipe(
            select(coreLoginT),
            map(UserDetails => this.userDetails = UserDetails
            ),
        ).subscribe();
    }
    userDetails: TokenLoginModel;
    enum: DocumentStateEnum;

    getUserDivisions(userId: number): Observable<DivisionTypeEnum[]> {
        return this.userDivisionsClient.getUserDivisions(userId);
    }

    getOrders(divisions: DivisionTypeEnum[], filter: IOrdersFilterListViewModel): Observable<PagedData<IOrdersListViewModel>> {
        const stringDivisions = divisions.map(x => x.toString());
        if (stringDivisions.includes(DivisionTypeEnum[DivisionTypeEnum.Admin])) {
            return this.getAdminOrders(filter);
        } else if (stringDivisions.includes(DivisionTypeEnum[DivisionTypeEnum.Sales])) {
            return this.getSalesOrders(filter);
        } else if (stringDivisions.includes(DivisionTypeEnum[DivisionTypeEnum.Partner])) {
            return this.getPartnerOrders(filter);
        } else {
            return this.getCustomerOrders(filter);
        }
    }
    getSalesOrders(filter: IOrdersFilterListViewModel): Observable<PagedData<IOrdersListViewModel>> {
        const createdFrom = filter.created ? new Date(filter.created) : undefined;
        const createdTo = filter.created ? new Date() : undefined;
        const deadlineFrom = filter.deadline ? new Date() : undefined;
        const deadlineTo = filter.deadline ? new Date(filter.deadline) : undefined;
        const statusDeadlineFrom = filter.statusDeadline ? new Date() : undefined;
        const statusDeadlineTo = filter.statusDeadline ? new Date(filter.statusDeadline) : undefined;
        return this.orderSalesClient.getOrdersBySales(filter.orderId, filter.workingNr, filter.currentStatus, statusDeadlineFrom,
            statusDeadlineTo,filter.responsible, filter.customer, filter.sales, createdFrom, createdTo, deadlineFrom, deadlineTo, undefined,
            filter.pager.pageIndex, filter.pager.pageSize).pipe(
                map((dto: PagedListDtoOfOrderListDto): PagedData<IOrdersListViewModel> => ({
                    items: dto.data.map(x => ({
                        id: x.id,
                        orderId: x.orderName,
                        workingNr: x.workingNumber,
                        currentStatus: x.currentStatus,
                        statusDeadline: x.statusDeadline,
                        responsible: x.responsibleName ? x.responsibleName : undefined,
                        customer: x.customerName ? x.customerName : undefined,
                        sales: x.salesName ? x.salesName : undefined,
                        created: x.createdOn,
                        deadline: x.deadline
                    })),
                    pageIndex: filter.pager.pageIndex,
                    pageSize: filter.pager.pageSize,
                    totalCount: dto.totalCount
                }))
            );
    }

    getCustomerOrders(filter: IOrdersFilterListViewModel): Observable<PagedData<IOrdersListViewModel>> {
        const createdFrom = filter.created ? new Date(filter.created) : undefined;
        const createdTo = filter.created ? new Date() : undefined;
        const deadlineFrom = filter.deadline ? new Date() : undefined;
        const deadlineTo = filter.deadline ? new Date(filter.deadline) : undefined;
        const statusDeadlineFrom = filter.statusDeadline ? new Date() : undefined;
        const statusDeadlineTo = filter.statusDeadline ? new Date(filter.statusDeadline) : undefined;
        return this.orderCustomerClient.getOrdersByCustomer(
            filter.orderId,
            filter.workingNr,
            filter.currentStatus,
            statusDeadlineFrom,
            statusDeadlineTo,
            filter.responsible,
            filter.customer,
            filter.sales,
            createdFrom,
            createdTo,
            deadlineFrom,
            deadlineTo,
            undefined,
            filter.pager.pageIndex,
            filter.pager.pageSize).pipe(
                map((dto: PagedListDtoOfOrderListDto): PagedData<IOrdersListViewModel> => ({
                    pageIndex: filter.pager.pageIndex,
                    pageSize: filter.pager.pageSize,
                    totalCount: dto.totalCount,
                    items: dto.data.map(x => ({
                        id: x.id,
                        orderId: x.orderName,
                        workingNr: x.workingNumber,
                        currentStatus: x.currentStatus,
                        statusDeadline: x.statusDeadline,
                        responsible: x.responsibleName ? x.responsibleName : undefined,
                        customer: x.customerName ? x.customerName : undefined,
                        sales: x.salesName ? x.salesName : undefined,
                        created: x.createdOn,
                        deadline: x.deadline
                    })),
                }))
            );
    }

    getAdminOrders(filter: IOrdersFilterListViewModel): Observable<PagedData<IOrdersListViewModel>> {
        const createdFrom = filter.created ? new Date(filter.created) : undefined;
        const createdTo = filter.created ? new Date() : undefined;
        const deadlineFrom = filter.deadline ? new Date() : undefined;
        const deadlineTo = filter.deadline ? new Date(filter.deadline) : undefined;
        const statusDeadlineFrom = filter.statusDeadline ? new Date() : undefined;
        const statusDeadlineTo = filter.statusDeadline ? new Date(filter.statusDeadline) : undefined;
        return this.ordersClient.getOrders(
            filter.orderId,
            filter.workingNr,
            filter.currentStatus,
            statusDeadlineFrom,
            statusDeadlineTo,
            filter.responsible,
            filter.customer,
            filter.sales,
            createdFrom,
            createdTo,
            deadlineFrom,
            deadlineTo,
            undefined,
            filter.pager.pageIndex,
            filter.pager.pageSize).pipe(
                map((dto: PagedListDtoOfOrderListDto): PagedData<IOrdersListViewModel> => ({
                    pageIndex: filter.pager.pageIndex,
                    pageSize: filter.pager.pageSize,
                    totalCount: dto.totalCount,
                    items: dto.data.map(x => ({
                        id: x.id,
                        orderId: x.orderName,
                        workingNr: x.workingNumber,
                        currentStatus: x.currentStatus,
                        currentStatusEnum: x.currentStatusEnum,
                        statusDeadline: x.statusDeadline,
                        responsible: x.responsibleName ? x.responsibleName : undefined,
                        customer: x.customerName ? x.customerName : undefined,
                        sales: x.salesName ? x.salesName : undefined,
                        created: x.createdOn,
                        deadline: x.deadline
                    })),
                }))
            );
    }

    getPartnerOrders(filter: IOrdersFilterListViewModel): Observable<PagedData<IOrdersListViewModel>> {
        const createdFrom = filter.created ? new Date(filter.created) : undefined;
        const createdTo = filter.created ? new Date() : undefined;
        const deadlineFrom = filter.deadline ? new Date() : undefined;
        const deadlineTo = filter.deadline ? new Date(filter.deadline) : undefined;
        const statusDeadlineFrom = filter.statusDeadline ? new Date() : undefined;
        const statusDeadlineTo = filter.statusDeadline ? new Date(filter.statusDeadline) : undefined;
        return this.orderPartnerClient.getOrdersByPartner(
            filter.orderId,
            filter.workingNr,
            filter.currentStatus,
            statusDeadlineFrom,
            statusDeadlineTo,
            filter.responsible,
            filter.customer,
            filter.sales,
            createdFrom,
            createdTo,
            deadlineFrom,
            deadlineTo,
            undefined,
            filter.pager.pageIndex,
            filter.pager.pageSize).pipe(
                map((dto: PagedListDtoOfOrderListDto): PagedData<IOrdersListViewModel> => ({
                    pageIndex: filter.pager.pageIndex,
                    pageSize: filter.pager.pageSize,
                    totalCount: dto.totalCount,
                    items: dto.data.map(x => ({
                        id: x.id,
                        orderId: x.orderName,
                        workingNr: x.workingNumber,
                        currentStatus: x.currentStatus,
                        currentStatusEnum: x.currentStatusEnum,
                        statusDeadline: x.statusDeadline,
                        responsible: x.responsibleName ? x.responsibleName : undefined,
                        customer: x.customerName ? x.customerName : undefined,
                        sales: x.salesName ? x.salesName : undefined,
                        created: x.createdOn,
                        deadline: x.deadline
                    })),
                }))
            );
    }

    getCustomers(name: string, take: number): Observable<IPersonViewModel[]> {
        name = (name === undefined) ? '' : name;
        return this.userSerachClient.searchUser(name, DivisionTypeEnum.Customer, take).pipe(
            map((dto: UserDto[]): IPersonViewModel[] => {
                return dto.map((user: UserDto): IPersonViewModel => {
                    return {
                        id: user.id,
                        name: user.name
                    };
                });
            })
        );
    }

    getSalesPersons(): Observable<IPersonViewModel[]> {
        return this.userParticipantClient.getUserNamesByCompany().pipe(
            map((dto: UserNameDto[]): IPersonViewModel[] => {
                return dto.map((user: UserNameDto): IPersonViewModel => {
                    return {
                        id: user.id,
                        name: user.name
                    };
                });
            })
        );
    }

    public getOrderBasics(id: string): Observable<IOrderDetailsBasicInfoViewModel> {
        return this.ordersClient.getById(id).pipe(map((res: OrderSalesHeaderDto): IOrderDetailsBasicInfoViewModel => ({
            createdOn: res.createdOn,
            currentStatus: res.currentStatus.translation,
            currentStatusState: res.currentStatus.state,
            customer: res.customer.name,
            deadline: res.deadline,
            orderId: res.orderId,
            orderName: res.orderName,
            responsible: res.responsible ? res.responsible.name : '',
            sales: res.sales.name,
            statusDeadline: res.statusDeadline,
            divisions: res.divisionTypes
        })));
    }

    public getOrder(id: string): Observable<IEditOrderViewModel> {
        return this.ordersClient.getById(id).pipe(map((res: OrderSalesHeaderDto): IEditOrderViewModel => ({
            currentStatusId: res.currentStatus.id,
            customerId: res.customer.id,
            customerName: res.customer.name,
            deadline: res.deadline,
            statusDeadline: res.statusDeadline,
            salesId: res.sales.id,
            orderId: res.orderId,
            orderName: res.orderName,
            countryId: res.customerAddress.countryId,
            postCode: res.customerAddress.postCode,
            city: res.customerAddress.city,
            address: res.customerAddress.address,
            responsibleId: res.responsible.id
        })));
    }

    deleteOrder(id: string): Observable<void> {
        return this.ordersClient.deleteOrder(id);
    }

    getCountries(): Observable<ICountriesListViewModel[]> {
        return this.countryClient.getCountries().pipe(map(x => x.map(this.countryToViewModel)));
    }

    private countryToViewModel(dto: CountryListDto): ICountriesListViewModel {
        let country: ICountriesListViewModel = {
            id: dto.id,
            code: dto.code,
            translation: dto.translation
        };
        return country;
    }

    postOrder(model: INewOrderViewModel): Observable<string> {
        const tempDeadline = new Date(model.deadline);
        tempDeadline.setMinutes(tempDeadline.getMinutes() - tempDeadline.getTimezoneOffset());
        let dto = new OrderCreateDto({
            customerUserId: model.customerId,
            orderName: model.orderName,
            salesPersonUserId: model.salesId,
            deadline: tempDeadline,
            shippingAddress: new AddressCreateDto({
                countryId: model.countryId,
                postCode: model.postCode,
                city: model.city,
                address: model.address
            })
        });
        return this.ordersClient.post(dto);
    }

    putOrder(id: string, model: IEditOrderViewModel): Observable<void> {

        const tempStatusDeadline = new Date(model.statusDeadline);
        tempStatusDeadline.setMinutes(tempStatusDeadline.getMinutes() - tempStatusDeadline.getTimezoneOffset());

        const tempDeadline = new Date(model.deadline);
        tempDeadline.setMinutes(tempDeadline.getMinutes() - tempDeadline.getTimezoneOffset());
        
        let dto = new OrderEditDto({
            customerUserId: model.customerId,
            currentStatusId: model.currentStatusId,
            assignedToUserId: model.responsibleId,
            statusDeadline: tempStatusDeadline,
            salesPersonUserId: model.salesId,
            deadline: tempDeadline,
            shippingAddress: new AddressCreateDto({
                countryId: model.countryId,
                postCode: model.postCode,
                city: model.city,
                address: model.address
            })
        });
        return this.ordersClient.updateOrder(id, dto);
    }

    getAppointment(id: number): Observable<IAppointmentDetailViewModel> {
        return this.appointmentsClient.getAppointmentById(id).pipe(map(this.appointmentToViewModel));
    }

    private appointmentToViewModel(dto: AppointmentDetailsDto): IAppointmentDetailViewModel {
        return {
            subject: dto.subject,
            date: dto.from,
            from: format(dto.from, 'HH:mm'),
            to: format(dto.to, 'HH:mm'),
            notes: dto.notes,
            address: dto.meetingRoomId === 0 ? {
                countryId: dto.address.countryId,
                postCode: dto.address.postCode,
                address: dto.address.address,
                city: dto.address.city
            } : {
                    countryId: undefined,
                    postCode: undefined,
                    address: '',
                    city: ''
                },
            categoryId: dto.categoryId,
            customerId: dto.customerId,
            customerName: dto.customerName,
            isNewAddress: dto.meetingRoomId === 0 ? true : false,
            meetingRoomId: dto.meetingRoomId,
            venueId: dto.venueId,
            partnerId: dto.partnerId
        };
    }

    getAppointments(id: string): Observable<IOrderAppointmentListViewModel[]> {
        return this.orderAppointmentsClient.getAppointmentByOrder(id)
            .pipe(map(res => res.map(this.appointmentsToViewModel)));
    }

    private appointmentsToViewModel(dto: AppointmentListDto): IOrderAppointmentListViewModel {
        return {
            id: dto.id,
            type: dto.subject,
            date: dto.from,
            from: format(dto.from, 'HH:mm'),
            to: format(dto.to, 'HH:mm'),
            notes: dto.notes,
            address: dto.address.postCode + ' ' + dto.address.city + ', ' + dto.address.address,
            customer: dto.customerName
        };
    }

    postAppointment(id: string, model: IAppointmentDetailViewModel): Observable<number> {
        const fromArray = model.from.split(':');
        const toArray = model.to.split(':');
        let dto = new AppointmentCreateDto({
            subject: model.subject,
            from: new Date(Date.UTC(model.date.getFullYear(), model.date.getMonth(), model.date.getDate(), +fromArray[0], +fromArray[1])),
            to: new Date(Date.UTC(model.date.getFullYear(), model.date.getMonth(), model.date.getDate(), +toArray[0], +toArray[1])),
            notes: model.notes,
            categoryId: model.categoryId,
            customerId: model.customerId,
            partnerId: model.partnerId
        });
        if (model.meetingRoomId) {
            dto.meetingRoomId = model.meetingRoomId;
        } else {
            dto.addressCreateDto = new AddressCreateDto({
                countryId: model.address.countryId,
                postCode: model.address.postCode,
                city: model.address.city,
                address: model.address.address
            });
        }
        return this.appointmentsClient.createAppointmentForOrder(id, dto);
    }

    putAppointment(id: number, model: IAppointmentDetailViewModel): Observable<void> {
        const fromArray = model.from.split(':');
        const toArray = model.to.split(':');
        let dto = new AppointmentUpdateDto({
            subject: model.subject,
            from: new Date(Date.UTC(model.date.getFullYear(), model.date.getMonth(), model.date.getDate(), +fromArray[0], +fromArray[1])),
            to: new Date(Date.UTC(model.date.getFullYear(), model.date.getMonth(), model.date.getDate(), +toArray[0], +toArray[1])),
            notes: model.notes,
            categoryId: model.categoryId,
            customerId: model.customerId,
            partnerId: model.partnerId
        });
        if (model.meetingRoomId > 0) {
            dto.meetingRoomId = model.meetingRoomId;
        } else {
            dto.address = new AddressUpdateDto({
                countryId: model.address.countryId,
                postCode: model.address.postCode,
                city: model.address.city,
                address: model.address.address
            });
        }
        return this.appointmentsClient.updateAppointment(id, dto);
    }

    deleteAppointment(id: number): Observable<void> {
        return this.appointmentsClient.deleteAppointment(id);
    }

    getOrderStates(): Observable<IOrderStateListViewModel[]> {
        return this.stateClient.getOrderStatuses().pipe(map(res => res.map(this.stateToViewModel)));
    }

    private stateToViewModel(dto: OrderStateDto): IOrderStateListViewModel {
        let model: IOrderStateListViewModel = {
            id: dto.id,
            state: dto.state,
            translation: dto.translation
        };
        return model;
    }

    getCurrencies(): Observable<IPriceViewModel[]> {
        return this.currencyClient.getCurrencies().pipe(
            map((dto: CurrencyDto[]): IPriceViewModel[] => {
                return dto.map((x: CurrencyDto): IPriceViewModel => ({
                    id: x.id,
                    name: x.name
                }));
            })
        );
    }

    getDecorboardsMaterial(): Observable<IDecorboardsViewModel[]> {
        return this.decorboardsClient.getDecorBoardMaterialCodes().pipe(
            map((dto: DecorBoardMaterialCodesDto[]): IDecorboardsViewModel[] => {
                return dto.map((x: DecorBoardMaterialCodesDto): IDecorboardsViewModel => ({
                    code: x.code,
                    id: x.id,
                }));
            })
        );
    }

    getOrderDocuments(id: string): Observable<IOrderDocumentViewModel> {
        return this.orderDocumentsClient.get(id).pipe(map(this.orderDocumentsToViewModel));
    }

    private orderDocumentsToViewModel(dto: DocumentGroupDto[]): IOrderDocumentViewModel {
        let model: IOrderDocumentViewModel = {
            documents: [],
            versionable: []
        };
        let versionableIndex = 0;
        let nonversionableIndex = 0;
        for (let documentDto of dto) {
            if (documentDto.isVersionable) {
                model.versionable.push({
                    documentGroupId: documentDto.documentGroupId,
                    documentGroupDto: documentDto,
                    folderId: documentDto.folderId,
                    name: documentDto.folderName,
                    type: documentDto.documentFolderType,
                    status: undefined,
                    translation: '',
                    fileCount: 0,
                    versions: []
                });
                let versionIndex = 0;
                for (let version of documentDto.versions) {
                    model.versionable[versionableIndex].versions.push({
                        versionId: version.id,
                        date: version.lastModifiedOn,
                        status: version.documentState.state,
                        translation: version.documentState.currentTranslation,
                        documents: []
                    });
                    model.versionable[versionableIndex].fileCount += version.documents.length;
                    model.versionable[versionableIndex].status = version.documentState.state;

                    for (let document of version.documents) {
                        model.versionable[versionableIndex].versions[versionIndex].documents.push({
                            id: document.id,
                            name: document.displayName,
                            type: document.fileExtensionType
                        });
                    }
                    versionIndex = versionIndex + 1;
                }
                versionableIndex = versionableIndex + 1;
            } else {
                model.documents.push({
                    documentGroupId: documentDto.documentGroupId,
                    documentGroupDto: documentDto,
                    folderId: documentDto.folderId,
                    name: documentDto.folderName,
                    type: documentDto.documentFolderType,
                    status: undefined,
                    translation: '',
                    fileCount: 0,
                    versions: []
                });
                let versionIndex = 0;
                for (let version of documentDto.versions) {
                    model.documents[nonversionableIndex].versions.push({
                        versionId: version.id,
                        date: version.lastModifiedOn,
                        status: version.documentState.state,
                        translation: version.documentState.currentTranslation,
                        documents: []
                    });
                    model.documents[nonversionableIndex].fileCount += version.documents.length;
                    model.documents[nonversionableIndex].status = version.documentState.state;

                    for (let document of version.documents) {
                        model.documents[nonversionableIndex].versions[versionIndex].documents.push({
                            id: document.id,
                            name: document.displayName,
                            type: document.fileExtensionType
                        });
                    }
                    versionIndex = versionIndex + 1;
                }
                nonversionableIndex = nonversionableIndex + 1;
            }
        }
        return model;
    }


    public getDocumentFolderTypes(): Observable<IFolderDropDownViewModel[]> {
        return this.documentTypeClient.getDocumentFolders().pipe(
            map((dto: DocumentFolderTypeDto[]): IFolderDropDownViewModel[] => {
                return dto.map(this.documentFolderTypeToViewModel);
            })
        );
    }

    private documentFolderTypeToViewModel(dto: DocumentFolderTypeDto): IFolderDropDownViewModel {
        let model: IFolderDropDownViewModel = {
            folderId: dto.folderId,
            folderName: dto.folderName,
            supportedTypes: []
        };
        for (let supportedType of dto.supportedTypes) {
            model.supportedTypes.push({
                typeId: supportedType.id,
                translation: supportedType.translation
            });
        }
        return model;
    }

    public postDocuments(model: IOrderDocumentCreateViewModel, id: string): Observable<void> {
        let dto = new DocumentUploadDto();
        dto.documentGroupId = model.folderId;
        dto.documentTypeId = model.typeId;
        dto.uploaderUserId = +this.userDetails.UserId;
        if (model.versionId > 0) {
            dto.documentGroupVersionId = model.versionId;
        }
        dto.documents = [];
        for (let document of model.documents) {
            let documentDto = new DocumentCreateDto({
                containerName: document.containerName,
                fileName: document.fileName
            });
            dto.documents.push(documentDto);
        }
        return this.orderDocumentsClient.post(id, dto);
    }


    getOrderTicket(orderId: string): Observable<IOrderTicketModel[]> {
        return this.orderTicketClient.getTicketsByOrder(orderId).pipe(
            map((dto: TicketByOrderListDto[]): IOrderTicketModel[] => {
                return dto.map((x: TicketByOrderListDto): IOrderTicketModel => ({
                    assignedTo: x.assignedTo,
                    closeOn: x.closedOn ? format(x.closedOn, 'yyyy. MM. dd.') : '',
                    deadline: x.deadline ? format(x.deadline, 'yyyy. MM. dd.') : '',
                    state: x.state
                }));
            })
        );
    }

    userSearch(name: string, divisionType: DivisionTypeEnum, take: number): Observable<IUsersListModel[]> {
        return this.userSearchClient.searchUser(name, divisionType, take);
    }

    getDocument(documentId: string): Observable<any> {
        return this.documentClient.getDocument(documentId);
    }
    getCategoriesForDropdown(): Observable<GroupingCategoryListViewModel[]> {
        return this.decorboardDropdownClient.getDecorBoardCategories().pipe(
            map(res => res.map(inside => ({
                id: inside.id,
                name: inside.name
            })))

        );
    }

    orderShipped(orderId: string): Observable<void> {
        return this.shippingStateClient.setShippinState(orderId);
    }

    orderInstalled(orderId: string): Observable<void> {
        return this.installationClient.setInstallationState(orderId);
    }

    orderOnSiteSurvey(orderId: string): Observable<void> {
        return this.onsiteSurveyClient.setOnSiteSurveyState(orderId);
    }
}
