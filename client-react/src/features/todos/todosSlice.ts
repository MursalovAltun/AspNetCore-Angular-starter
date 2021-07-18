import { createAsyncThunk, createEntityAdapter, createSlice, EntityState } from "@reduxjs/toolkit";
import { ITodoItemDto, TodoItemsClient } from "../../app/api";
import createApiClient from "../../app/create-api-client";
import { RootState } from "../../app/store";

const name = "todos";

export const loadList = createAsyncThunk<ITodoItemDto[]>(`${name}/loadList`, async () => {
  const todos = await createApiClient(TodoItemsClient).get();
  return todos.map(todo => ({ ...todo }));
})

const adapter = createEntityAdapter<ITodoItemDto>({
  selectId: (todo) => todo.id,
  sortComparer: (a, b) => a.lastModified.getTime() - b.lastModified.getTime()
});

const todosSlice = createSlice({
  name,
  initialState: adapter.getInitialState(),
  reducers: {},
  extraReducers: builder => {
    builder.addCase(loadList.fulfilled, (state: EntityState<ITodoItemDto>, { payload }) => adapter.upsertMany(state, payload))
  }
});

export const { selectAll: getAllTodos } = adapter.getSelectors<RootState>(state => state.todos);

export default todosSlice.reducer;
