export class SocialMediaAccountDTO{
    public Id: number;
    public Provider: string;
    public SocialId: string;
    public Email: string;
    public Username: string;
    constructor(){
        this.Id = 0;
        this.Provider = "";
        this.SocialId = "";
        this.Email = "";
        this.Username = "";
    }
}