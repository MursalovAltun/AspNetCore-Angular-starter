import { createReducer } from "@reduxjs/toolkit";
import { AuthActions, AuthSignInActions } from "./actions";

interface AuthState {
  firstName?: string;
  lastName?: string;
  currentUserId?: string;
}

const initialState: AuthState = {
  firstName: undefined,
  lastName: undefined,
  currentUserId: undefined,
};

const authReducer = createReducer(initialState, builder => {
  builder.addCase(AuthSignInActions.signInSuccess, (state: AuthState, { payload }) => {
    state.currentUserId = payload.currentUserId;
  });
  builder.addCase(AuthActions.logout, (state: AuthState) => {
    state.currentUserId = undefined;
    state.lastName = undefined;
    state.firstName = undefined;
  });
});

export default authReducer;
