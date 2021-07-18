import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { AuthClient, AuthenticateRequest, IAuthenticateRequest } from "../../app/api";
import createApiClient from "../../app/create-api-client";
import * as tokenManager from "./token-manager";
import jwtDecode from "jwt-decode";
import { push } from "connected-react-router";
import { RootState } from "../../app/store";

const name = "auth";

interface AuthState {
  currentUserId?: string;
  isAuthenticated?: boolean;
}

interface TokenPayload {
  nameid: string;
  nbf: number;
  exp: number;
  iat: number;
}

const initialState: AuthState = {
  currentUserId: undefined,
  isAuthenticated: undefined,
};

export const authenticate = createAsyncThunk<string, IAuthenticateRequest>(`${name}/authenticate`, async (request, { dispatch }) => {
  const result = await createApiClient(AuthClient).authenticate(new AuthenticateRequest(request));

  const token = result.token!;
  tokenManager.setAuthToken(token);

  const { nameid } = jwtDecode<TokenPayload>(token);

  dispatch(push("/todos"))

  return nameid;
})

const slice = createSlice({
  name,
  initialState,
  reducers: {},
  extraReducers: builder => {
    builder.addCase(authenticate.fulfilled, (state: AuthState, { payload }) => {
      state.currentUserId = payload;
    })
  }
});

export const isAuthenticated = (state: RootState) => !!state.auth.currentUserId;

export default slice.reducer;
