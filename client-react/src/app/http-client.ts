import axios, { AxiosError, AxiosRequestConfig } from "axios";
import { AppDispatch } from "./store";
import { showSnackbar } from "../components/snackbar/snackbarSlice";
import copy from "fast-copy";
import * as tokenManager from "../features/auth/token-manager";
import { AuthActions } from "../features/auth/actions";
import * as fromLoader from "../components/loader/loaderSlice";

export const setAuthHeaderForRequest = (request: AxiosRequestConfig): AxiosRequestConfig => {
  const authToken = tokenManager.getAuthToken();

  request = copy(request);

  request.headers.Authorization = `Bearer ${authToken}`;

  return request;
};

export const onResponseRejectedHandler = (error: AxiosError, dispatch: AppDispatch) => {
  if (!error.response || error.response?.status === 500) {
    dispatch(
      showSnackbar({
        message: "An unhandled error occurred.",
        type: "error",
      })
    );
  }

  if (error.response?.status === 401) {
    dispatch(AuthActions.logout());
  }

  if (error.response?.status === 400) {
    dispatch(showSnackbar({ message: "Error", type: "error" }));
  }
};

const httpClient = axios.create({
  transformResponse: data => data,
});

export const setupInterceptors = (dispatch: AppDispatch) => {
  httpClient.interceptors.request.use(
    request => {
      dispatch(fromLoader.toggleLoader());

      return setAuthHeaderForRequest(request);
    },
    () => {
      dispatch(fromLoader.toggleLoader());
    }
  );

  httpClient.interceptors.response.use(
    response => {
      dispatch(fromLoader.toggleLoader());

      return response;
    },
    (error: AxiosError) => {
      onResponseRejectedHandler(error, dispatch);

      dispatch(fromLoader.toggleLoader());

      return Promise.reject(error);
    }
  );
};

export default httpClient;
