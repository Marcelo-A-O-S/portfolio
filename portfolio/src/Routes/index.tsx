import { BrowserRouter, Route, Router, Routes} from "react-router-dom"
import Header from "../Components/Header"
import Home from "../Pages/Home"
import Sobre from "../Pages/About"
import Projetos from "../Pages/Projects"
import Contato from "../Pages/Contact"
export default function Rotas(){
    return(
    <BrowserRouter>
    <Header/> 
    <Routes >
    <Route index path="/portfolio/" element={<Home/>}/>
    <Route path="/portfolio/About" element={<Sobre/>}/>
    <Route path="/portfolio/Projects" element={<Projetos/>}/>
    <Route path="/portfolio/Contact" element={<Contato/>}/>
    <Route path="/portfolio/*" element={<Home/>}/>
    <Route path="/*" element={<Home/>}/>
    </Routes>
    </BrowserRouter>
    )
}