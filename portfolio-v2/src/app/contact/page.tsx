"use client"
import { useState } from "react";
import SendEmail from "@emailjs/browser"
export default function ContactPage() {
    const [nome, setNome] = useState("")
    const [email, setEmail] = useState("")
    const [message, setMessage] = useState("")
    var publicKey = process.env.NEXT_PUBLIC_APIKEY!;
    var serviceId = process.env.NEXT_PUBLIC_SERVICEID!;
    var templateId = process.env.NEXT_PUBLIC_TEMPLATEID!;
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
        alert("Olá, muito obrigado pelo envio, assim que possivel, retornarei o contato \n Tenha um ótimo dia!")
        setNome("");
        setEmail("");
        setMessage("");
    }
    return (
        <>
            <div className="flex min-h-screen items-center justify-start bg-dark ">
                <div className="mx-auto w-full max-w-lg p-6">
                    <h1 className="text-4xl font-medium">Contate-me:</h1>
                    <p className="mt-3">Caso queira entrar em contato comigo para tratar de negócios:</p>
                    <form onSubmit={enviarMensagem} className="mt-10">
                        <div className="grid gap-6 sm:grid-cols-2">
                            <div className="relative z-0">
                                <input type="text" name="name" value={nome} onChange={(event)=> setNome(event.target.value)} className="peer block w-full appearance-none border-0 border-b border-gray-500 bg-transparent py-2.5 px-0 text-sm text-gray-900 focus:border-blue-600 focus:outline-none focus:ring-0" placeholder=" " />
                                <label className="absolute top-3 -z-10 origin-[0] -translate-y-6 scale-75 transform text-sm text-gray-500 duration-300 peer-placeholder-shown:translate-y-0 peer-placeholder-shown:scale-100 peer-focus:left-0 peer-focus:-translate-y-6 peer-focus:scale-75 peer-focus:text-blue-600 peer-focus:dark:text-blue-500">Digite seu nome</label>
                            </div>
                            <div className="relative z-0">
                                <input type="text" name="email" value={email} onChange={(event)=>setEmail(event.target.value)} className="peer block w-full appearance-none border-0 border-b border-gray-500 bg-transparent py-2.5 px-0 text-sm text-gray-900 focus:border-blue-600 focus:outline-none focus:ring-0" placeholder=" " />
                                <label className="absolute top-3 -z-10 origin-[0] -translate-y-6 scale-75 transform text-sm text-gray-500 duration-300 peer-placeholder-shown:translate-y-0 peer-placeholder-shown:scale-100 peer-focus:left-0 peer-focus:-translate-y-6 peer-focus:scale-75 peer-focus:text-blue-600 peer-focus:dark:text-blue-500">Digite seu email</label>
                            </div>
                            <div className="relative z-0 col-span-2">
                                <textarea name="message" value={message} onChange={(event)=>setMessage(event.target.value)}  className="peer block w-full appearance-none border-0 border-b border-gray-500 bg-transparent py-2.5 px-0 text-sm text-gray-900 focus:border-blue-600 focus:outline-none focus:ring-0" placeholder=" "></textarea>
                                <label className="absolute top-3 -z-10 origin-[0] -translate-y-6 scale-75 transform text-sm text-gray-500 duration-300 peer-placeholder-shown:translate-y-0 peer-placeholder-shown:scale-100 peer-focus:left-0 peer-focus:-translate-y-6 peer-focus:scale-75 peer-focus:text-blue-600 peer-focus:dark:text-blue-500">Sua mensagem</label>
                            </div>
                        </div>
                        <button type="submit" className="mt-5 rounded-md bg-purple-600 px-10 py-2 text-white">Enviar</button>
                    </form>
                </div>
            </div>
        </>)
}