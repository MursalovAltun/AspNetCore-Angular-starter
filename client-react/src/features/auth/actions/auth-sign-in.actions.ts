import { createAction } from "@reduxjs/toolkit";
import featureKey from "../feature-key";
import { IAuthenticateRequest } from "../../../app/api";

export const signIn = createAction<IAuthenticateRequest>(`${featureKey} SIGN IN`);

export const signInSuccess = createAction<{ currentUserId: string }>(`${featureKey} SIGN IN SUCCESS`);
