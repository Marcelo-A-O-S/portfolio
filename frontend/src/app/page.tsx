import SplashScreen from "@/components/splash-screen";

export default function Home() {
  return (
    <main className="flex   w-full justify-center ">
      <SplashScreen/>
      <section className="relative w-full h-svh p-10">
        <div>
          <h1>Home</h1>
        </div>
      </section>
    </main>
  );
}
