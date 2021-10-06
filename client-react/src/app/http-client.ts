import axios, { AxiosError, AxiosRequestConfig } from "axios";
import { AppDispatch } from "./store";
import { showSnackbar } from "../components/snackbarSlice";
import copy from "fast-copy";
import * as tokenManager from "../features/auth/token-manager";
import * as fromAuth from "../features/auth/authSlice";

export const setAuthHeaderForRequest = (request: AxiosRequestConfig): AxiosRequestConfig => {
  const authToken = tokenManager.getAuthToken();

  request = copy(request);

  request.headers.Authorization = `Bearer ${authToken}`;

  return request;
};

export const onResponseRejectedHandler = (error: AxiosError, dispatch: AppDispatch) => {
  if (error.response?.status === 500) {
    dispatch(showSnackbar({ message: "An unhandled error occurred.", type: "error" }));
  }

  if (error.response?.status === 401) {
    dispatch(fromAuth.logout());
  }

  if (error.response?.status === 400) {
    dispatch(showSnackbar({ message: "Error", type: "error" }));
  }
};

const httpClient = axios.create();

export const setupInterceptors = (dispatch: AppDispatch) => {
  httpClient.interceptors.request.use(request => setAuthHeaderForRequest(request));

  httpClient.interceptors.response.use(
    response => {
      return response;
    },
    (error: AxiosError) => {
      onResponseRejectedHandler(error, dispatch);
    }
  );
};

export default httpClient;
