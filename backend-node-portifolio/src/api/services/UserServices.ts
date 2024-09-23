import { User } from "../domain/models/User";
import { UserRepository } from "../repositories/UserRepository";
import { IServices } from "./interfaces/IServices";

export class UserServices implements IServices<User> {
    userRepository: UserRepository;
    constructor() {
        this.userRepository = new UserRepository()
    }
    async Delete(entity: User): Promise<string> {
        return await this.userRepository.Delete(entity);
    }
    async DeleteById(Id: number): Promise<string> {
        return await this.userRepository.DeleteById(Id);
    }
    async Update(entity: User): Promise<string> {
        return await this.userRepository.Update(entity);
    }
    async Save(entity: User): Promise<string> {
        return await this.userRepository.Save(entity);
    }
    async GetAll(): Promise<User[]> {
        return await this.userRepository.ListAll();
    }
    async GetbyId(id: number): Promise<User> {
        return await this.userRepository.GetbyId(id);
    }

}