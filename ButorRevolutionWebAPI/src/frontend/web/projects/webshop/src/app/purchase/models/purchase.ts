export interface BasketPurchaseViewModel {
    customerId?: number;
    name?: string;
    emailAddress?: string;
    note?: string;
    taxNumber?: string;
    gaveEmailConsent: boolean;
    delieveryAddress?: AddressCreateModel;
    billingAddress?: AddressCreateModel;
}

export interface AddressCreateModel {
    address?: string;
    postCode: number;
    city?: string;
    countryId: number;
}

export interface CountryListSelectModel {
    id: number;
    name: string;
}

