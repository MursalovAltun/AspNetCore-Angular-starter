import { AppEpic } from "../../../app/store";
import { filter, map, switchMap } from "rxjs/operators";
import { TodosCollectionActions } from "../actions";
import createApiClient from "../../../app/create-api-client";
import { TodoItemsClient } from "../../../app/api";
import { from } from "rxjs";

export const loadListEpic: AppEpic = action$ =>
  action$.pipe(
    filter(TodosCollectionActions.loadList.match),
    switchMap(() =>
      from(createApiClient(TodoItemsClient).get()).pipe(
        map(response => TodosCollectionActions.loadListSuccess(response))
      )
    )
  );

export const addEpic: AppEpic = action$ =>
  action$.pipe(
    filter(TodosCollectionActions.add.match),
    switchMap(({ payload }) =>
      from(createApiClient(TodoItemsClient).post(payload)).pipe(
        map(response => TodosCollectionActions.addSuccess(response))
      )
    )
  );
