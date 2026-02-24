export class SocialAccount {
    id: string;
    provider: "google" | "github" | "linkedin";
    providerId:string;
    username: string;
    profileUrl: string;
    linkedAt:string;
    constructor(){
        this.id = "";
        this.providerId = "";
        this.username = "";
        this.profileUrl = "";
        this.linkedAt = "";
        this.provider = "google"
    }
}