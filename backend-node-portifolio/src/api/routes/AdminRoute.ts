import { Router } from "express"
import { AuthController } from "../controllers/AuthController";
import { UserServices } from "../services/UserServices";
import { AdminController } from "../controllers/AdminController";
/**
 * @swagger
 * /admin/check-admin:
 *   get:
 *     summary: Verificar Admin Registrado
 *     description: Retorna a quantidade de administradores registrados.
 *     responses:
 *       200:
 *         description: Retorna a quantidade de administradores.
 *       500:
 *         description: Erro no servidor.
 */
const AdminRoute = () =>{
    const adminRoute = Router();
    const adminController = new AdminController();
    adminRoute.get('/check-admin', adminController.checkAdmin);
    return adminRoute;
}

export { AdminRoute }