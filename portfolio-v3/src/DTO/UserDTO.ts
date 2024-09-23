import { SocialMediaAccountDTO } from "./SocialMediaAccountDTO";
export class UserDTO{
    public Id: number;
    public Guid: string;
    public Name: string;
    public Accounts: SocialMediaAccountDTO[];
    constructor(){
        this.Id = 0;
        this.Guid = "";
        this.Name = "";
        this.Accounts = Array<SocialMediaAccountDTO>();
    }
}