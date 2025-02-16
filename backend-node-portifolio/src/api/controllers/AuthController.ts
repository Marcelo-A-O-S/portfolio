import { Request, Response } from "express";
import { IAuthController } from "./interfaces/IAuthController";
import { loginSchema } from "./schemas/AuthSchemas";
import { UserServices } from "../services/UserServices";
import { User } from "../domain/models/User";
import { SocialMediaAccount } from "../domain/models/SocialMediaAccount";
import { IUserServices } from "../services/interfaces/IUserServices";

export class AuthController implements IAuthController{
    async login(req: Request, res: Response): Promise<Response> {
        const resultSchema = await loginSchema.safeParseAsync(req.body)
        if(resultSchema.success){
            const userServices = new UserServices()
            const user = new User()
            const userDTO = resultSchema.data
            console.log(userDTO)
            user.generateGuid()
            user.Id = userDTO.Id
            user.Name = userDTO.Name
            userDTO.Accounts?.map((socialMedia)=>{
                const account = new SocialMediaAccount()
                account.Email = socialMedia.Email
                account.Provider = socialMedia.Provider
                account.SocialId = socialMedia.SocialId
                account.Username = socialMedia.Username
                user.Accounts.push(account)
                
            })
            const result = await userServices.Save(user);
            return res.status(200).json(result)
        }
        return res.status(401).json(resultSchema.error)
    }
    
}