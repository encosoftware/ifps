export interface IAppointmentsModel {
    id: number;
    subject?: string;
    customerName?: string;
    notes?: string;
    from: Date;
    fromHour: number;
    fromMinute: number;
    to: Date;
    toHour: number;
    toMinute: number;
    address: IAddressModel;
    currentDay: number;
    currentMonth: string;
}

export interface IAddressModel {
    address?: string;
    postCode: number;
    city?: string;
    countryId: number;
}
