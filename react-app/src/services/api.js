const BASE_URL = import.meta.env.VITE_API_BASE_URL || "https://forzatrack.fly.dev/api";
const API_KEY = import.meta.env.VITE_API_KEY;

const defaultHeaders = {
  "Content-Type": "application/json",
  "X-Api-Key": API_KEY,
};

async function apiFetch(url, options = {}) {
  const response = await fetch(url, {
    ...options,
    headers: { ...defaultHeaders, ...options.headers },
  });

  if (!response.ok) {
    const errorBody = await response.text().catch(() => "");
    throw new Error(errorBody || `Request failed with status ${response.status}`);
  }

  const contentType = response.headers.get("content-type");
  if (contentType && contentType.includes("application/json")) {
    return response.json();
  }

  return null;
}

export async function authenticateUser(email, password) {
  return apiFetch(
    `${BASE_URL}/User/AuthenticateUser?email=${encodeURIComponent(email)}&password=${encodeURIComponent(password)}`
  );
}

export async function createUser(username, email, password) {
  return apiFetch(`${BASE_URL}/User/CreateUser`, {
    method: "POST",
    body: JSON.stringify({ username, email, password }),
  });
}

export async function getRecordsByUserId(id) {
  return apiFetch(`${BASE_URL}/Record/GetRecordsByUserId?id=${id}`);
}

export async function getBuildById(id) {
  return apiFetch(`${BASE_URL}/Build/GetBuildById/${id}`);
}

export async function getCarById(id) {
  return apiFetch(`${BASE_URL}/Car/GetCarById/${id}`);
}

export async function getAllCars() {
  return apiFetch(`${BASE_URL}/Car/GetAllCars`);
}

export async function createRecord(record) {
  return apiFetch(`${BASE_URL}/Record/CreateRecord`, {
    method: "POST",
    body: JSON.stringify(record),
  });
}

export async function setRecordDeleted(recordId) {
  return apiFetch(`${BASE_URL}/Record/SetRecordDeleted/${recordId}`, {
    method: "PUT",
  });
}
