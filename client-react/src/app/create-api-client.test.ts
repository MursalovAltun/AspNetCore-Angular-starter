import createApiClient from "./create-api-client";
import { AccountClient } from "./api";

describe("createApiClient", () => {
  test("should create a client from a type", () => {
    expect(createApiClient(AccountClient)).toBeInstanceOf(AccountClient);
  });
});
