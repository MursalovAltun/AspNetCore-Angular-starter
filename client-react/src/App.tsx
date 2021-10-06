import React, { useEffect } from "react";
import "./App.css";
import CustomSnackbar from "./components/snackbar";
import Login from "./features/auth/Login";
import { history } from "./app/store";
import { ConnectedRouter } from "connected-react-router";
import { Redirect, Route, Switch } from "react-router";
import TodosList from "./features/todos/TodosList";
import * as tokenManager from "./features/auth/token-manager";
import Registration from "./features/auth/Registration";
import { useAppDispatch } from "./app/hooks";
import * as fromAuth from "./features/auth/authSlice";

function App() {
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(fromAuth.me());
  }, [dispatch]);

  const logout = () => {
    dispatch(fromAuth.logout());
  };

  return (
    <ConnectedRouter history={history}>
      <div>
        <button type="button" onClick={logout}>
          Logout
        </button>
      </div>

      <CustomSnackbar />

      <Switch>
        <Route path="/" exact>
          <Redirect to={{ pathname: "/todos" }} />
        </Route>

        <Route path="/login" component={Login} />
        <Route path="/registration" component={Registration} />

        <Route
          path="/todos"
          render={() => (!!tokenManager.getAuthToken() ? <TodosList /> : <Redirect to="/login" />)}
        />
      </Switch>
    </ConnectedRouter>
  );
}

export default App;
