import { createSelector, createFeatureSelector } from '@ngrx/store';
import { ICoreState, ILoginState, IClaimState, ILoginTokenState } from '../state';
import { LanguageTypeEnum } from '../../../shared/clients';
import { TokenModel, TokenLoginModel } from '../../models/auth';


export const coreSelector = createFeatureSelector<any, ICoreState>('core');
export const coreLgnSelector = createFeatureSelector<any, ILoginState>('login');
export const coreClaimsSelector = createFeatureSelector<any, IClaimState>('claims');
export const coreLoginSelector = createFeatureSelector<any, ILoginTokenState>('loginToken');

export const coreLanguage = createSelector(
    coreSelector,
    (state): LanguageTypeEnum => state.language.language
);
export const coreLogin = createSelector(
    coreLgnSelector,
    (state): TokenModel => state ? state.token.token : null

);
export const coreClaims = createSelector(
    coreClaimsSelector,
    (state): string[] => state.claims.claims

);
export const coreLoginT = createSelector(
    coreLoginSelector,
    (state): TokenLoginModel => state.loginToken.loginToken

);


