import { Request, Response } from "express";

interface IAuthController {
    login(req:Request, res: Response): Promise<Response>
}
export { IAuthController }