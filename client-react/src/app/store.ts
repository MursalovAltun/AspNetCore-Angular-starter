import { AnyAction, combineReducers, configureStore, getDefaultMiddleware } from "@reduxjs/toolkit";
import { createEpicMiddleware, Epic } from "redux-observable";
import { createBrowserHistory } from "history";
import storeLogger from "./store-logger";
import snackbarReducer from "../components/snackbar/snackbarSlice";
import authReducer from "../features/auth/auth.reducer";
import { connectRouter, routerMiddleware } from "connected-react-router";
import todosReducer from "../features/todos/todos.reducer";
import rootEpic from "./epics";
import loaderReducer from "../components/loader/loaderSlice";

export const history = createBrowserHistory();

const reducer = combineReducers({
  router: connectRouter(history),
  auth: authReducer,
  snackbar: snackbarReducer,
  todos: todosReducer,
  loader: loaderReducer,
});

export type AppState = ReturnType<typeof reducer>;
export type AppEpic = Epic<AnyAction, AnyAction, AppState>;
export type AppStore = typeof store;
export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;

const epicMiddleware = createEpicMiddleware<AnyAction, AnyAction, AppState>();

export const store = configureStore({
  reducer,
  middleware: [
    ...getDefaultMiddleware({
      thunk: false,
    }),
    routerMiddleware(history),
    epicMiddleware,
    storeLogger,
  ],
});

epicMiddleware.run(rootEpic);
