import { createAction } from "@reduxjs/toolkit";
import featureKey from "../featureKey";
import { ITodoItemDto } from "../../../app/api";

export const loadList = createAction(`${featureKey} Load List`);

export const loadListSuccess = createAction<ITodoItemDto[]>(`${featureKey} Load List Success`);
