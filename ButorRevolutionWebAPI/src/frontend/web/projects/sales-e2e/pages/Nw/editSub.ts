import { $, ElementFinder, element } from "protractor";

export class EditSubCategory {
    public clickLink: ElementFinder;
    public home: ElementFinder;
    public hamburger: ElementFinder;
    public subCat: ElementFinder;
    public edit: ElementFinder;
    public popupHeader: ElementFinder;
    public bla: ElementFinder;
    public mainBtn: ElementFinder;

    constructor() {
        this.clickLink = $(".menu-list-a");
        this.home = element(".container__menu-name");
        this.popupHeader = $(".popupHeader");
        this.hamburger = $("butor-hamburger-menu");
        this.mainBtn = element(".btn-group button");
        this.subCat = $(".subcat");
        this.edit = $(".hamburger-menu-list-item:nth-child(2) button");
        this.bla = $("#item1");
    }
    public bal() {
        return element("#item1");
    }
    public engInput() {
        return element(".input-control:nth-child(1)");
    }
}
