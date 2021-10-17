import { createAction } from "@reduxjs/toolkit";
import featureKey from "../feature-key";
import { UserDto } from "../../../app/api";

export const meRequest = createAction(`${featureKey} Me Request`);

export const meSuccess = createAction<UserDto>(`${featureKey} Me Success`);

export const idleTimeout = createAction(`${featureKey} Idle Timeout`);

export const logout = createAction(`${featureKey} Logout`);
