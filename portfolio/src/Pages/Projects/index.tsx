import "./style.css"
import image from "../../img/anime.jpg"
import React, {useEffect , useState} from "react"
import Projeto from "../../Models/Projetos"

const teste = require("../../Models/Formacao/index")

export default function Projetos(){
    const [projeto, setProjetos] = useState<Projeto[]>([])
    var projetoNome = "";
    useEffect(()=>{
        console.log(teste)
        fetch("./Dados/Projetos/index.json",{
            headers:{
                Accept: "application/json"
            }
         }).then((res)=>res.json()).then((body)=>{
            setProjetos(body.Projetos)
            
         }
        )
        
      renderizarProjetos()
       
    },[])
    function AcessarDetalhe(linkprojeto:string){
        window.open(linkprojeto,"_blank");
        
    }
    function renderizarProjetos(){
        return projeto.map((projeto)=>{
            return <div className="card-projeto">
            <div className="position-image-card">
                <img src={projeto.imagemprojeto} alt="" className="image-card"/>
            </div>
            <div className="posicao-titulo-card">
                <h1 className="titulo-card">{projeto.nomeprojeto}</h1>
            </div>
            <div className="posicao-descricao-card">
                <p className="descricao-card">{projeto.descricaoprojeto}</p>
            </div>
            <div className="posicao-button">
                <button value={projeto.linkprojeto} onClick={()=>AcessarDetalhe(projeto.linkprojeto)} className="button-card">Ver detalhes</button>
            </div>
            
        </div>
        })
    }
    return(
        <section  className="Projetos">
            <div>
                <h1 className="Titulo-projetos">Projetos</h1>
            </div>
            <div className="alinhamento-projetos">
                {renderizarProjetos()}
            </div>
        </section>
    )
}