import { createEntityAdapter, createReducer, EntityState } from "@reduxjs/toolkit";
import { TodoItemDto } from "../../app/api";
import { TodosCollectionActions } from "./actions";

interface IState extends EntityState<TodoItemDto> {}

export const adapter = createEntityAdapter<TodoItemDto>({
  selectId: todo => todo.id,
  sortComparer: (a, b) => new Date(a.lastModified).getTime() - new Date(b.lastModified).getTime(),
});

const todosReducer = createReducer(adapter.getInitialState(), builder => {
  builder.addCase(TodosCollectionActions.loadListSuccess, (state: IState, { payload }) =>
    adapter.addMany(state, payload)
  );
  builder.addCase(TodosCollectionActions.addSuccess, (state: IState, { payload }) => adapter.addOne(state, payload));
});

export default todosReducer;
