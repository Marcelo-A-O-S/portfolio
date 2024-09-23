import { start } from "./api"
import * as dotenv from 'dotenv'
dotenv.config({
    override: true
})
export interface Config{
    port:number
}
(async ()=>{
    const portEnv: string = process.env.PORT as string
    if (portEnv) {
        const portNumber = parseInt(portEnv);
        if (isNaN(portNumber)) {
            console.error("Porta inválida: deve ser um número");
            return;
        }
        const config: Config = {
            port: portNumber
        };
        await start(config);
    } else {
        console.log("Erro em obter a porta de conexão");
    }
})()