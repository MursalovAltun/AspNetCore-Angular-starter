import React from 'react';
import './App.css';
import CustomSnackbar from "./components/snackbar";
import Login from "./features/auth/Login";
import { history } from "./app/store";
import { ConnectedRouter } from "connected-react-router";
import { Redirect, Route, Switch } from "react-router";
import TodosList from "./features/todos/TodosList";
import { useAppSelector } from "./app/hooks";
import * as fromAuth from "./features/auth/authSlice";

function App() {
  const isAuthenticated = useAppSelector(fromAuth.isAuthenticated);

  return (
    <ConnectedRouter history={history}>
      <CustomSnackbar/>

      <Switch>
        <Route path="/" exact>
          <Redirect to={{ pathname: '/todos' }}/>
        </Route>

        <Route path="/login" component={Login}/>

        <Route
          path="/todos"
          render={() => isAuthenticated ?
            <TodosList/> :
            <Redirect to="/login"/>
          }/>
      </Switch>
    </ConnectedRouter>
  );
}

export default App;
