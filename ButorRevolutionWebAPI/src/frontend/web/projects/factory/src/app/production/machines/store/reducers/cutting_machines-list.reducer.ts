import { PagerModel } from '../../../../../../../factory/src/app/shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { ICuttingMachinesFilterViewModel } from '../../models/cutting_machines.model';
import { CuttingMachinesAction, ActionTypes } from '../actions/cutting_machines-list.actions';

const initialState: ICuttingMachinesFilterViewModel = {
    machineName: '',
    softwareVersion: '',
    serialNumber: '',
    code: '',
    yearOfManufacture: undefined,
    brandId: undefined,
    pager: new PagerModel()
};

function cuttingMachinesListFilterReducer(
    state: ICuttingMachinesFilterViewModel = initialState,
    action: CuttingMachinesAction
): ICuttingMachinesFilterViewModel {
    switch (action.type) {
        case ActionTypes.ChangeFilter:
            return {
                ...state,
                ...action.payload
            };

        case ActionTypes.DeleteFilter:
            return {
                ...initialState
            };
    }

    return state;
}

export const cuttingMachinesListReducer = combineReducers({
    filters: cuttingMachinesListFilterReducer
});
