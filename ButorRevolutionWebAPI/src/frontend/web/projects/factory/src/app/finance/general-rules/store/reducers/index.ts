import { combineReducers, compose } from '@ngrx/store';
import { generalRulesListReducers } from './general-rules.reducers';

export const generalRulesReducers = combineReducers({
    generalRulesList: generalRulesListReducers
});
