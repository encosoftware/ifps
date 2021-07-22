import { Directive, TemplateRef, ViewContainerRef, Input } from '@angular/core';

@Directive({
  selector: '[webshopMyRefreshOnChange]'
})
export class MyRefreshOnChangeDirective {
  private value: any;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef) { }

  @Input() set webshopMyRefreshOnChange(value: any) {
    if (this.value !== value) {
      this.viewContainer.clear();
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.value = value;
    }
  }
}
