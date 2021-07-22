export interface IHamburgerMenuModel {
    // tslint:disable-next-line:ban-types
    action: string | Function;
    text: string;
    color: string;
    claim?: string | string[];
    icon?: string;
    isDisabled?: boolean;
}
