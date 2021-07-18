import axios, { AxiosError } from "axios";
import { AppDispatch } from "./store";
import { showSnackbar } from "../components/snackbarSlice";
import * as tokenManager from "../features/auth/token-manager";

const httpClient = axios.create();

export const setupInterceptors = (dispatch: AppDispatch) => {
  httpClient.interceptors.request.use(request => {
    const authToken = tokenManager.getAuthToken();
    const authHeader = "Authorization";

    request.headers[authHeader] = `Bearer ${authToken}`;

    return request;
  });

  httpClient.interceptors.response.use(response => {
    return response;
  }, (error: AxiosError) => {
    if (error.response?.status === 500) {
      dispatch(showSnackbar({ message: "An unhandled error occured.", type: "error" }));
    }

    if (error.response?.status === 401) {
      // logout
    }

    if (error.response?.status === 400) {
      dispatch(showSnackbar({ message: 'Error', type: "error" }));
    }
  });
}

export default httpClient;
