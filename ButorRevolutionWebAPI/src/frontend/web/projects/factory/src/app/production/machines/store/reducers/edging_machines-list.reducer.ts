import { PagerModel } from '../../../../../../../factory/src/app/shared/models/pager.model';
import { combineReducers } from '@ngrx/store';
import { IEdgingMachinesFilterViewModel } from '../../models/edging_machines.model';
import { EdgingMachinesAction, ActionTypes } from '../actions/edging_machines-list.actions';

const initialState: IEdgingMachinesFilterViewModel = {
    machineName: '',
    softwareVersion: '',
    serialNumber: '',
    code: '',
    yearOfManufacture: undefined,
    brandId: undefined,
    pager: new PagerModel()
};

function edgingMachinesListFilterReducer(
    state: IEdgingMachinesFilterViewModel = initialState,
    action: EdgingMachinesAction
): IEdgingMachinesFilterViewModel {
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

export const edgingMachinesListReducer = combineReducers({
    filters: edgingMachinesListFilterReducer
});
