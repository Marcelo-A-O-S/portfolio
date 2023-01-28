import "./style.css"
import React, { useState, useEffect } from "react"
import image from "../../img/anime.jpg"
import ArrowBack from "../../Icons/arrow-back-ios.svg"
import ArrowForward from "../../Icons/arrow-forward-ios.svg"
import Skills from "../../Models/Skills"
import Formacao from "../../Models/Formacao"
import NET from "../../img/netcore.svg"
import Typescript from "../../img/typescript.svg"
import AspNet from "../../img/Aspnetcore.svg"
import Csharp from "../../img/csharp.svg"
import Reactjs from "../../img/ReactJS.svg"
import Sqlserver from "../../img/sqlserver.svg"
import Excelencia from "../../img/excelencia.png"
import Senai from "../../img/senai.png"

export default function Sobre(){
    const skills : Array<Skills> = [
        new Skills("Typescript","TypeScript é um superconjunto de JavaScript, ou seja, um conjunto de ferramentas e formas mais eficientes de escrever código JavaScript, adicionando recursos que não estão presentes de maneira nativa na linguagem.",Typescript),
        new Skills("Csharp","O C# é uma linguagem de programação multiparadigma criada pela Microsoft, sendo a principal da plataforma .NET.",Csharp),
        new Skills(".NET Core",".NET é uma plataforma de desenvolvedor multiplataforma de código aberto gratuita para criar muitos tipos de aplicativos.",NET),
        new Skills("ASP.NET Core","ASP.NET Core é um framework open-source, multiplataforma, criado pela Microsoft e a comunidade.",AspNet),
        new Skills("SQL Server","O Microsoft SQL Server é um sistema gerenciador de Banco de dados relacional (SGBD) desenvolvido pela Sybase em parceria com a Microsoft .",Sqlserver),
        new Skills("React JS","O React é uma biblioteca de código aberto para interfaces gráficas (GUI) que tem como foco uma só coisa: tornar a experiência do usuário com a interface mais eficiente.",Reactjs),
    ];
    
    const formacoes : Array<Formacao> = [
        new Formacao("Senai/SC - Servico Nacional de Aprendizagem Industrial","Técnico em Desenvolvimento de Sistemas",Senai,"08/2022","08/2024"),
        new Formacao("Colégio Excelência Bocaiuva","Técnico em Mêcanica",Excelencia,"10/2021","10/2023")
    ];
    const [countSkills, setCountSkills] = useState(3);
    const [countFormacao, setCountFormacao] = useState(0);
    
    
    function renderizarFormacoes(){
        var formacoes_academicas = [];
        for (let index = countFormacao ; index <= countFormacao; index++) {
            formacoes_academicas.push(<div className="formacao-academica">
                                            <div className="formacao-info">
                                                <h1 className="nome-instituicao">{formacoes[index].nome_curso}</h1>
                                                <p className="nome-curso">Nome da instituicão de ensino: {formacoes[index].nome_instituicao}</p>
                                                <p className="data-inicio">Data inicio: {formacoes[index].data_inicio}</p>
                                                <p className="data-termino">Data termino: {formacoes[index].data_termino}</p>
                                            </div>
                                            <div>
                                                <img src={formacoes[index].img_instituicao} alt=""/>
                                            </div>
                                        </div>
            )  
        };
        return formacoes_academicas;
    }
    // const renderizarCards;
    function AnteriorFormacao(){
        if((countFormacao - 1 ) >= 0){
            setCountFormacao(countFormacao - 1 );
        }
    }
    function ProximaFormacao(){
        if((countFormacao + 1) < formacoes.length){
            setCountFormacao(countFormacao + 1);
        }
    }
    function renderizarCards() {
        
        var cards = [];
         for (let index = countSkills - 3; index <= countSkills; index++) {
             if(index >= 0 || index >= skills.length){
                console.log(index)
                if(skills[index] !== undefined){
                    
                    cards.push(<div className="card-skill">
                                <img src={skills[index].image} alt="" className="image-skills"/>
                                <div className="info-skills">
                                    <p className="titulo-info-skills">{skills[index].titulo}</p>
                                    <p className="text-info-skills">{skills[index].text}</p>
                                </div>
                            </div>)
                }  
             }
        }
        return cards
    }
    function AnteriorSkill(){
        
        if((countSkills - 3) > 0 ){
            setCountSkills(countSkills - 1)
        }else{
            return;
        }
    }
    function ProximaSkill(){
        
        if((countSkills + 1) < skills.length){
            setCountSkills(countSkills + 1)
        }else{
            return;
        }
    }
    return(
        <div>
            <section className="Sobre">
                <div>
                    <img src={image} alt="" className="image-sobre"/>
                </div>
                <div className="apresentacao-sobre">
                    <h1 className="titulo-sobre"><span className="sobre1">Sobre</span> <span className="sobre2">mim</span><span className="esclamacao">|</span></h1>
                    <p className="text-sobre"> Meu nome é Marcelo, sou estudante de programacão e apaixonado pela mesma, estudo diariamente assuntos relacionados a mesma, se tornou um dos meus hobbys, também gosto de animes e séries mas dedico a maior parte do meu tempo a programacão, tenho um vicio em criar aplicacões!</p>
                </div>
            </section>
            <section id="SKills" className="Skills" onScroll={(event)=>console.log(event)}>
                <div className="titulo-skills">
                    <h1 className="titulo-sobre">Skills<span className="esclamacao">|</span></h1>
                </div>
                <div className="alinhamento-carrosel">
                    <div className="skill-orientacao">
                        <img src={ArrowBack} alt="" onClick={()=>AnteriorSkill()} />
                    </div>
                    <div className="carrosel">
                        {renderizarCards()}
                    </div>
                    <div className="skill-orientacao">
                    <img src={ArrowForward} alt="" onClick={()=>ProximaSkill()}/>
                    </div>
                </div>
            </section>
            <section className="Formacao">
                <div className="titulo-formacao">
                    <h1 className="titulo-sobre">Formacão<span className="esclamacao">|</span></h1>
                </div>
                <div className="alinhamento-formacao">
                    <div className="formacao-orientacao">
                            <img src={ArrowBack} alt="" onClick={()=>AnteriorFormacao()} />
                        </div>
                        <div className="formulario-formacao">

                            {renderizarFormacoes()}
                        </div>
                        <div className="formacao-orientacao">
                        <img src={ArrowForward} alt="" onClick={()=>ProximaFormacao()}/>
                        </div>
                    </div>
            </section>
            </div>
        
    )
}