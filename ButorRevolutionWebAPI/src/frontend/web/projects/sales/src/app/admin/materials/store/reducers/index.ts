import { ActionReducer, combineReducers, compose } from '@ngrx/store';
import { IMaterialsFeatureState } from '../state';
import { worktopListReducer } from './worktop-list.reducer';
import { decorboardListReducer } from './decorboard-list.reducer';
import { foilListReducer } from './foil-list.reducer';
import { accessoryListReducer } from './accessory-list.reducer';
import { applianceListReducer } from './appliance-list.reducer';

export const materialsReducers = combineReducers({
    worktopList: worktopListReducer,
    decorboardList: decorboardListReducer,
    foilList: foilListReducer,
    accessoryList: accessoryListReducer,
    applianceList: applianceListReducer
});
