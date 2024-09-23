import express, { Express } from "express";
import bodyParser from 'body-parser';
import cors from 'cors'
import { Config } from "..";
import { AuthRoute } from "./routes/AuthRoute";
import swaggerUi from "swagger-ui-express"
import swaggerJsdoc,{Options} from "swagger-jsdoc"
const options: Options = {
    definition: {
      openapi: '3.0.0',
      info: {
        title: 'Documentation Api Portfólio',
        description:'Documentação relacionada a minha api para meu portfólio.', 
        contact: {
            name: 'Marcelo Augusto de Oliveira Soares',
            email: "marceloaugustooliveirasoares@gmail.com"
        },
        license: {
            name: "Apache 2.0",
            url: "http://www.apache.org/licenses/LICENSE-2.0.html"
        },
        version: '1.0.0',
      },
    },
    apis: [
        './src/api/routes/AuthRoute.ts'
    ], 
  };
const openapiSpecification = swaggerJsdoc(options);
export class App{
    private server: express.Application;
    private port: number;
    constructor(props: Config){
        this.server = express();
        this.port = props.port;
        this.listen(this.port);
        this.middleware();
        this.routes();
        this.server.use('/api-docs',swaggerUi.serve, swaggerUi.setup(openapiSpecification))
    }
    getApp():express.Application{
        return this.server;
    }
    listen(port:number):void{
        this.server.listen(port,()=>{
            console.log(`Escutando na porta: http://localhost:${port}`);
            console.log(`Documentção da aplicação: http://localhost:${port}/api-docs`);
        })
    }
    private middleware(){
        console.log('Configurando middleware...');
        this.server.use(cors());
        this.server.use(bodyParser.json());
        console.log('Middleware configurado!');
    }
    private routes(){
        this.server.use('/auth', AuthRoute());
    }
}