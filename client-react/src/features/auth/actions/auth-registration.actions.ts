import { createAction } from "@reduxjs/toolkit";
import { IAccountCreateRequest } from "../../../app/api";
import featureKey from "../feature-key";

export const register = createAction<IAccountCreateRequest>(`${featureKey} REGISTER`);

export const registerSuccess = createAction<{ currentUserId: string }>(`${featureKey} REGISTER`);
