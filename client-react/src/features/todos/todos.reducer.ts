import { createEntityAdapter, createReducer, EntityState } from "@reduxjs/toolkit";
import { ITodoItemDto } from "../../app/api";
import { TodosCollectionActions } from "./actions";

export const adapter = createEntityAdapter<ITodoItemDto>({
  selectId: todo => todo.id,
  sortComparer: (a, b) => a.lastModified.getTime() - b.lastModified.getTime(),
});

const todosReducer = createReducer(adapter.getInitialState(), builder => {
  builder.addCase(TodosCollectionActions.loadListSuccess, (state: EntityState<ITodoItemDto>, { payload }) =>
    adapter.addMany(state, payload)
  );
});

export default todosReducer;
