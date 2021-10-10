import { createAction } from "@reduxjs/toolkit";
import featureKey from "../feature-key";

export const idleTimeout = createAction(`${featureKey} Idle Timeout`);

export const logout = createAction(`${featureKey} Logout`);
