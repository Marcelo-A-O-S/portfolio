import "./style.css"
import foto from "../../img/image.png"
import Instagram from "../../Icons/instagram.svg"
import Github from "../../Icons/github.svg"
import Linkedin from "../../Icons/Linkedin.svg"
import {Link} from "react-router-dom"

export default function Home(){
    return(
        <section className="posicionamento-home">
            <div className="image-apresentacao">
                <img  src={foto} alt="" className="image-home"/>
            </div>
           <div className="apresentacao">
            <h1 className="titulo-apresentacao">Olá, meu nome é Marcelo Augusto <span className="esclamacao">|</span></h1>
            <h2 className="text-apresentacao">Desenvolvedor Full-Stack</h2>
            <div className="social-media">
                <a target="_blank" className="link-social" href="https://www.linkedin.com/in/marcelo-augusto-dev/"><img className="social-image-linkedin" src={Linkedin}  alt=""/></a>
                <a target="_blank" className="link-social" href="https://github.com/Marcelo-A-O-S"><img className="social-image-github" src={Github} alt=""/></a>
                <a target="_blank" className="link-social" href="https://www.instagram.com/marcelo.augusto1234/"><img className="social-image" src={Instagram} alt=""/></a>                
            </div>
           </div>
        </section>
    )
}