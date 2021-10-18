import { createReducer, createSelector } from "@reduxjs/toolkit";
import { AuthActions, AuthSignInActions } from "./actions";
import { RootState } from "../../app/store";

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
  builder.addCase(AuthActions.meSuccess, (state: AuthState, { payload }) => {
    state.currentUserId = payload.id;
    state.lastName = payload.firstName;
    state.firstName = payload.lastName;
  });
});

const authState = createSelector(
  (state: RootState) => state.auth,
  auth => auth
);

export const getCurrentUserId = createSelector(authState, auth => auth.currentUserId);

export const isAuthenticated = createSelector(getCurrentUserId, userId => {
  return !!userId;
});

export const getFullName = createSelector(authState, auth => {
  return `${auth.firstName} ${auth.lastName}`;
});

export const getFullNameAvatar = createSelector(authState, auth => {
  return `${auth.firstName?.charAt(0).toUpperCase()} ${auth.lastName?.charAt(0).toUpperCase()}`;
});

export default authReducer;
