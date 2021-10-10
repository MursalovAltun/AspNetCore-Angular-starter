import { AppEpic } from "../../../app/store";
import { filter, map, switchMap } from "rxjs/operators";
import { TodosCollectionActions } from "../actions";
import createApiClient from "../../../app/create-api-client";
import { ITodoItemDto, TodoItemsClient } from "../../../app/api";
import { from } from "rxjs";

export const loadListEpic: AppEpic = action$ =>
  action$.pipe(
    filter(TodosCollectionActions.loadList.match),
    switchMap(() =>
      from(createApiClient(TodoItemsClient).get()).pipe(
        map(response => TodosCollectionActions.loadListSuccess(response as ITodoItemDto[]))
      )
    )
  );
