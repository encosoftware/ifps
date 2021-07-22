import { PagerModel } from '../../../../../../../factory/src/app/shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { ICncMachinesFilterViewModel } from '../../models/cnc_machines.model';
import { CncMachinesAction, ActionTypes } from '../actions/cnc_machines-list.actions';

const initialState: ICncMachinesFilterViewModel = {
    machineName: '',
    softwareVersion: '',
    serialNumber: '',
    code: '',
    yearOfManufacture: undefined,
    brandId: undefined,
    pager: new PagerModel()
};

function cncMachinesListFilterReducer(
    state: ICncMachinesFilterViewModel = initialState,
    action: CncMachinesAction
): ICncMachinesFilterViewModel {
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

export const cncMachinesListReducer = combineReducers({
    filters: cncMachinesListFilterReducer
});
