import axios from "axios"
import { getToken } from "./autenticacao"

const api = axios.create({
  baseURL: "http://localhost:1675/api/"
})

api.interceptors.request.use(async config => {
  const token = getToken()
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config;
})

export default api