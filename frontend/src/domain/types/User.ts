import { SocialAccounts } from "./SocialAccounts"

export type User = {
    id: string,
    name: string,
    email: string,
    role: string,
    createdAt: Date,
    status: string,
    socialAccounts: SocialAccounts[]
}