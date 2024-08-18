import Image from "next/image";
import ImgPerfil from "../assets/imagemperfil.jpg"
export default function Home() {
  return (
    <main className="container mx-auto flex min-h-screen flex-col items-center justify-between ">
      <section className="flex w-full gap-16 flex-col justify-between items-center p-10 sm:p-28 md:flex-row">
        <div className="flex flex-col">
          <h1 className="text-purple-600
                  mb-6
                  uppercase
                  font-bold
                  text-[32px]
                  sm:text-[40px]
                  lg:text-[36px]
                  xl:text-[40px]">Full Stack Developer</h1>
          <div className="mt-4  justify-center items-center">
            <span className="inline-flex justify-center items-center mt-2 sm:ml-auto sm:mt-0 sm:justify-start">
              <a target="_blank" href="https://www.facebook.com/marcelo.augusto.dev/" className="text-purple-500 cursor-pointer hover:text-purple-700">
                <svg fill="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                  className="w-5 h-5" viewBox="0 0 24 24">
                  <path d="M18 2h-3a5 5 0 00-5 5v3H7v4h3v8h4v-8h3l1-4h-4V7a1 1 0 011-1h3z"></path>
                </svg>
              </a>
              <a target="_blank" href="https://x.com/Telo_Augusto_O" className="ml-3 text-purple-500 cursor-pointer hover:text-purple-700">
                <svg fill="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                  className="w-5 h-5" viewBox="0 0 24 24">
                  <path
                    d="M23 3a10.9 10.9 0 01-3.14 1.53 4.48 4.48 0 00-7.86 3v1A10.66 10.66 0 013 4s-4 9 5 13a11.64 11.64 0 01-7 2c9 5 20 0 20-11.5a4.5 4.5 0 00-.08-.83A7.72 7.72 0 0023 3z">
                  </path>
                </svg>
              </a>
              <a target="_blank" href="https://www.instagram.com/marcelo.augusto1234/" className="ml-3 text-purple-500 cursor-pointer hover:text-purple-700">
                <svg fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round"
                  stroke-width="2" className="w-5 h-5" viewBox="0 0 24 24">
                  <rect width="20" height="20" x="2" y="2" rx="5" ry="5"></rect>
                  <path d="M16 11.37A4 4 0 1112.63 8 4 4 0 0116 11.37zm1.5-4.87h.01"></path>
                </svg>
              </a>
              <a target="_blank" href="https://www.linkedin.com/in/marcelo-augusto-dev/" className="ml-3 text-purple-500 cursor-pointer hover:text-purple-700">
                <svg fill="currentColor" stroke="currentColor" stroke-linecap="round"
                  stroke-linejoin="round" stroke-width="0" className="w-5 h-5" viewBox="0 0 24 24">
                  <path stroke="none"
                    d="M16 8a6 6 0 016 6v7h-4v-7a2 2 0 00-2-2 2 2 0 00-2 2v7h-4v-7a6 6 0 016-6zM2 9h4v12H2z">
                  </path>
                  <circle cx="4" cy="4" r="2" stroke="none"></circle>
                </svg>
              </a>
              <a target="_blank" href="https://github.com/Marcelo-A-O-S" className="ml-3 text-purple-500 hover:text-purple-700">
                <svg className="h-6 w-6" fill="currentColor" viewBox="0 0 24 24" aria-hidden="true">
                  
                  <path fill-rule="evenodd" d="M12 2C6.477 2 2 6.484 2 12.017c0 4.425 2.865 8.18 6.839 9.504.5.092.682-.217.682-.483 0-.237-.008-.868-.013-1.703-2.782.605-3.369-1.343-3.369-1.343-.454-1.158-1.11-1.466-1.11-1.466-.908-.62.069-.608.069-.608 1.003.07 1.531 1.032 1.531 1.032.892 1.53 2.341 1.088 2.91.832.092-.647.35-1.088.636-1.338-2.22-.253-4.555-1.113-4.555-4.951 0-1.093.39-1.988 1.029-2.688-.103-.253-.446-1.272.098-2.65 0 0 .84-.27 2.75 1.026A9.564 9.564 0 0112 6.844c.85.004 1.705.115 2.504.337 1.909-1.296 2.747-1.027 2.747-1.027.546 1.379.202 2.398.1 2.651.64.7 1.028 1.595 1.028 2.688 0 3.848-2.339 4.695-4.566 4.943.359.309.678.92.678 1.855 0 1.338-.012 2.419-.012 2.747 0 .268.18.58.688.482A10.019 10.019 0 0022 12.017C22 6.484 17.522 2 12 2z" clip-rule="evenodd" />
                </svg>
              </a>
            </span>
          </div>
          <p>Olá, meu nome é Marcelo Augusto e sou desenvolvedor nas horas vagas!</p>
        </div>
        <div className="flex">
          <Image className="rounded-3xl" src={ImgPerfil} alt="Marcelo Augusto" />
        </div>
      </section>
    </main>
  );
}
