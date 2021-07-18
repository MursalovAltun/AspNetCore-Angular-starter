import { useAppDispatch, useAppSelector } from "../../app/hooks";
import * as fromTodos from "./todosSlice";
import { useEffect } from "react";

const TodosList = () => {
  const todos = useAppSelector(fromTodos.getAllTodos);
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(fromTodos.loadList());
  }, [dispatch]);

  return(
    <div>
      <ul>
        {todos.map(todo => <li key={todo.id}>{todo.description}</li>)}
      </ul>
    </div>
  )
}

export default TodosList;
