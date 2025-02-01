const BASE_URL = "http://localhost:5141/api";

export async function authenticateUser(email, password) {
  const response = await fetch(`${BASE_URL}/User/AuthenticateUser?email=${encodeURIComponent(email)}&password=${encodeURIComponent(password)}`)
  const authResponse = await response.json()
  return authResponse
};

export async function createUser(username, email, password){
  const user = {username: username, email: email, password: password}

  let response = await fetch(`${BASE_URL}/User/CreateUser`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(user)
  })
  
  const createResponse = await response.json()
  return createResponse

}