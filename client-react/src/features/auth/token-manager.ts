const authTokenKey = "example-access-token";

export const getAuthToken = (): string | null => localStorage.getItem(authTokenKey)

export const setAuthToken = (token: string): void => {
  if (!token) {
    throw new Error("Token cannot be null");
  }

  localStorage.setItem(authTokenKey, token);
}
