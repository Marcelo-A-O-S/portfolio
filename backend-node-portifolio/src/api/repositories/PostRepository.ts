import { Post } from "../domain/models/Post";
import { IPostRepository } from "./interfaces/IPostRepository";
import { prisma } from "../data/PrismaClient";

export class PostRepository implements IPostRepository{
    Update(Entity: Post): Promise<string> {
        throw new Error("Method not implemented.");
    }
    async ListAll(): Promise<Post[]> {
        throw new Error("Method not implemented.");
    }
    Save(Entity: Post): Promise<string> {
        if(Entity.Id ==  0 ){
            prisma.post.create({
                data:{
                    DateCreate: Entity.DateCreate,
                    Guid: Entity.Guid,
                }
            })
        }
        throw new Error("Method not implemented.");
    }
    GetbyId(Id: number): Promise<Post> {
        throw new Error("Method not implemented.");
    }
    Delete(Entity: Post): Promise<string> {
        throw new Error("Method not implemented.");
    }
    DeleteById(Id: number): Promise<string> {
        throw new Error("Method not implemented.");
    }

}