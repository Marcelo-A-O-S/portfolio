"use client"
import Image from "next/image";
import { useEffect, useState } from "react";

export default function Home() {
  const [ closeIntro, setCloseIntro] = useState(false)
  const [ showIntro, setShowIntro] = useState(false)
  const [ showWelcome, setShowWelcome] = useState(false)
  const [isFirstVisit, setIsFirstVisit] = useState(false);
  useEffect(()=>{
    setTimeout(()=>{
      setShowIntro(true)
      setTimeout(()=>{
        setShowIntro(false)
        setShowWelcome(true)
        setTimeout(()=>{
          setShowWelcome(false)
          setCloseIntro(true)
        },4000)
      },2600)
    },400)
  },[])
  
  return (
    <div className="">
        <div className={`intro ${closeIntro?"intro-close":""} `}>
          <h1 className="logo-header">
            <span className={`${showIntro?"logo-title":"d-none"}`}>Hello, my name is Marcelo Augusto!</span>
            <span className={`${showWelcome?"logo-welcome":"d-none"}`}>Welcome!</span>
          </h1>
        </div>
        <section className="flex">
          <div className="">
              <div>
                  <h1>FullStack Developer</h1>
              </div>
          </div>
          <div>

          </div>
        </section>
        <section className="">
          <div>

          </div>
          <div>

          </div>
        </section>
    </div>
  );
}
