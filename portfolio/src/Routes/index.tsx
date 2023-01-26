import { BrowserRouter, Route, Router, Routes} from "react-router-dom"
import Header from "../Components/Header"
import Home from "../Pages/Home"
import Sobre from "../Pages/About"

export default function Rotas(){
    return(
    <BrowserRouter>
    <Header/> 
    <Routes >
    <Route index path="/portfolio/" element={<Home/>}/>
    <Route path="/portfolio/About" element={<Sobre/>}/>
    <Route path="#Projetos" element={""}/>
    <Route path="#Contato" element={""}/>
    <Route path="/portfolio/*" element={""}/>
    </Routes>
    </BrowserRouter>
    )
}