import { createAction } from "@reduxjs/toolkit";
import featureKey from "../feature-key";
import { AccountCreateRequest } from "../../../app/api";

export const register = createAction<AccountCreateRequest>(`${featureKey} REGISTER`);

export const registerSuccess = createAction<{ currentUserId: string }>(`${featureKey} REGISTER`);
