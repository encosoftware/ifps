import { ICuttingMachinesListState } from './cutting_machines-list.state';
import { ICncMachinesListState } from './cnc_machines-list.state';
import { IEdgingMachinesListState } from './edging_machines-list.state';

export interface IMachinesFeatureState {
    cuttingMachinesList: ICuttingMachinesListState;
    cncMachinesList: ICncMachinesListState;
    edgingMachinesList: IEdgingMachinesListState;
}
