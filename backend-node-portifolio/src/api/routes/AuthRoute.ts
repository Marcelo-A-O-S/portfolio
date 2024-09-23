import { Router } from "express"
import { AuthController } from "../controllers/AuthController";
import { UserServices } from "../services/UserServices";
/**
 * @swagger
 * /auth/login:
 *   post:
 *     summary: Logar um usuário
 *     description: Retorna um token de autenticação.
 *     responses:
 *       200:
 *         description: Usuário autenticado com sucesso.
 *       401:
 *         description: Credenciais inválidas.
 */
const AuthRoute = () =>{
    const authRoute = Router();
    const authController = new AuthController();
    authRoute.post('/login', authController.login);
    return authRoute;
}

export { AuthRoute }