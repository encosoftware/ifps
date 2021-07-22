import { IWorktopListState } from './worktop-list.state';
import { IDecorboardListState } from './decorboard-list.state';
import { IFoilListState } from './foil-list.state';
import { IAccessoryListState } from './accessory-list.state';
import { IApplianceListState } from './appliance-list.state';

export interface IMaterialsFeatureState {
    worktopList: IWorktopListState;
    decorboardList: IDecorboardListState;
    foilList: IFoilListState;
    accessoryList: IAccessoryListState;
    applianceList: IApplianceListState;
}
