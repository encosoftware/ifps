import { combineReducers, compose } from '@ngrx/store';
import { cuttingMachinesListReducer } from './cutting_machines-list.reducer';
import { cncMachinesListReducer } from './cnc_machines-list.reducer';
import { edgingMachinesListReducer } from './edging_machines-list.reducer';

export const machinesReducers = combineReducers({
    cuttingMachinesList: cuttingMachinesListReducer,
    cncMachinesList: cncMachinesListReducer,
    edgingMachinesList: edgingMachinesListReducer
});
