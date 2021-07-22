import { combineReducers, ActionReducer, ActionReducerMap } from '@ngrx/store';
import { ICoreState, ILoginState, IClaimState, ILoginTokenState } from '../state';
import { coreLoginTokenReducers, coreClaimsReducers, coreLoginReducers, coreLanguageReducers } from './core.reducers';

export const coreReducers: ActionReducer<ICoreState> = combineReducers({
    language: coreLanguageReducers
});
export const coreLgnReducers: ActionReducer<ILoginState> = combineReducers({
    token: coreLoginReducers
});

export const coreClaimReducers: ActionReducer<IClaimState> = combineReducers({
    claims: coreClaimsReducers
});

export const coreLoginTReducers: ActionReducer<ILoginTokenState> = combineReducers({
    loginToken: coreLoginTokenReducers
});

export const reducers: ActionReducerMap<ICoreCombineState> = {
    language: coreReducers,
    token: coreLgnReducers,
    claims: coreClaimReducers,
    loginToken: coreLoginTReducers
};

export interface ICoreCombineState {
    language: ICoreState;
    token: ILoginState;
    claims: IClaimState;
    loginToken: ILoginTokenState;
}
