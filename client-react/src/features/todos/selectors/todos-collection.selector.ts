import { adapter } from "../todos.reducer";
import { RootState } from "../../../app/store";
import { createSelector } from "@reduxjs/toolkit";

export const { selectAll: getAllTodos } = adapter.getSelectors<RootState>(state => state.todos);

export const getDoneTodos = createSelector(getAllTodos, todos => todos.filter(todo => todo.done));
