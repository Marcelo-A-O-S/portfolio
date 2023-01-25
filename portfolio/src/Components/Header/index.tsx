import './style.css';
import React, {useState, useEffect } from 'react';
//import Hamburguer from '../Hamburguer';
export default function Header(){
    const [value, setValue] = useState(true);
    const [width, setWidth] = useState(window.innerWidth);
    const { innerWidth , innerHeight } = window;
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
                    <a className="Logo-link" href='#'>Marcelo Augusto.Dev</a>
                 </div>
                
                <div id="hamburguer"  className='Hamburguer'  onClick={() => setValue(!value)}>
                        <span className="line burger1"/>
                        <span className="line burger2"/>
                        <span className="line burger3"/>
                </div>
                <ul id="Menuitens"className={value?'Menu':"Menu-dropdown"}>
                    <li className='menu-item'><a className="link" href='/portfolio/'>Home</a></li>
                    <li className='menu-item'><a className="link" href='#'>Sobre</a></li>
                    <li className='menu-item'><a className="link" href='#'>Projetos</a></li>
                    <li className='menu-item'><a className="link" href='#'>Contato</a></li>
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