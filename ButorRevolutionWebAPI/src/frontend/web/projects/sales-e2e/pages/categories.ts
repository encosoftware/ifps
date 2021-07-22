import { element, by } from "protractor";
import { expect } from "chai";

export class CategoryPageObj {

    constructor() {
    }

    public getTree() {
        return element.all(by.css("mat-tree-node span")).first();
    }
    public getHamburgerMenu() {
        return element.all(by.tagName("butor-hamburger-menu"));
    }

    public getHeaderCategoryPage() {
        return element.all(by.tagName("h3")).first();
    }
    public getPopupHeader() {
        return element(by.css(".ng-star-inserted .popupHeader"));
    }
    public isPresentPopupHeader() {
        return this.getPopupHeader().isPresent();
    }
    public isPresentSnackBar() {
        return this.getByClassNameAll("mat-simple-snackbar").first().isPresent();
    }
    public isPresentHamburger() {
        return this.getByClassNameAll("mat-menu-content").first().isPresent();
    }
    public getMainCat(text: string) {
        return element.all(by.linkText(text));
    }
    public getEditBtn() {
        return element.all(by.buttonText("Edit"));
    }
    public getpopUpisPresent() {
        return element(by.css(".cdk-overlay-backdrop-showing")).isPresent();
    }
    public getPopupHeaderText() {
        return element(by.css(".ng-star-inserted .popupHeader")).getText();
    }
    public getInput(text: string) {
        return element(by.css(`input[name="${text}"]`));
    }
    public getClickBtn(text: string) {
        return element.all(by.buttonText(text)).click();
    }
    public getCssSelectAll(text: string) {
        return element.all(by.css(text));
    }
    public getByClassNameAll(text: string) {
        return element.all(by.className(text))
    }
    public getSelectPopup(text: string) {
        return element(by.css(`ng-select[name="${text}"]`)).click();
    }
    public getOptionSelect(text: string) {
        return element.all(by.css(`div[role="${text}"]`)).get(0).click();
    }

    public selectedElement(text : string) {
        return this.getCssSelectAll(".ng-select").first().getText().then(
            el => expect(el).to.equal(`${text}\n×`)).catch((err)=> {throw err})
    }
}
