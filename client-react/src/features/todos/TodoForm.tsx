import * as yup from "yup";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { useAppDispatch } from "../../app/hooks";
import { TodosCollectionActions } from "./actions";

interface TodoFormProfile {
  description: string;
}

const schema: yup.SchemaOf<TodoFormProfile> = yup.object().shape({
  description: yup.string().required(),
});

const TodoForm = () => {
  const dispatch = useAppDispatch();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<TodoFormProfile>({
    resolver: yupResolver(schema),
  });

  const onSubmit = (data: TodoFormProfile) => {
    dispatch(TodosCollectionActions.add({ ...data }));
  };

  return (
    <>
      <h2>Create Todo</h2>
      <form onSubmit={handleSubmit(onSubmit)}>
        <input type="text" {...register("description")} />
        <p>{errors.description?.message}</p>

        <input type="submit" />
      </form>
    </>
  );
};

export default TodoForm;
