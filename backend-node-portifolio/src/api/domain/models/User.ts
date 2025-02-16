import { SocialMediaAccount } from "./SocialMediaAccount";
import { ERoles } from "@prisma/client";
import {v4 as uuid} from "uuid"
export class User{
    public Id: number;
    public Guid: string;
    public Name: string;
    public Role: ERoles;
    public Accounts: Array<SocialMediaAccount>;
    constructor(){
        this.Id = 0;
        this.Guid = "";
        this.Name = "";
        this.Accounts = new Array<SocialMediaAccount>()
        this.Role = ERoles.USER
    }
    generateGuid(){
        this.Guid = uuid();
    }
}