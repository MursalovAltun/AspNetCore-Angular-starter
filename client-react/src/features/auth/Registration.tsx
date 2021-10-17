import { useAppDispatch } from "../../app/hooks";
import * as yup from "yup";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { push } from "connected-react-router";
import { AuthRegistrationActions } from "./actions";
import createApiClient from "../../app/create-api-client";
import { AccountClient } from "../../app/api";
import { Button, IconButton, InputAdornment, Paper, Typography } from "@mui/material";
import { FormInputText } from "../../components/controls/FormInputText";
import VisibilityOff from "@mui/icons-material/VisibilityOff";
import Visibility from "@mui/icons-material/Visibility";
import { useState } from "react";

interface RegistrationFormProfile {
  email: string;
  password: string;
  confirmPassword: string;
  firstName: string;
  lastName: string;
}

const schema: yup.SchemaOf<RegistrationFormProfile> = yup.object().shape({
  firstName: yup.string().required(),
  lastName: yup.string().required(),
  email: yup
    .string()
    .required()
    .email()
    .test(
      "duplicate",
      "This email is already taken",
      async email => !(await createApiClient(AccountClient).emailIsTaken(email))
    ),
  password: yup.string().required(),
  confirmPassword: yup
    .string()
    .required()
    .oneOf([yup.ref("password"), null], "Passwords must match"),
});

const Registration = () => {
  const dispatch = useAppDispatch();

  const { handleSubmit, control } = useForm<RegistrationFormProfile>({
    resolver: yupResolver(schema),
  });

  const onSubmit = (data: RegistrationFormProfile) => {
    dispatch(
      AuthRegistrationActions.register({
        ...data,
        captchaToken: "test",
      })
    );
  };

  const navigateToLogin = () => {
    console.log("awdawd");
    dispatch(push("login"));
  };

  const [showPassword, setShowPassword] = useState(false);

  const showPasswordHandler = () => {
    setShowPassword(!showPassword);
  };

  return (
    <Paper
      style={{
        display: "grid",
        gridRowGap: "20px",
        padding: "20px",
        margin: "10px 300px",
      }}
    >
      <Typography variant="h6">Registration</Typography>

      <FormInputText name="firstName" control={control} label="First Name" />

      <FormInputText name="lastName" control={control} label="Last Name" />

      <FormInputText name="email" control={control} label="Email" />

      <FormInputText
        name="password"
        type={showPassword ? "text" : "password"}
        control={control}
        label="Password"
        InputProps={{
          endAdornment: (
            <InputAdornment position="end">
              <IconButton aria-label="toggle password visibility" onClick={showPasswordHandler} edge="end">
                {showPassword ? <VisibilityOff /> : <Visibility />}
              </IconButton>
            </InputAdornment>
          ),
        }}
      />

      <FormInputText
        name="confirmPassword"
        type={showPassword ? "text" : "password"}
        control={control}
        label="Confirm Password"
        InputProps={{
          endAdornment: (
            <InputAdornment position="end">
              <IconButton aria-label="toggle password visibility" onClick={showPasswordHandler} edge="end">
                {showPassword ? <VisibilityOff /> : <Visibility />}
              </IconButton>
            </InputAdornment>
          ),
        }}
      />

      <Button onClick={handleSubmit(onSubmit)} variant={"contained"}>
        Submit
      </Button>

      <Button onClick={navigateToLogin} variant={"outlined"}>
        Already have an account?
      </Button>
    </Paper>
  );
};

export default Registration;
