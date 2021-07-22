import { Component, OnInit, TemplateRef, ViewChild, ViewContainerRef, AfterViewInit } from '@angular/core';
import { TemplatePortal } from '@angular/cdk/portal';

@Component({
    selector: 'butor-layout-panel-middle',
    template: '<ng-template><ng-content></ng-content></ng-template>'
})
export class LayoutPanelMiddleComponent implements OnInit {

    @ViewChild(TemplateRef, {static: true}) private implicitContent: TemplateRef<any>;

    private contentPortal: TemplatePortal | null = null;

    get middleContent(): TemplatePortal | null {
        return this.contentPortal;
    }

    constructor(private viewContainerRef: ViewContainerRef) { }

    ngOnInit(): void {
        this.contentPortal = new TemplatePortal(this.implicitContent, this.viewContainerRef);
    }
}
