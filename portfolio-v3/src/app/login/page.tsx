import ButtonLoginFacebook from "../components/Buttons/ButtonLoginFacebook";
import ButtonLoginGithub from "../components/Buttons/ButtonLoginGithub";
import ButtonLoginGoogle from "../components/Buttons/ButtonLoginGoogle";

export default function LoginPage() {
    return (
        <section className="container mx-auto flex justify-center justify-items-center">
            <div className="bg-black">
                <div className="lg:grid lg:min-h-screen lg:grid-cols-12">
                    <section className="relative flex h-32 items-end bg-gray-900 lg:col-span-5 lg:h-full xl:col-span-6">
                        <img
                            alt=""
                            src="https://th.bing.com/th/id/R.c243ceecd3a14460b76a952893e07083?rik=zBf3zqvvrHSxRg&pid=ImgRaw&r=0&sres=1&sresct=1"
                            className="absolute inset-0 h-full w-full object-cover opacity-80"
                        />

                        <div className="hidden lg:relative lg:block lg:p-12">

                            <h2 className="mt-6 text-2xl font-bold text-white sm:text-3xl md:text-4xl">
                                Welcome! 
                            </h2>

                            <p className="mt-4 leading-relaxed text-white">
                            Faça login usando sua rede social favorita para interagir com meus projetos, deixando seu feedback e mostrando apoio ao conteúdo que mais gostou.
                            </p>
                        </div>
                    </section>
                    <main
                        className="flex items-center justify-center px-8 py-8 sm:px-12 lg:col-span-7 lg:px-16 lg:py-12 xl:col-span-6"
                    >
                        <div className="max-w-xl lg:max-w-3xl">
                            <div className="relative -mt-16 block lg:hidden">
                                <h1 className="mt-2 text-2xl font-bold text-white sm:text-3xl md:text-4xl">
                                    Welcome!
                                </h1>
                                <p className="mt-4 leading-relaxed text-white">
                                Faça login usando sua rede social favorita para interagir com meus projetos, deixando seu feedback e mostrando apoio ao conteúdo que mais gostou.
                                </p>
                            </div>
                            <div className="flex flex-col w-full gap-2">
                                <h2 className="mt-4 leading-relaxed text-white"> Faça login usando a sua rede social favorita!</h2>
                                <ButtonLoginGithub/>
                                <ButtonLoginGoogle/>
                                <ButtonLoginFacebook/>
                                <div className="w-full flex justify-center justify-items-center text-center items-center">
                                    <hr className="bg-white w-full"/>
                                        <p className="text-white p-2">OR</p>
                                    <hr className="bg-white w-full"/>
                                </div>
                                <form action="">
                                    <div>
                                        <label htmlFor="" className="mt-4 leading-relaxed text-white">Email</label>
                                        <input type="text" className="w-full rounded-md focus:ring focus:ring-opacity-75 dark:text-gray-50 focus:dark:ring-violet-600 dark:border-gray-300 p-2"/>
                                    </div>
                                    <div>
                                        <label htmlFor="">Password</label>
                                        <input type="text" className="w-full rounded-md focus:ring focus:ring-opacity-75 dark:text-gray-50 focus:dark:ring-violet-600 dark:border-gray-300 p-2" />
                                    </div>
                                </form>
                                <p className="mt-4 leading-relaxed text-white">Nós garantimos sua privacidade. Usamos suas informações apenas para permitir que você interaja com os projetos.</p>
                            </div>
                        </div>
                    </main>
                </div>
            </div>
        </section>
    )
}