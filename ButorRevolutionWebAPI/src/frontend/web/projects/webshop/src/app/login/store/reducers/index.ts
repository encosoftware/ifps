import { combineReducers, ActionReducer, ActionReducerMap } from '@ngrx/store';
import { coreLanguageReducers, coreLoginReducers, coreLoginTokenReducers } from './core.reducers';
import { ICoreState, ILoginState, ILoginTokenState } from '../state';

export const coreReducers: ActionReducer<ICoreState> = combineReducers({
    language: coreLanguageReducers
});
export const coreLgnReducers: ActionReducer<ILoginState> = combineReducers({
    token: coreLoginReducers
});


export const coreLoginTReducers: ActionReducer<ILoginTokenState> = combineReducers({
    loginToken: coreLoginTokenReducers
});

export const reducers: ActionReducerMap<ICoreCombineState> = {
    language: coreReducers,
    token: coreLgnReducers,
    loginToken: coreLoginTReducers
};

export interface ICoreCombineState {
    language: ICoreState;
    token: ILoginState;
    loginToken: ILoginTokenState;
}
