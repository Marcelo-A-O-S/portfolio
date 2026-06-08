import SplashScreen from "@/components/splash-screen";

export default function Home() {
  return (
    <main className="mx-auto flex min-h-screen inset-0 w-full max-w-[1440px] justify-center ">
      <SplashScreen/>
      <section className="relative w-full h-svh p-10">
        <div>
          <h1>Home</h1>
        </div>
      </section>
    </main>
  );
}
