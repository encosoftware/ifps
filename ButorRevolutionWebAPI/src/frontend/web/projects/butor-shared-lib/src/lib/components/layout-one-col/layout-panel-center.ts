import { Component, OnInit, TemplateRef, ViewChild, ViewContainerRef } from '@angular/core';
import { TemplatePortal } from '@angular/cdk/portal';

@Component({
    selector: 'butor-layout-panel-center',
    template: '<ng-template><ng-content></ng-content></ng-template>'
})
export class LayoutPanelCenterComponent implements OnInit {

    @ViewChild(TemplateRef, { static: true }) private implicitContent: TemplateRef<any>;

    private contentPortal: TemplatePortal | null = null;

    get centerContent(): TemplatePortal | null {
        return this.contentPortal;
    }

    constructor(private viewContainerRef: ViewContainerRef) { }

    ngOnInit(): void {
        this.contentPortal = new TemplatePortal(this.implicitContent, this.viewContainerRef);
    }
}
