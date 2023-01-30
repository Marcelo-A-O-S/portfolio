import './style.css';
import {Link} from "react-router-dom"
import React, {useState, useEffect , } from 'react';
//import Hamburguer from '../Hamburguer';
export default function Header(){
    const [value, setValue] = useState(true);
    const [width, setWidth] = useState(window.innerWidth);
    const { innerWidth , innerHeight } = window;
    function redimensionamento(){
        var navigation = document.getElementById("navigation");
        if(navigation?.className === "navbar"){
            setValue(!value);
        }
        
        
    }
    useEffect(()=>{
        
        window.addEventListener('resize',()=>{
            var navigation = document.getElementById("navigation");
            if(window.innerWidth >= 572){
                if(navigation?.className === 'navbar'){
                    setValue(!value);   
                }
            }
        })
    })
    return(
        <header>
            <nav id="navigation" className={value? 'navbar-layout':'navbar'}>
                <div className='Logo'>
                    <Link className="Logo-link"to='/portfolio/'>Marcelo Augusto.Dev</Link>
                 </div>
                
                <div id="hamburguer"  className='Hamburguer'  onClick={() => setValue(!value)}>
                        <span className="line burger1"/>
                        <span className="line burger2"/>
                        <span className="line burger3"/>
                </div>
                <ul id="Menuitens"className={value?'Menu':"Menu-dropdown"}>
                    <li className='menu-item'><Link onClick={() => redimensionamento()} className="link" to='/portfolio/'>Home</Link></li>
                    <li className='menu-item'><Link onClick={() => redimensionamento()} className="link" to='/portfolio/About'>Sobre</Link></li>
                    <li className='menu-item'><Link onClick={() => redimensionamento()} className="link" to='/portfolio/Projects'>Projetos</Link></li>
                    <li className='menu-item'><Link onClick={() => redimensionamento()} className="link" to='/portfolio/Contact'>Contato</Link></li>
                </ul>
                
                
                
                
            {/* <input className="checkbox" type="checkbox" name="" id="" />
            <div className='Hamburguer'>
                <span className="line burger1"/>
                <span className="line burger2"/>
                <span className="line burger3"/>
            </div> */}
        </nav>
        </header>
    )
}