import "./style.css"
import foto from "../../img/image.png"

export default function Home(){
    return(
        <section className="posicionamento-home">
            <div className="image-apresentacao">
                <img  src={foto} alt="" className="image-home"/>
            </div>
           <div className="apresentacao">
            <h1 className="titulo-apresentacao">Olá, meu nome é Marcelo Augusto <span className="esclamacao">|</span></h1>
            <h2 className="text-apresentacao">Desenvolvedor Full-Stack</h2>
            <p className="text-apresentacao"></p>
           </div>
        </section>
    )
}