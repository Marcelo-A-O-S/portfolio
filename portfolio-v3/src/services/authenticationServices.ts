import { UserDTO } from '@/DTO/UserDTO';
import https from 'https';
import axios from "axios";
const agent = new https.Agent({
    rejectUnauthorized: false
  });
const api = axios.create({
    baseURL:process.env.NEXT_PUBLIC_HOST,
    httpsAgent: agent,
})
export async function AuthLogin(dataLogin: UserDTO){
    const response = await api.post("/auth/login",dataLogin);
    return response.data
}

export default {
    AuthLogin
}