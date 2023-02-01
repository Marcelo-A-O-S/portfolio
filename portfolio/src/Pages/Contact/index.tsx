import "./style.css"
import React, {useState, useEffect } from "react"
import Projeto from "../../Models/Projetos"
import SendEmail from "@emailjs/browser"


export default function Contato(){
    const [nome, setNome] = useState("")
    const [email, setEmail] = useState("")
    const [message, setMessage] = useState("")
    var publicKey = process.env.REACT_APP_KEY!;
    var serviceId = process.env.REACT_APP_SERVICEID!;
    var templateId = process.env.REACT_APP_TEMPLATEID!;
    const enviarMensagem = (event:any) =>{
        event.preventDefault();
        
        const templateParams = {
            from_name:nome,
            email:email,
            message:message
        }
        try {
            
            SendEmail.send(serviceId,templateId,templateParams,publicKey).then((res)=> console.log(`Status de envio: ${res.status}`))
        } catch ( error) {
            console.log(error)
        }
       

        alert(" Olá, muito obrigado pelo envio, assim que possivel, retornarei o contato \n Tenha um ótimo dia!")
        setNome("");
        setEmail("");
        setMessage("");
    }
    return(
        <section className="Contato">
            <div>
                <h1 className="titulo-contato">Contato</h1>
                
            </div>
            <form onSubmit={enviarMensagem} className="Formulario-contato">
                <p className="label-contato">Caso queira entrar em contato comigo para tratar de negócios:</p>
                <div className="area-info">
                    <label className="label-contato">Digite o seu nome abaixo:</label>
                    <input className="input" type={"text"} value={nome} onChange={(event)=> setNome(event.target.value)} placeholder="Digite o nome aqui" required/>
                </div>
                <div className="area-info">
                    <label className="label-contato">Digite o seu email abaixo:</label>
                    <input className="input" type={"email"} value={email} onChange={(event)=>setEmail(event.target.value)} placeholder="Digite o seu email aqui" required/>
                </div>  
                
                <div className="area-info">
                    <label className="label-contato">Digite o assunto abaixo:</label>
                    <textarea className="text-contato" placeholder="Digite aqui a mensagem a ser enviada" value={message} onChange={(event)=>setMessage(event.target.value)} required></textarea>
                </div>
                <div>
                    <button className="btn-contato" type={"submit"}>Enviar</button>
                </div>  
            </form>
        </section>
    )
}