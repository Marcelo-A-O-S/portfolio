import axios from "axios";

const host = process.env.NEXT_BACKEND_API!;
export const apiClient = async() =>{
    const instance = axios.create({
        baseURL: host,
        headers: {
            "Content-Type":"application/json"
        },
    });
    return instance;
}