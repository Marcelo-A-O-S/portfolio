import { SocialMediaAccount } from "../domain/models/SocialMediaAccount";
import { ISocialMediaRepository } from "../repositories/interfaces/ISocialMediaRepository";
import { SocialMediaRepository } from "../repositories/SocialMediaRepository";
import { ISocialMediaServices } from "./interfaces/ISocialMediaServices";

export class SocialMediaServices implements ISocialMediaServices{
    private socialMediaRepository: ISocialMediaRepository;
    constructor(){
        this.socialMediaRepository =  new SocialMediaRepository()
    }
    async Save(entity: SocialMediaAccount): Promise<string> {
        return await this.socialMediaRepository.Save(entity);
    }
    async GetAll(): Promise<SocialMediaAccount[]> {
        return await this.socialMediaRepository.ListAll();
    }
    async GetbyId(id: number): Promise<SocialMediaAccount> {
        return await this.socialMediaRepository.GetbyId(id);
    }
    async Delete(entity: SocialMediaAccount): Promise<string> {
        return await this.socialMediaRepository.Delete(entity);
    }
    async DeleteById(Id: number): Promise<string> {
        return await this.socialMediaRepository.DeleteById(Id);
    }
    async Update(entity: SocialMediaAccount): Promise<string> {
        return await this.socialMediaRepository.Update(entity);
    }

}