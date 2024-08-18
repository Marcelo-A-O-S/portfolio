import { Metadata } from "next";
import ListProjects from "../components/ListProjects"
export const metadata: Metadata = {
    title: "Projects",
    description: "Veja os projetos que desenvolvi e estão em desenvolvimento!",
  };
export default function ProjectsPage() {

    return (
        <>
            <main className="container mx-auto ">
                <section className="flex flex-col items-center gap-7 sm:p-28">
                    <h1 className="text-3xl font-semibold tracking-tight text-purple-600">Projects</h1>
                    <ListProjects/>
                </section>
            </main>
        </>
    )
}