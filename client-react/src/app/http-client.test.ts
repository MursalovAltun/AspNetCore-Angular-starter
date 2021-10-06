import * as testSut from "./http-client";
import { AxiosRequestConfig } from "axios";
import * as tokenManager from "../features/auth/token-manager";

describe("httpClient", () => {
  test("should set authorization header", () => {
    const testAuthToken = "testauthtoken";

    const request: AxiosRequestConfig = {
      method: "GET",
      url: "http://testendoint.com",
    };

    jest.spyOn(tokenManager, "getAuthToken").mockReturnValue(testAuthToken);

    const expected: AxiosRequestConfig = {
      ...request,
      headers: { Authorization: `Bearer ${testAuthToken}` },
    };

    const actual = testSut.setAuthHeaderForRequest(request);

    expect(actual).toEqual(expected);
  });
});
