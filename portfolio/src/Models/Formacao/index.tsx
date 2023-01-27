export default class Formacao{
    nome_instituicao:string;
    nome_curso:string;
    img_instituicao:string;
    data_inicio:string;
    data_termino:string;
    constructor(instituicao:string,curso:string,img:string,datainicio:string,datatermino:string){
        this.nome_instituicao = instituicao;
        this.nome_curso = curso;
        this.img_instituicao = img;
        this.data_inicio = datainicio;
        this.data_termino = datatermino;
    }
}