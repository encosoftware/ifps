export interface IAppointmentListViewModel {
    id: number;
    from: Date;
    to: Date;
    subject: string;
    customerName: string;
    address: IAddressViewModel;
    notes: string;
    categoryName?: string;
}

export interface IAppointmentDetailViewModel {
    subject: string;
    customerId: number;
    categoryId: number;
    orderId?: string | undefined;
    customerName: string;
    partnerId: number;
    date: Date;
    from: string;
    to: string;
    address: IAddressViewModel;
    meetingRoomId: number;
    notes: string;
    isNewAddress: boolean;
    venueId: number;
}

export interface IAddressViewModel {
    countryId?: number;
    postCode: number;
    city: string;
    address: string;
}

export interface IPersonViewModel {
    isTechnicalAccount?: boolean; 
    name: string;
    id: number;
}

export interface IDropDownViewModel {
    id: number;
    name?: string;
}

export interface IDropDownStringViewModel {
    id: string;
    name: string;
}
