import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { useEffect } from "react";
import { TodosCollectionActions } from "./actions";
import { TodosCollectionSelectors } from "./selectors";
import TodoForm from "./TodoForm";
import createApiClient from "../../app/create-api-client";
import { PublicKeyCredentialType, WebauthnClient } from "../../app/api";
import { TextHelpers } from "../../helpers";
import { Button } from "@mui/material";

const TodosList = () => {
  const todos = useAppSelector(TodosCollectionSelectors.getAllTodos);
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(TodosCollectionActions.loadList());
  }, [dispatch]);

  const register = async () => {
    const client = createApiClient(WebauthnClient);
    const options = await client.getRegisterOptions();
    console.log(options);
    if (navigator.credentials) {
      const newCredentialInfo = (await navigator.credentials.create({
        publicKey: {
          pubKeyCredParams: options.pubKeyCredParams!.map(param => ({
            alg: param.alg,
            type: param.type,
          })),
          attestation: options.attestation,
          rp: { id: options.rp!.id, name: options.rp!.name! },
          user: {
            name: options.user?.name,
            id: TextHelpers.coerceToArrayBuffer(options.user.id),
            displayName: options.user.displayName!,
          },
          timeout: options.timeout,
          excludeCredentials: options.excludeCredentials.map(cred => ({
            id: TextHelpers.coerceToArrayBuffer(cred.id),
            type: cred.type,
          })),
          extensions: options.extensions,
          authenticatorSelection: options.authenticatorSelection,
          challenge: TextHelpers.coerceToArrayBuffer(options.challenge),
        },
      })) as PublicKeyCredential;
      console.log("SUCCESS", newCredentialInfo);
      const resp = await client.validateRegister({
        authenticatorName: "example",
        attestationRawResponse: {
          id: newCredentialInfo.id,
          rawId: TextHelpers.coerceToBase64Url(new Uint8Array(newCredentialInfo.rawId)),
          response: {
            attestationObject: TextHelpers.coerceToBase64Url(
              new Uint8Array((newCredentialInfo.response as AuthenticatorAttestationResponse).attestationObject)
            ),
            clientDataJson: TextHelpers.coerceToBase64Url(
              new Uint8Array((newCredentialInfo.response as AuthenticatorAttestationResponse).clientDataJSON)
            ),
          },
          type: newCredentialInfo.type as PublicKeyCredentialType,
          extensions: newCredentialInfo.getClientExtensionResults() as any,
        },
      });

      console.log(resp);
    }
  };

  return (
    <div>
      <Button onClick={register} variant={"contained"}>
        Register fingerprint
      </Button>
      <TodoForm></TodoForm>
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
