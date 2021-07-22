import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from '../../shared/shared.module';
import { generalExpensesReducers } from './store/reducers';
import { GeneralExpensesRoutingModule } from './general-expenses-routing.module';
import { GeneralExpensesPageComponent } from './pages/general-expenses.page.component';
import { NewExpenseComponent } from './components/new-expense/new-expense.component';
import { RecurringCostsComponent } from './components/recurring-costs/recurring-costs.component';

@NgModule({
  declarations: [
    GeneralExpensesPageComponent,
    NewExpenseComponent,
    RecurringCostsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    GeneralExpensesRoutingModule,
    StoreModule.forFeature('generalExpenses', generalExpensesReducers)
  ],
  entryComponents: [
    NewExpenseComponent,
    RecurringCostsComponent
  ],
  providers: [
  ]
})
export class GeneralExpensesModule { }
