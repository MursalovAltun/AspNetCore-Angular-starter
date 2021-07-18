import httpClient from "./http-client";
import { AxiosInstance } from "axios";

const createApiClient = <T>(service: { new(baseUrl?: string, instance?: AxiosInstance): T }): T => {
  return new service(process.env.REACT_APP_API_URL, httpClient);
};

export default createApiClient;
