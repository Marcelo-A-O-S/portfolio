import './style.css';
import React, {useState } from 'react';
//import Hamburguer from '../Hamburguer';
export default function Header(){
    const [value, setValue] = useState(true);
    
    function mudancaTela(teste:any){
        console.log(teste)
        if(window.innerWidth == 572){
            console.log("Maior que 572");
        }
    }
    return(
        <header >
            <nav  className={value? 'navbar-layout':'navbar'}>
                <div className='Logo'>
                    <a className="Logo-link" href='#'>Marcelo Augusto.Dev</a>
                 </div>
                
                <div  className='Hamburguer' onChange={(teste)=>console.log(teste)} onClick={() => setValue(!value)}>
                        <span className="line burger1"/>
                        <span className="line burger2"/>
                        <span className="line burger3"/>
                </div>
                <ul id="Menuitens"className={value?'Menu':"Menu-dropdown"}>
                    <li className='menu-item'><a className="link" href='#'>Home</a></li>
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