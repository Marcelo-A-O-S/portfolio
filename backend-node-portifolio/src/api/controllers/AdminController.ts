import { Request, Response } from "express";
import { UserServices } from "../services/UserServices";

export class AdminController{
    async checkAdmin(req: Request, res: Response): Promise<Response>{
        var userservices = new UserServices()
        var users = await userservices.FindByRole("ADMIN")
        return res.json(users.length);
    }
}