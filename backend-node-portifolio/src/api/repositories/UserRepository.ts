import { ERoles } from "@prisma/client";
import { prisma } from "../data/PrismaClient";
import { User } from "../domain/models/User";
import { IUserRepository } from "./interfaces/IUserRepository";

export class UserRepository implements IUserRepository{
    async FindByRoles(role: ERoles): Promise<User[]> {
        const users: User[] = [];
        const usersDB = await prisma.user.findMany({
            where:{
                Role: role
            }
        })
        if(usersDB){
            return Object.assign(users, usersDB);
        }
        return users;
    }
    async FindByGuid(guid: string): Promise<User> {
        const user = new User()
        const userDB = await prisma.user.findFirst({
            where:{
                Guid: guid
            }
        })
        if (userDB) {
            return Object.assign(new User(), userDB);
        }
        return user;
    }
    async FindIfExistsByGuid(guid: string): Promise<boolean> {
        const userDB = await prisma.user.findFirst({
            where:{
                Guid: guid
            }
        })
        if(userDB){
            return true
        }
        return false
    }
    async FindByName(name: string): Promise<User> {
        const user = new User()
        const userDB = await prisma.user.findFirst({
            where:{
                Name: name
            }
        })
        if(userDB){
            user.Id = userDB.Id;
            user.Guid = userDB.Guid;
            user.Name = userDB.Name;
        }
        return user;
    }
    async FindIfExistsByName(name: string): Promise<boolean> {
        const userDB = await prisma.user.findFirst({
            where:{
                Name: name
            }
        })
        if(userDB){
            return true
        }
        return false
    }
    async Update(Entity: User): Promise<string> {
        await prisma.user.update({
            where:{
                Id: Entity.Id
            },
            data:{
                Name: Entity.Name
            }
        })
        return "Atualizado com sucesso!"
    }
    async ListAll(): Promise<User[]> {
        const users: User[] = [];
        const usersDb = await prisma.user.findMany();
        usersDb.map((userDb, index) =>{
            const user = new User()
            user.Guid = userDb.Guid;
            user.Id = userDb.Id;
            user.Name = userDb.Name;
            users.push(user)
        })
        return users;
    }
    async Save(Entity: User): Promise<string> {
        if(Entity.Id == 0){
            await prisma.user.create({
                data:{
                    Guid: Entity.Guid,
                    Name: Entity.Name,
                    Accounts: {
                        create: Entity.Accounts.map(account => ({
                            Email: account.Email,
                            Provider: account.Provider,
                            SocialId: account.SocialId,
                            Username: account.Username
                        }))
                    }
                },
                include: {
                    Accounts: true
                }
            })
            return "Salvo com sucesso!"
        }else{
            await prisma.user.update({
                where:{
                    Id: Entity.Id
                },
                data:{
                    Name: Entity.Name
                }
            })
            return "Atualizado com sucesso!"
        }
    }
    async GetbyId(Id: number): Promise<User> {
        const userDB = await prisma.user.findUnique({
            where:{
                Id: Id
            }
        })
        const user = new User()
        if(userDB){
            user.Id = userDB.Id;
            user.Guid = userDB.Guid;
            user.Name = userDB.Name;
        }   
        return user;
    }
    async Delete(Entity: User): Promise<string> {
        const result = await prisma.user.delete({
            where:{
                Id: Entity.Id
            }
        })
        return "Deletado com sucesso!";
    }
    async DeleteById(Id: number): Promise<string> {
        const result = await prisma.user.delete({
            where:{
                Id: Id
            }
        })
        return "Deletado com sucesso!";
    }


}