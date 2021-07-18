import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { useAppDispatch } from "../../app/hooks";
import * as fromAuth from "./authSlice";

interface LoginFormProfile {
  email: string;
  password: string;
}

const schema = yup.object().shape({
  email: yup.string().required().email(),
  password: yup.string().required(),
})

const Login = () => {
  const dispatch = useAppDispatch();

  const { register, handleSubmit, formState: { errors } } = useForm<LoginFormProfile>({
    resolver: yupResolver(schema)
  });
  const onSubmit = (data: LoginFormProfile) => {
    dispatch(fromAuth.authenticate({ ...data, captchaToken: "test" }))
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <input type="text" {...register("email")} />
      <p>{errors.email?.message}</p>

      <input type="password" {...register("password")} />
      <p>{errors.password?.message}</p>

      <input type="submit" />
    </form>
  )
}

export default Login;
