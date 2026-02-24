export type LoginRequest = {
    name?: string,
    username?: string,
    email?: string,
    profileUrl?: string,
    provider: string,
    providerId: string,
    deviceId: string,
    deviceName: string
}