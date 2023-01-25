import { BrowserRouter, Route, Router, Routes} from "react-router-dom"


export default function Rotas(){
    return(
    <BrowserRouter>
    <Routes >
    <Route index path="#" element={""}/>
    <Route path="#Sobre" element={""}/>
    <Route path="#Projetos" element={""}/>
    <Route path="#Contato" element={""}/>
    <Route path="*" element={""}/>
    </Routes>
    </BrowserRouter>
    )
}