import { useAppDispatch, useAppSelector } from "../../app/hooks";
import * as fromTodos from "./todosSlice";
import { useEffect } from "react";

const TodosList = () => {
  const todos = useAppSelector(fromTodos.getAllTodos);
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(fromTodos.loadList());
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
