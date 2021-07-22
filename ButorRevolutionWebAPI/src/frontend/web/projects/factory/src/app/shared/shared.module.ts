import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import {
  PerfectScrollbarConfigInterface,
  PerfectScrollbarModule,
  PERFECT_SCROLLBAR_CONFIG
} from 'ngx-perfect-scrollbar';
import { HttpClientModule } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTreeModule } from '@angular/material/tree';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { PortalModule } from '@angular/cdk/portal';
import { CdkTableModule } from '@angular/cdk/table';
import { TranslateModule } from '@ngx-translate/core';
import {
  ButorSharedLibModule
} from 'butor-shared-lib';
import { SecConverterPipe } from './pipes/process-time.pipe';
import { MustMatchDirective } from './directives/must-match.directive';
import { CookieService } from 'ngx-cookie-service';
import { ClaimsDirective } from './directives/claims.directive';
import { MaterialMenuComponent } from './components/material-menu/material-menu.component';
import { LanguagePipe } from './pipes/language.pipe';
import { TooltipComponent } from './components/tooltip/tooltip.component';
import { IncludePipe, NotIncludePipe } from './pipes/include.pipe';
import { UploadPicComponent } from './components/upload-pic/upload-pic.component';
import { ImageDirective } from './directives/image.directive';
import { MyRefreshOnChangeDirective } from './directives/my-refresh-on-change.directive';
import { BtnHamburgerComponent } from './components/btn-hamburger/btn-hamburger.component';
import { BtnHamburgerInsideComponent } from './components/btn-hamburger/btn-hamburger-inside.component';
import { NewWindowComponent } from './components/new-window/new-window.component';
import {DragDropModule} from '@angular/cdk/drag-drop';
import { ChartsModule } from 'ng2-charts';
import { OrderAmountValidatorDirective } from './directives/order-amount-validator.directive';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  wheelPropagation: true
};

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    PerfectScrollbarModule,
    HttpClientModule,
    ReactiveFormsModule,
    CdkTableModule,
    PortalModule,
    FormsModule,
    TranslateModule,
    MatExpansionModule,
    MatButtonModule,
    MatDialogModule,
    MatTableModule,
    MatTabsModule,
    MatMenuModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatPaginatorModule,
    MatSortModule,
    MatTreeModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatChipsModule,
    MatProgressBarModule,
    ButorSharedLibModule,
    DragDropModule,
    ChartsModule
  ],
  declarations: [
    SecConverterPipe,
    MustMatchDirective,
    ClaimsDirective,
    MaterialMenuComponent,
    LanguagePipe,
    TooltipComponent,
    IncludePipe,
    NotIncludePipe,
    UploadPicComponent,
    ImageDirective,
    MyRefreshOnChangeDirective,
    BtnHamburgerComponent,
    BtnHamburgerInsideComponent,
    ClaimsDirective,
    NewWindowComponent,
    OrderAmountValidatorDirective
  ],
  exports: [
    MatButtonModule,
    MatSortModule,
    UploadPicComponent,
    RouterModule,
    CommonModule,
    PerfectScrollbarModule,
    HttpClientModule,
    MatExpansionModule,
    ReactiveFormsModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatTreeModule,
    MatProgressBarModule,
    MatIconModule,
    CdkTableModule,
    MatChipsModule,
    MatTableModule,
    MatTabsModule,
    FormsModule,
    TranslateModule,
    MatDialogModule,
    MatSnackBarModule,
    MatDatepickerModule,
    MatNativeDateModule,
    SecConverterPipe,
    MustMatchDirective,
    ClaimsDirective,
    MaterialMenuComponent,
    LanguagePipe,
    TooltipComponent,
    IncludePipe,
    NotIncludePipe,
    ImageDirective,
    MyRefreshOnChangeDirective,
    BtnHamburgerComponent,
    BtnHamburgerInsideComponent,
    ButorSharedLibModule,
    NewWindowComponent,
    DragDropModule,
    ChartsModule,
    OrderAmountValidatorDirective
  ],
  providers: [
    SecConverterPipe,
    {
      provide: PERFECT_SCROLLBAR_CONFIG,
      useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG
    },
    CookieService
  ]
})
export class SharedModule {}
