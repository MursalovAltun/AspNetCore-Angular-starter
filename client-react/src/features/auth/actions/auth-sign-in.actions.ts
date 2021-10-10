import { createAction } from "@reduxjs/toolkit";
import featureKey from "../feature-key";
import { AuthenticateRequest } from "../../../app/api";

export const signIn = createAction<AuthenticateRequest>(`${featureKey} SIGN IN`);

export const signInSuccess = createAction<{ currentUserId: string }>(`${featureKey} SIGN IN SUCCESS`);
