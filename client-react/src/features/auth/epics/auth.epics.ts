import { AppEpic } from "../../../app/store";
import { filter, map, mapTo, switchMap, switchMapTo, tap } from "rxjs/operators";
import { AuthActions } from "../actions";
import { push } from "connected-react-router";
import { from, fromEvent, merge, timer } from "rxjs";
import * as tokenManager from "../token-manager";
import createApiClient from "../../../app/create-api-client";
import { AccountClient } from "../../../app/api";

export const meEpic: AppEpic = action$ =>
  action$.pipe(
    filter(AuthActions.meRequest.match),
    switchMap(() => from(createApiClient(AccountClient).me()).pipe(map(response => AuthActions.meSuccess(response))))
  );

export const logout: AppEpic = action$ =>
  action$.pipe(
    filter(AuthActions.logout.match),
    tap(() => tokenManager.reset()),
    mapTo(push("login"))
  );

export const idleLogout: AppEpic = action$ =>
  action$.pipe(filter(AuthActions.idleTimeout.match), mapTo(AuthActions.logout()));

const clicks$ = fromEvent(document, "click");
const keys$ = fromEvent(document, "keydown");
const mouse$ = fromEvent(document, "mousemove");

export const idleTimoutEpic = () =>
  merge(clicks$, mouse$, keys$).pipe(
    switchMapTo(timer(5 * 60 * 1000)),
    filter(() => !!tokenManager.getAuthToken()),
    mapTo(AuthActions.idleTimeout())
  );
