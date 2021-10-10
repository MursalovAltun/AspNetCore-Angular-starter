import { createAction } from "@reduxjs/toolkit";
import featureKey from "../featureKey";
import { TodoItemAddRequest, TodoItemDto } from "../../../app/api";

export const loadList = createAction(`${featureKey} Load List`);

export const loadListSuccess = createAction<TodoItemDto[]>(`${featureKey} Load List Success`);

export const add = createAction<TodoItemAddRequest>(`${featureKey} Add`);

export const addSuccess = createAction<TodoItemDto>(`${featureKey} Add Success`);
