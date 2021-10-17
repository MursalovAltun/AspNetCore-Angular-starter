import { combineEpics } from "redux-observable";
import { AuthEpics, AuthRegistrationEpics, AuthSignInEpics } from "../features/auth/epics";
import { TodosCollectionEpics } from "../features/todos/epics";
import { AppEpic } from "./store";
import { catchError } from "rxjs/operators";

const epics = combineEpics(
  AuthEpics.idleTimoutEpic,
  AuthEpics.logout,
  AuthEpics.idleLogout,
  AuthEpics.meEpic,
  AuthSignInEpics.signInEpic,
  AuthSignInEpics.signInSuccessEpic,
  AuthRegistrationEpics.register,
  TodosCollectionEpics.loadListEpic,
  TodosCollectionEpics.addEpic
);

const rootEpic: AppEpic = (action$, store$, dependencies) =>
  epics(action$, store$, dependencies).pipe(
    catchError((error, source) => {
      console.error(error);
      return source;
    })
  );

export default rootEpic;
