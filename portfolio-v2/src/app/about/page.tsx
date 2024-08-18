import Image from "next/image"
import ImgAbout from "../../assets/anime.jpg"
export default function PageAbout() {
    return (
        <>
            <main className="container mx-auto">
                <section className="flex">
                    <div className="relative flex-col items-start m-auto align-middle">
                        <div className="grid grid-cols-1  sm:p-28  lg:grid-cols-2 lg:gap-24 ">
                            <div className="relative items-center  m-auto lg:inline-flex lg:order-first">
                                <div className="max-w-xl text-center lg:text-left">
                                    <div>
                                        <p className="text-3xl font-semibold tracking-tight text-purple-600 ">
                                            Sobre mim
                                        </p>
                                        <p className="w-full max-w-xl mt-4 text-base tracking-tight text-white">
                                        Olá, sou Marcelo Augusto, de Olhos D'Água. Sou programador autodidata e dedico a maior parte do meu tempo ao desenvolvimento de aplicações e ao engajamento em comunidades de tecnologia, onde compartilho e adquiro conhecimento. Programar é mais do que um hobby para mim; é o meu maior investimento, tanto profissional quanto pessoal. Além da programação, sou apaixonado por animes e séries, que também estimulam minha criatividade.
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div className="order-first flex w-full mt-12 aspect-square">
                                <Image className=" rounded-3xl w-full mx-auto" style={{
                                    width: "350px",
                                    height: "350px"
                                }} alt="hero" src={ImgAbout} />
                            </div>
                        </div>
                    </div>
                </section>
                
            </main>

        </>)
}