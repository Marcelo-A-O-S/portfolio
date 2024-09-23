import { User } from "./User";

export class SocialMediaAccount{
    public Id: number;
    public Provider: string;
    public SocialId: string;
    public Email: string;
    public Username: string;
    public UserId : number;
    public User: User;
    constructor(){
        this.Id = 0;
        this.Provider = "";
        this.SocialId = "";
        this.Email = "";
        this.Username = "";
        this.UserId = 0;
        this.User = new User();
    }
}