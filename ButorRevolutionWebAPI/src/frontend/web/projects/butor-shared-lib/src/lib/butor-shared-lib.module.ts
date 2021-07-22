import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CdkTableModule } from '@angular/cdk/table';
import { PortalModule } from '@angular/cdk/portal';
import { MatDialogModule } from '@angular/material/dialog';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { FormInputComponent } from './components/form-input/form-input.component';
import { FormTextareaComponent } from './components/form-textarea/form-textarea.component';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { HamburgerMenuComponent } from './components/hamburger-menu/hamburger-menu.component';
import { FormFieldComponent } from './components/form-field/form-field.component';
import { CardsComponent } from './components/form-field/cards/cards.component';
import { CardWrapperComponent } from './components/form-field/cards/card-wrapper/card-wrapper.component';
import { FormCheckboxComponent } from './components/form-checkbox/form-checkbox.component';
import { PanelComponent } from './components/panel/panel.component';
import { PerfectScrollbarModule, PerfectScrollbarConfigInterface, PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar';
import { LayoutOneColComponent } from './components/layout-one-col/layout-one-col.component';
import { LayoutPanelCenterComponent } from './components/layout-one-col/layout-panel-center';
import { LayoutOneColActionsComponent } from './components/layout-one-col/layout-one-col-actions';
import { NgSelectModule } from './components/ng-select/ng-select.module';
import { LayoutTwoColComponent } from './components/layout-two-col/layout-two-col.component';
import { LayoutTwoColActionsComponent } from './components/layout-two-col/layout-two-col-actions.component';
import { LayoutPanelRightComponent } from './components/layout-two-col/layout-panel-right';
import { LayoutPanelLeftComponent } from './components/layout-two-col/layout-panel-left';
import { LayoutPanelBottomComponent } from './components/layout-two-row/layout-panel-bottom';
import { LayoutPanelTopComponent } from './components/layout-two-row/layout-panel-top';
import { LayoutTwoRowComponent } from './components/layout-two-row/layout-two-row.component';
import { SnackbarService } from './services/snackbar.service';
import { ProfileComponent } from './components/profile/profile.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  wheelPropagation: true    
};
@NgModule({
  imports: [
    CommonModule,
    HttpClientModule,
    RouterModule,
    MatMenuModule,
    PerfectScrollbarModule,
    MatProgressSpinnerModule,
    CdkTableModule,
    PortalModule,
    NgSelectModule,
    MatSnackBarModule,
    ReactiveFormsModule,
    FormsModule,
    MatDialogModule,
  ],
  declarations: [
    FormInputComponent,
    FormTextareaComponent,
    FormFieldComponent,
    CardsComponent,
    CardWrapperComponent,
    BreadcrumbComponent,
    HamburgerMenuComponent,
    FormCheckboxComponent,
    PanelComponent,
    LayoutOneColComponent,
    LayoutPanelCenterComponent,
    LayoutOneColActionsComponent,
    LayoutTwoColComponent,
    LayoutTwoColActionsComponent,
    LayoutPanelRightComponent,
    LayoutPanelLeftComponent,
    LayoutPanelBottomComponent,
    LayoutPanelTopComponent,
    LayoutTwoRowComponent,
    ProfileComponent
  ],
  exports: [
    HttpClientModule,
    FormInputComponent,
    FormTextareaComponent,
    FormFieldComponent,
    CardsComponent,
    CardWrapperComponent,
    BreadcrumbComponent,
    RouterModule,
    HamburgerMenuComponent,
    FormCheckboxComponent,
    PanelComponent,
    PerfectScrollbarModule,
    LayoutOneColComponent,
    LayoutPanelCenterComponent,
    LayoutOneColActionsComponent,
    MatProgressSpinnerModule,
    CdkTableModule,
    LayoutTwoColComponent,
    LayoutTwoColActionsComponent,
    LayoutPanelRightComponent,
    LayoutPanelLeftComponent,
    LayoutPanelBottomComponent,
    LayoutPanelTopComponent,
    LayoutTwoRowComponent,
    MatSnackBarModule,
    ReactiveFormsModule,
    FormsModule,
    ReactiveFormsModule,
    FormsModule,
    MatDialogModule,
    ProfileComponent,
    NgSelectModule
  ],
  providers: [
    {
      provide: PERFECT_SCROLLBAR_CONFIG,
      useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG
    },
    SnackbarService
  ],
  entryComponents: [ProfileComponent]
})
export class ButorSharedLibModule { }
