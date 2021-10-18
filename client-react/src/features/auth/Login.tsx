import * as yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { useAppDispatch } from "../../app/hooks";
import { push } from "connected-react-router";
import createApiClient from "../../app/create-api-client";
import { PublicKeyCredentialType, WebauthnClient } from "../../app/api";
import { TextHelpers } from "../../helpers";
import { AuthSignInActions } from "./actions";
import { setAuthToken } from "./token-manager";
import { Button, IconButton, InputAdornment, Paper, Typography } from "@mui/material";
import Visibility from "@mui/icons-material/Visibility";
import VisibilityOff from "@mui/icons-material/VisibilityOff";
import { FormInputText } from "../../components/controls/FormInputText";
import { useState } from "react";

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

  const { handleSubmit, control } = useForm<LoginFormProfile>({
    resolver: yupResolver(schema),
  });

  const onSubmit = async (data: LoginFormProfile) => {
    dispatch(AuthSignInActions.signIn({ ...data, captchaToken: "test" }));
  };

  const loginFingerprint = async (data: LoginFormProfile) => {
    const client = createApiClient(WebauthnClient);
    const loginOptions = await client.getLoginOptions(data.email);
    const credReqOptions: PublicKeyCredentialRequestOptions = {
      challenge: TextHelpers.coerceToArrayBuffer(loginOptions.challenge),
      allowCredentials: loginOptions.allowCredentials.map(cred => ({
        type: cred.type,
        id: TextHelpers.coerceToArrayBuffer(cred.id),
      })),
      rpId: loginOptions.rpId,
      timeout: loginOptions.timeout,
      extensions: loginOptions.extensions,
      userVerification: loginOptions.userVerification,
    };
    const creds = (await navigator.credentials.get({ publicKey: credReqOptions })) as PublicKeyCredential;
    const response = creds.response as AuthenticatorAssertionResponse;
    const loginCreds = await client.validateLogin({
      userEmail: data.email,
      assertionRawResponse: {
        id: creds.id,
        rawId: TextHelpers.coerceToBase64Url(new Uint8Array(creds.rawId)),
        response: {
          signature: TextHelpers.coerceToBase64Url(new Uint8Array(response.signature)),
          authenticatorData: TextHelpers.coerceToBase64Url(new Uint8Array(response.authenticatorData)),
          clientDataJson: TextHelpers.coerceToBase64Url(new Uint8Array(response.clientDataJSON)),
        },
        type: creds.type as PublicKeyCredentialType,
        extensions: creds.getClientExtensionResults() as any,
      },
    });

    setAuthToken(loginCreds.token);
    dispatch(push("todos"));
  };

  const navigateToRegistration = () => {
    dispatch(push("registration"));
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
      <Typography variant="h6">Login</Typography>

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

      <Button onClick={handleSubmit(onSubmit)} variant={"contained"}>
        Submit
      </Button>

      <Button onClick={handleSubmit(loginFingerprint)} variant={"outlined"}>
        Using Fingerprint
      </Button>

      <Button onClick={navigateToRegistration} variant={"outlined"}>
        Don't have an account?
      </Button>
    </Paper>
  );
};

export default Login;
