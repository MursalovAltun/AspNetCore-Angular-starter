import React, { useEffect } from "react";
import "./App.css";
import CustomSnackbar from "./components/snackbar/snackbar";
import Login from "./features/auth/Login";
import { history } from "./app/store";
import { ConnectedRouter } from "connected-react-router";
import { Redirect, Route, Switch } from "react-router";
import TodosList from "./features/todos/TodosList";
import * as tokenManager from "./features/auth/token-manager";
import Registration from "./features/auth/Registration";
import { useAppDispatch, useAppSelector } from "./app/hooks";
import { AuthActions } from "./features/auth/actions";
import { Avatar, BottomNavigation, BottomNavigationAction, Button, Paper } from "@mui/material";
import * as fromAuth from "./features/auth/auth.reducer";
import Loader from "./components/loader/loader";

function App() {
  const dispatch = useAppDispatch();
  const isAuthenticated = useAppSelector(fromAuth.isAuthenticated);
  const fullNameAvatar = useAppSelector(fromAuth.getFullNameAvatar);

  useEffect(() => {
    if (!!tokenManager.getAuthToken()) {
      // dispatch(AuthActions.meRequest());
    }
  }, [dispatch]);

  const logout = () => {
    dispatch(AuthActions.logout());
  };

  return (
    <ConnectedRouter history={history}>
      <Loader />

      {isAuthenticated && (
        <Paper
          sx={{
            display: "flex",
            padding: "4px",
            alignItems: "center",
            justifyContent: "flex-end",
          }}
        >
          <Avatar>{fullNameAvatar}</Avatar>
          <Button onClick={logout}>Logout</Button>
        </Paper>
      )}

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

      {isAuthenticated && (
        <Paper sx={{ position: "fixed", bottom: 0, left: 0, right: 0 }} elevation={3}>
          <BottomNavigation showLabels value={"todos"}>
            <BottomNavigationAction label="Todos" value="todos" />
          </BottomNavigation>
        </Paper>
      )}
    </ConnectedRouter>
  );
}

export default App;
