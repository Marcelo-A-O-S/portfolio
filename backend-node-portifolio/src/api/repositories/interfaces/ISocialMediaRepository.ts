
import { IGenerics } from "../../data/interface/IGenerics";
import { SocialMediaAccount } from "../../domain/models/SocialMediaAccount";

export interface ISocialMediaRepository extends IGenerics<SocialMediaAccount>{
    VerifyIfExistsSocialMediaByEmail(email: string): Promise<boolean>;
    FindSocialMediaByEmail(email: string): Promise<SocialMediaAccount>;
    VerifyIfExistsSocialMediaBySocialId(socialId: string): Promise<boolean>;
    FindSocialMediaBySocialId(socialId:string):Promise<SocialMediaAccount>;

}