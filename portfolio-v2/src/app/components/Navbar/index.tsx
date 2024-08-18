"use client"
import Link from "next/link";
import { useState } from "react";

export default function Navbar() {
  const [showMenu, setShowMenu] = useState(false);

  const toggleMenu = () => {
    setShowMenu(!showMenu);
  };
  const closeMenu = () =>{
    setShowMenu(false);
  }
  return (
    <>
      <header className="">
        <nav className="container mx-auto px-6 py-3">
          <div className="flex items-center justify-between">
            <div className="text-white font-bold text-xl">
              <a href="/">Marcelo Augusto.Dev</a>
            </div>
            <div className="hidden md:block">
              <ul className="flex items-center space-x-8">
                <li><Link href="/" className="text-white">Home</Link></li>
                <li><Link href="/about" className="text-white">About</Link></li>
                <li><Link href="/projects" className="text-white">Projects</Link></li>
                <li><Link href="/contact" className="text-white">Contact</Link></li>
              </ul>
            </div>
            <div className="md:hidden">
              <button className="outline-none mobile-menu-button" onClick={toggleMenu}>
                <svg className="w-6 h-6 text-white" x-show="!showMenu" fill="none" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" viewBox="0 0 24 24" stroke="currentColor">
                  <path d="M4 6h16M4 12h16M4 18h16"></path>
                </svg>
              </button>
            </div>
          </div>
          <div className={`mobile-menu ${showMenu ? "" : "hidden"} md:hidden`}>
            <ul className="mt-4 space-y-4">
              <li><Link onClick={closeMenu} href="/" className="block px-4 py-2 text-white bg-gray-900 rounded">Home</Link></li>
              <li><Link onClick={closeMenu} href="/about" className="block px-4 py-2 text-white bg-gray-900 rounded">About</Link></li>
              <li><Link onClick={closeMenu} href="/projects" className="block px-4 py-2 text-white bg-gray-900 rounded">Projects</Link></li>
              <li><Link onClick={closeMenu} href="/contact" className="block px-4 py-2 text-white bg-gray-900 rounded">Contact</Link></li>
            </ul>
          </div>
        </nav>
      </header>
    </>
  )
}