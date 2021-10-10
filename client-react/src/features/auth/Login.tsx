import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { useAppDispatch } from "../../app/hooks";
import { push } from "connected-react-router";
import { AuthSignInActions } from "./actions";

interface LoginFormProfile {
  email: string;
  password: string;
}

const schema: yup.SchemaOf<LoginFormProfile> = yup.object().shape({
  email: yup.string().required().email(),
  password: yup.string().required(),
});

const Login = () => {
  const dispatch = useAppDispatch();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormProfile>({
    resolver: yupResolver(schema),
  });
  const onSubmit = (data: LoginFormProfile) => {
    dispatch(AuthSignInActions.signIn({ ...data, captchaToken: "test" }));
  };

  const navigateToRegistration = () => {
    dispatch(push("registration"));
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <input type="text" {...register("email")} />
      <p>{errors.email?.message}</p>

      <input type="password" {...register("password")} />
      <p>{errors.password?.message}</p>

      <input type="submit" />

      <button type="button" onClick={navigateToRegistration}>
        Don't have an account yet?
      </button>
    </form>
  );
};

export default Login;
