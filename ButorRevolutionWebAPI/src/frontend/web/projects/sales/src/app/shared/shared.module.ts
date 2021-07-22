
import { NgModule, LOCALE_ID } from '@angular/core';
import { CommonModule, registerLocaleData } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatNativeDateModule, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule } from '@angular/material/dialog';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTreeModule } from '@angular/material/tree';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { PortalModule } from '@angular/cdk/portal';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { TooltipComponent } from './components/tooltip/tooltip.component';
import { KeysPipe } from './pipes/object-keys.pipe';
import { HttpClientModule } from '@angular/common/http';
import { MatPaginatorModule } from '@angular/material/paginator';
import { CdkTableModule } from '@angular/cdk/table';
import { IncludePipe, NotIncludePipe } from './pipes/include.pipe';
import { ImageCropperModule } from 'ngx-image-cropper';
import { UploadPicComponent } from './components/upload-pic/upload-pic.component';
import { LayoutThreeColActionsComponent } from './components/layout-three-col/layout-three-col-actions.components';
import { LayoutThreeColComponent } from './components/layout-three-col/layout-three-col.component';
import { LayoutPanelMiddleComponent } from './components/layout-three-col/layout-panel-middle';
import {
  CalendarModule, DateAdapter
} from 'angular-calendar';
import { DateTestPipe } from './pipes/date-test.pipe';
import { PictureSrcPipe } from './pipes/picture-src.pipe';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { MaterialTreePaddingDirective } from './directives/material-tree-padding.directive';
import { TranslateModule } from '@ngx-translate/core';

import { RouterModule } from '@angular/router';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { LanguagePipe } from './pipes/language.pipe';
import { MaterialMenuComponent } from './components/materialMenu/material-menu.component';
import { MustMatchDirective } from './directives/must-match.directive';
import { CookieService } from 'ngx-cookie-service';
import { ClaimsDirective } from './directives/claims.directive';
import { MyRefreshOnChangeDirective } from './directives/my-refresh-on-change.directive';
import { BtnHamburgerComponent } from './components/btn-hamburger/btn-hamburger.component';
import { BtnHamburgerInsideComponent } from './components/btn-hamburger/btn-hamburger-inside.component';
import { ButorSharedLibModule } from 'butor-shared-lib';
import localeHu from '@angular/common/locales/hu';
import { ImageDirective } from './directives/image.directive';
import { MultiUploadPicComponent } from './components/multi-upload-pic/multi-upload-pic.component';
import { SpaceSeparatorPipe } from './pipes/space-separator.pipe';
import { OnlyNumbersDirective } from './directives/only-numbers.directive';


const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  wheelPropagation: true
};
registerLocaleData(localeHu);
@NgModule({
  imports: [
    CommonModule,
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
    MatChipsModule,
    ReactiveFormsModule,
    PerfectScrollbarModule,
    PortalModule,
    FormsModule,
    HttpClientModule,
    CdkTableModule,
    TranslateModule,
    RouterModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory,
    }),
    MatProgressBarModule,
    ImageCropperModule,
    ButorSharedLibModule,
    MatDatepickerModule,
    MatNativeDateModule,
  ],
  declarations: [
    KeysPipe,
    TooltipComponent,
    LayoutThreeColActionsComponent,
    LayoutThreeColComponent,
    LayoutPanelMiddleComponent,
    IncludePipe,
    NotIncludePipe,
    UploadPicComponent,
    DateTestPipe,
    ConfirmDialogComponent,
    PictureSrcPipe,
    MaterialTreePaddingDirective,
    LanguagePipe,
    MustMatchDirective,
    MaterialMenuComponent,
    ClaimsDirective,
    MyRefreshOnChangeDirective,
    BtnHamburgerComponent,
    BtnHamburgerInsideComponent,
    ImageDirective,
    MultiUploadPicComponent,
    SpaceSeparatorPipe,
    OnlyNumbersDirective
  ],
  exports: [
    CommonModule,
    MatExpansionModule,
    MatButtonModule,
    MatDialogModule,
    TooltipComponent,
    ReactiveFormsModule,
    MatTableModule,
    MatTabsModule,
    MatSnackBarModule,
    PerfectScrollbarModule,
    MatDatepickerModule,
    MatNativeDateModule,
    FormsModule,
    HttpClientModule,
    MatProgressSpinnerModule,
    LayoutThreeColActionsComponent,
    LayoutThreeColComponent,
    LayoutPanelMiddleComponent,
    CdkTableModule,
    MatPaginatorModule,
    MatSortModule,
    IncludePipe,
    NotIncludePipe,
    UploadPicComponent,
    MatTreeModule,
    MatIconModule,
    CalendarModule,
    DateTestPipe,
    BtnHamburgerComponent,
    BtnHamburgerInsideComponent,
    ImageDirective,
    ConfirmDialogComponent,
    PictureSrcPipe,
    MaterialTreePaddingDirective,
    TranslateModule,
    RouterModule,
    LanguagePipe,
    MustMatchDirective,
    MaterialMenuComponent,
    MatChipsModule,
    ClaimsDirective,
    MyRefreshOnChangeDirective,
    MatProgressBarModule,
    ImageCropperModule,
    ButorSharedLibModule,
    MultiUploadPicComponent,
    SpaceSeparatorPipe,
    OnlyNumbersDirective
  ],
  providers: [
    {
      provide: PERFECT_SCROLLBAR_CONFIG,
      useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG
    },
    SpaceSeparatorPipe,
    CookieService,
    { provide: LOCALE_ID, useValue: 'hu-HU' },
    { provide: MAT_DATE_LOCALE, useValue: 'hu-HU' }
  ],
  entryComponents: [ConfirmDialogComponent]
})
export class SharedModule { }
