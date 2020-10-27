import axios from "axios";

const api = axios.create({
  baseURL: "http://localhost:1675/api/"
});

export default api;