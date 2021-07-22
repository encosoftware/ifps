import { combineReducers } from '@ngrx/store';
import { CoreAction, ActionTypes, CoreActionToken, CoreActionLogin } from '../actions/core.actions';
import { TokenModel, TokenLoginModel } from '../../../shared/models/auth';
import { LanguageTypeEnum } from '../../../shared/clients';

const initialLanguageState: LanguageTypeEnum = LanguageTypeEnum.EN;
const initialLoginState: TokenModel = ({
    accessToken: '',
    refreshToken: ''
});
const initialLoginTokenState: TokenLoginModel = ({
    accessToken: '',
    refreshToken: '',
    Email: '',
    ImageContainerName: '',
    ImageFileName: '',
    BasketId: '',
    Language: '',
    RoleName: '',
    UserName: '',
    IFPSClaim: [],
    UserId: '',
});

function coreLoginTokenChoosenReducers(
    state: TokenLoginModel = initialLoginTokenState,
    action: CoreActionLogin
): TokenLoginModel {

    switch (action.type) {
        case ActionTypes.Login:
            return action.payload;
    }

    return state;
}

export const coreLoginTokenReducers = combineReducers({
    loginToken: coreLoginTokenChoosenReducers
});

function coreLanguageChoosenReducers(
    state: LanguageTypeEnum = initialLanguageState,
    action: CoreAction
): LanguageTypeEnum {

    switch (action.type) {
        case ActionTypes.Language:
            return action.payload;
    }

    return state;
}

export const coreLanguageReducers = combineReducers({
    language: coreLanguageChoosenReducers
});

function coreLoginChoosenReducers(
    state: TokenModel = initialLoginState,
    action: CoreActionToken
): TokenModel {

    switch (action.type) {
        case ActionTypes.Token:
            return { ...action.payload };
    }

    return { ...state };
}

export const coreLoginReducers = combineReducers({
    token: coreLoginChoosenReducers
});

