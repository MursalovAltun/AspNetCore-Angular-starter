import { filter, map, mapTo, switchMap, tap } from "rxjs/operators";
import { AuthSignInActions } from "../actions";
import createApiClient from "../../../app/create-api-client";
import { AuthClient } from "../../../app/api";
import { from } from "rxjs";
import * as tokenManager from "../token-manager";
import jwtDecode from "jwt-decode";
import { AppEpic } from "../../../app/store";
import { push } from "connected-react-router";
import { TokenPayload } from "../models";

export const signInEpic: AppEpic = action$ =>
  action$.pipe(
    filter(AuthSignInActions.signIn.match),
    switchMap(({ payload }) =>
      from(createApiClient(AuthClient).authenticate(payload)).pipe(
        tap(({ token }) => tokenManager.setAuthToken(token!)),
        map(({ token }) => jwtDecode<TokenPayload>(token!)),
        map(({ nameid }) => AuthSignInActions.signInSuccess({ currentUserId: nameid }))
      )
    )
  );

export const signInSuccessEpic: AppEpic = action$ =>
  action$.pipe(filter(AuthSignInActions.signInSuccess.match), mapTo(push("todos")));
