import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from '../../shared/shared.module';
import { generalRulesReducers } from './store/reducers';
import { GeneralRulesRoutingModule } from './general-rules-routing.module';
import { GeneralRulesPageComponent } from './pages/general-rules.page.component';
import { NewRuleComponent } from './components/new-rule/new-rule.component';

@NgModule({
  declarations: [
    GeneralRulesPageComponent,
    NewRuleComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    GeneralRulesRoutingModule,
    StoreModule.forFeature('generalRules', generalRulesReducers)
  ],
  entryComponents: [
    NewRuleComponent
  ],
  providers: [
  ]
})
export class GeneralRulesModule { }
