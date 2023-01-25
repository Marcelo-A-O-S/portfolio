import { BrowserRouter, Route, Router, Routes} from "react-router-dom"
import Header from "../Components/Header"
import Home from "../Pages/Home"


export default function Rotas(){
    return(
    <BrowserRouter>
    <Header/> 
    <Routes >
    <Route index path="/portfolio/" element={<Home/>}/>
    <Route path="#Sobre" element={""}/>
    <Route path="#Projetos" element={""}/>
    <Route path="#Contato" element={""}/>
    <Route path="/portfolio/*" element={""}/>
    </Routes>
    </BrowserRouter>
    )
}