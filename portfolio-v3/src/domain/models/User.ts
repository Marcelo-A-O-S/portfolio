import { SocialMediaAccount } from "./SocialMediaAccount";

export class User{
    public Id: number;
    public Guid: string;
    public Name: string;
    public Accounts: Array<SocialMediaAccount>;
    constructor(){
        this.Id = 0;
        this.Guid = "";
        this.Name = "";
        this.Accounts = new Array<SocialMediaAccount>()
    }
}