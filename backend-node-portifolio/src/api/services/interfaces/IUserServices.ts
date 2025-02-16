import { User } from "../../domain/models/User";
import { IServices } from "./IServices";
import { ERoles } from "@prisma/client";
export interface IUserServices extends IServices<User>{ 
    FindByRole(role: ERoles): Promise<User[]>
}