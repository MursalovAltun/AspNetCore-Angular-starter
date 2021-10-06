import { useAppDispatch } from "../../app/hooks";
import * as yup from "yup";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { push } from "connected-react-router";
import * as fromAuth from "./authSlice";

interface RegistrationFormProfile {
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
}

const schema = yup.object().shape({
  firstName: yup.string().required(),
  lastName: yup.string().required(),
  email: yup.string().required().email(),
  password: yup.string().required(),
  confirmPassword: yup
    .string()
    .required()
    .oneOf([yup.ref("password"), null], "Passwords must match"),
});

const Registration = () => {
  const dispatch = useAppDispatch();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegistrationFormProfile>({
    resolver: yupResolver(schema),
  });
  const onSubmit = (data: RegistrationFormProfile) => {
    dispatch(fromAuth.register({ ...data, captchaToken: "test" }));
  };

  const navigateToLogin = () => {
    dispatch(push("login"));
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <input type="text" {...register("firstName")} />
      <p>{errors.firstName?.message}</p>

      <input type="text" {...register("lastName")} />
      <p>{errors.lastName?.message}</p>

      <input type="email" {...register("email")} />
      <p>{errors.email?.message}</p>

      <input type="password" {...register("password")} />
      <p>{errors.password?.message}</p>

      <input type="password" {...register("confirmPassword")} />
      <p>{errors.confirmPassword?.message}</p>

      <input type="submit" />

      <button type="button" onClick={navigateToLogin}>
        Already have an account?
      </button>
    </form>
  );
};

export default Registration;
