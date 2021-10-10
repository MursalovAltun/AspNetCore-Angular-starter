import { AppEpic } from "../../../app/store";
import { filter, mapTo, switchMapTo, tap } from "rxjs/operators";
import { AuthActions } from "../actions";
import { push } from "connected-react-router";
import { fromEvent, merge, timer } from "rxjs";
import * as tokenManager from "../token-manager";

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
