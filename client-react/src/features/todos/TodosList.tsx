import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { useEffect } from "react";
import { TodosCollectionActions } from "./actions";
import { TodosCollectionSelectors } from "./selectors";

const TodosList = () => {
  const todos = useAppSelector(TodosCollectionSelectors.getAllTodos);
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(TodosCollectionActions.loadList());
  }, [dispatch]);

  return (
    <div>
      {(!todos || todos.length < 1) && <div>You have nothing to do. Please create one</div>}
      <ul>
        {todos.map(todo => (
          <li key={todo.id}>{todo.description}</li>
        ))}
      </ul>
    </div>
  );
};

export default TodosList;
