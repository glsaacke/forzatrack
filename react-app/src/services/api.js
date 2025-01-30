const BASE_URL = "http://localhost:5141/api";

export async function authenticateUser(email, password) {
    const response = await fetch(`${BASE_URL}/User/AuthenticateUser?email=${encodeURIComponent(email)}&password=${encodeURIComponent(password)}`);
    const authResponse = await response.json();
    return authResponse;
  };