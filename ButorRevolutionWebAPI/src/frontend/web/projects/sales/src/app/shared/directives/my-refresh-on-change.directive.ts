import { Directive, TemplateRef, ViewContainerRef, Input, OnDestroy } from '@angular/core';

@Directive({
  selector: '[butorMyRefreshOnChange]'
})
export class MyRefreshOnChangeDirective {


  private value: any;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef) { }

  @Input() set butorMyRefreshOnChange(value: any) {
    if (this.value !== value) {
      this.viewContainer.clear();
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.value = value;
    }
  }
}
