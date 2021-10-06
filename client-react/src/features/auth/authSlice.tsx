import { createAction, createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import {
  AccountClient,
  AccountCreateRequest,
  AuthClient,
  AuthenticateRequest,
  IAccountCreateRequest,
  IAuthenticateRequest,
  IUserDto,
} from "../../app/api";
import createApiClient from "../../app/create-api-client";
import * as tokenManager from "./token-manager";
import jwtDecode from "jwt-decode";
import { push } from "connected-react-router";

const name = "auth";

interface AuthState {
  firstName?: string;
  lastName?: string;
  currentUserId?: string;
}

interface TokenPayload {
  nameid: string;
  nbf: number;
  exp: number;
  iat: number;
}

const initialState: AuthState = {
  firstName: undefined,
  lastName: undefined,
  currentUserId: undefined,
};

export const authenticate = createAsyncThunk<string, IAuthenticateRequest>(
  `${name}/authenticate`,
  async (request, { dispatch }) => {
    const result = await createApiClient(AuthClient).authenticate(new AuthenticateRequest(request));

    const token = result.token!;
    tokenManager.setAuthToken(token);

    const { nameid } = jwtDecode<TokenPayload>(token);

    dispatch(push("/todos"));

    return nameid;
  }
);

export const register = createAsyncThunk<string, IAccountCreateRequest>(
  `${name}/register`,
  async (request, { dispatch }) => {
    const result = await createApiClient(AccountClient).create(new AccountCreateRequest(request));

    const token = result.token!;
    tokenManager.setAuthToken(token);

    const { nameid } = jwtDecode<TokenPayload>(token);

    dispatch(push("/todos"));

    return nameid;
  }
);

export const me = createAsyncThunk<IUserDto>(`${name}/me`, async () => {
  return await createApiClient(AccountClient).me();
});

export const logout = createAction(`${name}/logout`);

const slice = createSlice({
  name,
  initialState,
  reducers: {},
  extraReducers: builder => {
    builder.addCase(authenticate.fulfilled, (state: AuthState, { payload }) => {
      state.currentUserId = payload;
    });
    builder.addCase(register.fulfilled, (state: AuthState, { payload }) => {
      state.currentUserId = payload;
    });
    builder.addCase(me.fulfilled, (state: AuthState, { payload }) => {
      state.firstName = payload.firstName;
      state.lastName = payload.lastName;
    });
    builder.addCase(logout, (state: AuthState) => {
      state.currentUserId = undefined;
      state.lastName = undefined;
      state.firstName = undefined;
    });
  },
});

export default slice.reducer;
