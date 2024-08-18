"use client"
import { useEffect, useState } from "react"
import dataProjects from "../../data/projects/data.json"
import Project from "@/models/Project"
import Image from "next/image"
import ImgLoadingImage from "../../assets/loading.png"
export default function ProjectsPage() {
    const [projects, setProjects] = useState<Project[]>()
    useEffect(() => {
        setProjects(dataProjects.Projetos)
    }, [])
    return (
        <>
            <main className="container mx-auto ">
                <section className="flex flex-col items-center gap-7 sm:p-28">
                    <h1 className="text-3xl font-semibold tracking-tight text-purple-600">Projects</h1>
                    <div className="md:px-4 md:grid md:grid-cols-2 lg:grid-cols-3 gap-5 space-y-4 md:space-y-0">
                        {projects?.map(project => {
                            return (<>
                                <div className="max-w-sm bg-black border-solid border-2 border-purple-600  px-6 pt-6 pb-2 rounded-xl shadow-lg transform hover:scale-105 transition duration-500">
                                    <div className="relative">
                                        {project.imagemprojeto?
                                        <img className="w-full rounded-xl" src={project.imagemprojeto} alt="Colors" />
                                        :
                                        <img className="w-full rounded-xl" src={ImgLoadingImage.src} alt="Colors" />
                                        }
                                        
                                    </div>
                                    <h1 className="mt-4 text-purple-600 text-2xl font-bold cursor-pointer">{project.nomeprojeto}</h1>
                                    <div className="my-4 flex flex-col gap-4">
                                        <div className="flex space-x-1 items-center h-full">
                                            
                                            <p>{project.descricaoprojeto}</p>
                                        </div>
                                        <a href={project.linkprojeto} target="_blank" className="mt-4 text-xl w-full text-center text-white bg-purple-600 py-2 rounded-xl shadow-lg">Ver projeto</a>
                                    </div>
                                </div>
                            </>)
                        })}

                    </div>
                </section>
            </main>
        </>
    )
}