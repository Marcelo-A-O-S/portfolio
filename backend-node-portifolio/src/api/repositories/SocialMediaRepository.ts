import { SocialMediaAccount } from "../domain/models/SocialMediaAccount";
import { ISocialMediaRepository } from "./interfaces/ISocialMediaRepository";
import { prisma } from "../data/PrismaClient";
export class SocialMediaRepository implements ISocialMediaRepository{
    async VerifyIfExistsSocialMediaBySocialId(socialId: string): Promise<boolean> {
        const result = await prisma.socialMediaAccount.findFirst({
            where:{
                SocialId: socialId
            }
        })
        if(result){
            return true
        }
        return false
    }
    async FindSocialMediaBySocialId(socialId: string): Promise<SocialMediaAccount> {
        const socialMediaAccount = new SocialMediaAccount()
        const socialMedia = await prisma.socialMediaAccount.findFirst({
            where:{
                SocialId: socialId
            },
        })
        if(socialMedia){
            socialMediaAccount.Id = socialMedia.Id;
            socialMediaAccount.Email = socialMedia.Email;
            socialMediaAccount.Provider = socialMedia.Provider;
            socialMediaAccount.SocialId = socialMedia.SocialId;
            socialMediaAccount.UserId = socialMedia.UserId;
            socialMediaAccount.Username = socialMedia.Username;
            return socialMediaAccount
        }
        return socialMediaAccount
    }
    async VerifyIfExistsSocialMediaByEmail(email: string): Promise<boolean> {
        const result = await prisma.socialMediaAccount.findFirst({
            where:{
                Email: email
            }
        })
        if(result){
            return true
        }
        return false
    }
    async FindSocialMediaByEmail(email: string): Promise<SocialMediaAccount> {
        const socialMediaAccount = new SocialMediaAccount()
        const socialMedia = await prisma.socialMediaAccount.findFirst({
            where:{
                Email: email
            },
        })
        if(socialMedia){
            socialMediaAccount.Id = socialMedia.Id;
            socialMediaAccount.Email = socialMedia.Email;
            socialMediaAccount.Provider = socialMedia.Provider;
            socialMediaAccount.SocialId = socialMedia.SocialId;
            socialMediaAccount.UserId = socialMedia.UserId;
            socialMediaAccount.Username = socialMedia.Username;
            return socialMediaAccount
        }
        return socialMediaAccount
    }
    async Update(Entity: SocialMediaAccount): Promise<string> {
        await prisma.socialMediaAccount.update({
            where:{
                Id: Entity.Id
            },
            data:{
                Email: Entity.Email,
                Provider: Entity.Provider,
                SocialId: Entity.SocialId,
                Username: Entity.Username
            }
        })
        return "Atualzado com sucesso!"
    }
    async ListAll(): Promise<SocialMediaAccount[]> {
        const socialsMedia: SocialMediaAccount[] = []
        const socialsMediaDb = await prisma.socialMediaAccount.findMany()
        if(socialsMedia.length > 0){
            socialsMediaDb.map((socialMediaDb)=>{
                const socialMedia = new SocialMediaAccount()
                socialMedia.Email = socialMediaDb.Email
                socialMedia.Provider = socialMediaDb.Provider
                socialMedia.SocialId = socialMediaDb.SocialId
                socialMedia.Id = socialMediaDb.Id
                socialMedia.Username = socialMediaDb.Username
                socialMedia.UserId = socialMediaDb.UserId
                socialsMedia.push(socialMedia)
            })
        }
        return socialsMedia
    }
    async Save(Entity: SocialMediaAccount): Promise<string> {
        if(Entity.Id == 0){
            await prisma.socialMediaAccount.create({
                data:{
                    Id: Entity.Id,
                    Email: Entity.Email,
                    Provider: Entity.Provider,
                    SocialId: Entity.SocialId,
                    Username: Entity.Username,
                    UserId: Entity.UserId
                }
            })
            return "Salvo com sucesso!"
        }
        await prisma.socialMediaAccount.update({
            where:{
                Id: Entity.Id
            },
            data:{
                Email: Entity.Email,
                Provider: Entity.Provider,
                SocialId: Entity.SocialId,
                Username: Entity.Username
            }
        })
        return "Atualzado com sucesso!"
    }
    async GetbyId(Id: number): Promise<SocialMediaAccount> {
        const socialMediaAccount = new SocialMediaAccount()
        const socialMedia = await prisma.socialMediaAccount.findUnique({
            where:{
                Id: Id
            }
        })
        if(socialMedia){
            socialMediaAccount.Id = socialMedia.Id;
            socialMediaAccount.Email = socialMedia.Email;
            socialMediaAccount.Provider = socialMedia.Provider;
            socialMediaAccount.SocialId = socialMedia.SocialId;
            socialMediaAccount.UserId = socialMedia.UserId;
            socialMediaAccount.Username = socialMedia.Username;
            return socialMediaAccount
        }
        return socialMediaAccount
    }
    async Delete(Entity: SocialMediaAccount): Promise<string> {
        await prisma.socialMediaAccount.delete({
            where:{
                Id: Entity.Id
            }
        })
        return "Deletado com sucesso!"
    }
    async DeleteById(Id: number): Promise<string> {
        await prisma.socialMediaAccount.delete({
            where:{
                Id: Id
            }
        })
        return "Deletado com sucesso!"
    }
}