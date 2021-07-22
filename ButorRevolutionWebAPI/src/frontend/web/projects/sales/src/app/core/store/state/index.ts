import { ICoreLanguageState, ICoreLoginState, ICoreClaimsState, ILoginTState } from './core.state';

export interface ICoreState {
    language: ICoreLanguageState;
}
export interface ILoginState {
    token: ICoreLoginState;
}

export interface IClaimState {
    claims: ICoreClaimsState;
}

export interface ILoginTokenState {
    loginToken: ILoginTState;
}
