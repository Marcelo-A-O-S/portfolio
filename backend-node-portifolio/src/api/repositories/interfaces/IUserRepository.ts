
import { ERoles } from "@prisma/client";
import { IGenerics } from "../../data/interface/IGenerics";
import { User } from "../../domain/models/User";

export interface IUserRepository extends IGenerics<User>{
    FindByName(name: string): Promise<User>;
    FindIfExistsByName(name:string): Promise<boolean>;
    FindByGuid(guid: string): Promise<User>;
    FindIfExistsByGuid(guid: string): Promise<boolean>;
    FindByRoles(role: ERoles): Promise<User[]>;
    
}