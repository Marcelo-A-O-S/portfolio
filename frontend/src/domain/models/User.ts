export class User {
    id: string;
    email: string;
    name: string;
    image: string;
    createdAt: string;
    constructor(){
        this.id = "";
        this.name = "";
        this.email = "";
        this.image = "";
        this.createdAt = Date.now().toLocaleString()
    }
}