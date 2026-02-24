import { LoginRequest } from "@/domain/dtos/LoginRequest";
const host = process.env.BACKEND_SERVER!;
export const loginOAuth = async (loginRequest: LoginRequest) =>{
    const response = await fetch(`${host}/api/auth/login`,{
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(loginRequest),
    });
    if(!response.ok) throw new Error(`Èrror: ${response.status}`);
    const data = await response.json();
    return data;
}
export const refreshAsync = async (userId:string,refreshToken:string, deviceId: string) => {
    const response = await fetch(`${host}/api/auth/refreshToken`,{
        method: "POST",
        headers: {
            "Content-Type":"application/json"
        },
        body: JSON.stringify({
            userId,
            deviceId,
            refreshToken
        })
    })
    return response;
}
export const logout = async (userId:string, deviceId:string) =>{
    const response = await fetch(`${host}/api/auth/logout`,{
        method: "POST",
        headers: {
            "Content-type":"application/json"
        },
        body: JSON.stringify({
            userId,
            deviceId
        })
    })
    return response;
}