import { Component, OnInit, TemplateRef, ViewChild, ViewContainerRef } from '@angular/core';
import { TemplatePortal } from '@angular/cdk/portal';

@Component({
    selector: 'butor-layout-panel-bottom',
    template: '<ng-template><ng-content></ng-content></ng-template>'
})
export class LayoutPanelBottomComponent implements OnInit {

    @ViewChild(TemplateRef, { static: true }) private implicitContent: TemplateRef<any>;

    private contentPortal: TemplatePortal | null = null;

    get bottomContent(): TemplatePortal | null {
        return this.contentPortal;
    }

    constructor(private viewContainerRef: ViewContainerRef) { }

    ngOnInit(): void {
        this.contentPortal = new TemplatePortal(this.implicitContent, this.viewContainerRef);
    }
}
