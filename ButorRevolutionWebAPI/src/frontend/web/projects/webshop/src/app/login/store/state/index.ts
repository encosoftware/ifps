import { ICoreLanguageState, ICoreLoginState, ILoginTState } from './core.state';

export interface ICoreState {
    language: ICoreLanguageState;
}
export interface ILoginState {
    token: ICoreLoginState;
}

export interface ILoginTokenState {
    loginToken: ILoginTState;
}
