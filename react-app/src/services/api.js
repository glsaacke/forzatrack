const BASE_URL = import.meta.env.VITE_API_URL;
const API_KEY = import.meta.env.VITE_API_KEY;

function getAuthHeaders() {
  const token = sessionStorage.getItem("token");
  return {
    'Content-Type': 'application/json',
    'X-Api-Key': API_KEY,
    ...(token ? { 'Authorization': `Bearer ${token}` } : {})
  };
}

export async function authenticateUser(email, password) {
  const response = await fetch(`${BASE_URL}/User/AuthenticateUser`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'X-Api-Key': API_KEY
    },
    body: JSON.stringify({ email, password })
  });

  if (response.status === 429) {
    return { success: false, message: 'Too many attempts, please wait a moment and try again.' };
  }

  const authResponse = await response.json();
  return authResponse;
}

export async function createUser(username, email, password){
  const user = {username: username, email: email, password: password}

  let response = await fetch(`${BASE_URL}/User/CreateUser`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json', 'X-Api-Key': API_KEY},
    body: JSON.stringify(user)
  })

  if (response.status === 429) {
    return { success: false, message: 'Too many attempts, please wait a moment and try again.' };
  }

  const createResponse = await response.json()
  return createResponse
}

export async function getRecordsByUserId(id){
  const response = await fetch(`${BASE_URL}/Record/GetRecordsByUserId?id=${id}`,{
    method: 'GET',
    headers: getAuthHeaders()
  })
  const records = await response.json()
  return records
}

export async function getBuildById(id){
  const response = await fetch(`${BASE_URL}/Build/GetBuildById/${id}`,{
    method: 'GET',
    headers: getAuthHeaders()
  })
  const build = await response.json()
  return build
}

export async function getCarById(id){
  const response = await fetch(`${BASE_URL}/Car/GetCarById/${id}`,{
    method: 'GET',
    headers: getAuthHeaders()
  })
  const car = await response.json()
  return car
}

export async function getAllCars(){
  const response = await fetch(`${BASE_URL}/Car/GetAllCars`,{
    method: 'GET',
    headers: getAuthHeaders()
  })
  const cars = await response.json()
  return cars
}

export async function createRecord(record){

  let response = await fetch(`${BASE_URL}/Record/CreateRecord`, {
    method: 'POST',
    headers: getAuthHeaders(),
    body: JSON.stringify(record)
  })

  const jsonResponse = await response.json()

  return jsonResponse

}

export async function pingServer() {
  await fetch(`${BASE_URL}/health`);
}

export async function setRecordDeleted(recordId) {
  let response = await fetch(`${BASE_URL}/Record/SetRecordDeleted/${recordId}`, {
    method: 'PUT',
    headers: getAuthHeaders()
  })

  return response
}
