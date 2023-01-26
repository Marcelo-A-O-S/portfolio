import "./style.css"
import image from "../../img/anime.jpg"

export default function Sobre(){
    return(
        <div>
            <section className="Sobre">
                <div>
                    <img src={image} alt="" className="image-sobre"/>
                </div>
                <div className="apresentacao-sobre">
                    <h1 className="titulo-sobre"><span className="sobre1">Sobre</span> <span className="sobre2">mim</span><span className="esclamacao">|</span></h1>
                    <p className="text-sobre"> Meu nome é Marcelo, sou estudante de programacão e apaixonado pela mesma, estudo diariamente assuntos relacionados a mesma, se tornou um dos meus hobbys, também gosto de animes e séries mas dedico a maior parte do meu tempo a programacão, tenho um vicio em criar aplicacões!</p>
                </div>
            </section>
            <section className="Skills">
                <div className="titulo-skills">
                    <h1 className="titulo-sobre">Skills<span className="esclamacao">|</span></h1>
                </div>
                <div className="carrosel">
                    <div className="card-skill">
                        <img src={image} alt="" className="image-skills"/>
                        <p className="text-sobre">texto teste</p>
                    </div>
                    <div className="card-skill">
                        <img src={image} alt="" className="image-skills"/>
                        <p className="text-sobre">texto teste</p>
                    </div>
                    <div className="card-skill">
                        <img src={image} alt="" className="image-skills"/>
                        <p className="text-sobre">texto teste</p>
                    </div>
                </div>
            </section>
            </div>
        
    )
}