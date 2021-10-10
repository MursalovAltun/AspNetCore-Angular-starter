import { AppEpic } from "../../../app/store";
import { filter, map, switchMap, tap } from "rxjs/operators";
import { AuthRegistrationActions } from "../actions";
import { from } from "rxjs";
import createApiClient from "../../../app/create-api-client";
import { AccountClient, AccountCreateRequest } from "../../../app/api";
import * as tokenManager from "../token-manager";
import jwtDecode from "jwt-decode";
import { TokenPayload } from "../models";

export const register: AppEpic = action$ =>
  action$.pipe(
    filter(AuthRegistrationActions.register.match),
    switchMap(({ payload }) =>
      from(createApiClient(AccountClient).create(new AccountCreateRequest(payload))).pipe(
        tap(({ token }) => tokenManager.setAuthToken(token!)),
        map(({ token }) => jwtDecode<TokenPayload>(token!)),
        map(({ nameid }) => AuthRegistrationActions.registerSuccess({ currentUserId: nameid }))
      )
    )
  );
