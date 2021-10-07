import { filter, mapTo } from "rxjs/operators";
import * as fromAuth from "./authSlice";
import { push } from "connected-react-router";

// @ts-ignore
export const logoutEpic = action$ => action$.pipe(filter(fromAuth.logout.match), mapTo(push("login")));
