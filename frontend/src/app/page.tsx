import SplashScreen from "@/components/splash-screen";
import Image from "next/image";

export default function Home() {
  return (
    <main className="relative mx-auto flex min-h-screen inset-0 w-full max-w-[1440px] justify-center bg-white dark:bg-black ">
      <SplashScreen/>
      <section className="relative w-screen h-svh p-10">
        <div>
          <h1>Home</h1>
        </div>
      </section>
    </main>
  );
}
