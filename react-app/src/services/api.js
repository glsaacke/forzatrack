// const BASE_URL = "https://forzatrack.fly.dev/api";
const BASE_URL = "http://localhost:5141/api";
const API_KEY = import.meta.env.VITE_API_KEY;


export async function authenticateUser(email, password) {
  const response = await fetch(`${BASE_URL}/User/AuthenticateUser?email=${encodeURIComponent(email)}&password=${encodeURIComponent(password)}`, {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-Api-Key': API_KEY
    }
  });

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
  
  const createResponse = await response.json()
  return createResponse
}

export async function getRecordsByUserId(id){
  const response = await fetch(`${BASE_URL}/Record/GetRecordsByUserId?id=${id}`,{
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-Api-Key': API_KEY
    }
  })
  const records = await response.json()
  return records
}

export async function getBuildById(id){
  const response = await fetch(`${BASE_URL}/Build/GetBuildById/${id}`,{
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-Api-Key': API_KEY
    }
  })
  const build = await response.json()
  return build
}

export async function getCarById(id){
  const response = await fetch(`${BASE_URL}/Car/GetCarById/${id}`,{
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-Api-Key': API_KEY
    }
  })
  const car = await response.json()
  return car
}

export async function getAllCars(){
  const response = await fetch(`${BASE_URL}/Car/GetAllCars`,{
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'X-Api-Key': API_KEY
    }
  })
  const cars = await response.json()
  return cars
}

export async function createRecord(record){

  let response = await fetch(`${BASE_URL}/Record/CreateRecord`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json', 'X-Api-Key': API_KEY },
    body: JSON.stringify(record)
  })

  const jsonResponse = await response.json()

  return jsonResponse

}

export async function setRecordDeleted(recordId) {
  let response = await fetch(`${BASE_URL}/Record/SetRecordDeleted/${recordId}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'X-Api-Key': API_KEY
    }
  })

  return response
}
