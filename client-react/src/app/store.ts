import { Action, configureStore, ThunkAction } from '@reduxjs/toolkit';
import { createLogger } from 'redux-logger';
import snackbarReducer from "../components/snackbarSlice";
import todosReducer from "../features/todos/todosSlice";
import authReducer from "../features/auth/authSlice";
import { createBrowserHistory } from "history";
import { connectRouter, routerMiddleware } from "connected-react-router";

export const history = createBrowserHistory()

const logger = createLogger({
  duration: true,
  timestamp: false,
  collapsed: true,
  colors: {
    title: () => '#139BFE',
    prevState: () => '#1C5FAF',
    action: () => '#149945',
    nextState: () => '#A47104',
    error: () => '#ff0005',
  },
});

export const store = configureStore({
  reducer: {
    router: connectRouter(history) as any,
    auth: authReducer,
    snackbar: snackbarReducer,
    todos: todosReducer,
  },
  middleware: getDefaultMiddleware => [...getDefaultMiddleware().concat(routerMiddleware(history)).concat(logger)],
});

export type AppStore = typeof store;
export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
export type AppThunk<ReturnType = void> = ThunkAction<ReturnType,
  RootState,
  unknown,
  Action<string>>;
