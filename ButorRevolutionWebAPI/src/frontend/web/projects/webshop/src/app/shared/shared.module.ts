import { NgModule } from '@angular/core';
import { FooterTextColumnComponent } from './components/footer-text-column/footer-text-column.component';
import { MenuComponent } from './components/menu/menu.component';
import {
  ButorSharedLibModule,
} from 'butor-shared-lib';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatNativeDateModule } from '@angular/material/core';
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
import { PerfectScrollbarConfigInterface, PERFECT_SCROLLBAR_CONFIG, PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { ImageSliderComponent } from './components/image-slider/image-slider.component';
import { CommonModule } from '@angular/common';
import { CdkTableModule } from '@angular/cdk/table';
import { RadiobuttonComponent } from './components/radiobutton/radiobutton.component';
import { Ng5SliderModule } from 'ng5-slider';
import { FormsModule } from '@angular/forms';
import { ImageSliderRowComponent } from './components/image-slider-row/image-slider-row.component';
import { HttpClientModule } from '@angular/common/http';
import {MatGridListModule} from '@angular/material/grid-list';
import { ImageDirective } from './directives/image.directive';
import { LanguagePipe } from './pipes/language.pipe';
import { CookieService } from 'ngx-cookie-service';
import { MustMatchDirective } from './directives/must-match.directive';
import { MyRefreshOnChangeDirective } from './directives/my-refresh-on-change.directive';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';


const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  wheelPropagation: true 
};

@NgModule({
  declarations: [
    FooterTextColumnComponent,
    MenuComponent,
    ImageSliderComponent,
    RadiobuttonComponent,
    ImageSliderRowComponent,
    ImageDirective,
    LanguagePipe,
    MustMatchDirective,
    MyRefreshOnChangeDirective
  ],
  imports: [
    CommonModule,
    RouterModule,
    CdkTableModule,
    MatExpansionModule,
    MatButtonModule,
    PerfectScrollbarModule,
    MatDialogModule,
    MatTableModule,
    MatTabsModule,
    MatMenuModule,
    TranslateModule,
    MatProgressSpinnerModule,
    MatSortModule,
    MatSnackBarModule,
    MatTreeModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatChipsModule,
    MatGridListModule,
    MatProgressBarModule,
    Ng5SliderModule,
    FormsModule,
    HttpClientModule,
    ButorSharedLibModule
  ],
  exports: [
    CommonModule,
    ButorSharedLibModule,
    PerfectScrollbarModule,
    FooterTextColumnComponent,
    MenuComponent,
    CdkTableModule,
    TranslateModule,
    MatExpansionModule,
    MatButtonModule,
    MatGridListModule,
    MatDialogModule,
    MatTableModule,
    MatTabsModule,
    MatMenuModule,
    MatProgressSpinnerModule,
    MatSortModule,
    MatSnackBarModule,
    MatTreeModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatChipsModule,
    MatProgressBarModule,
    ImageSliderComponent,
    RadiobuttonComponent,
    Ng5SliderModule,
    FormsModule,
    ImageSliderRowComponent,
    HttpClientModule,
    ImageDirective,
    LanguagePipe,
    MustMatchDirective,
    MyRefreshOnChangeDirective,
  ],
  providers: [
    {
      provide: PERFECT_SCROLLBAR_CONFIG,
      useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG
    },
    CookieService
  ]
})
export class SharedModule { }
