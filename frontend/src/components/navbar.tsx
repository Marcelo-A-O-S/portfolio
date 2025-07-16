import React from 'react'
import { Menu } from 'lucide-react'
import { ToggleTheme } from './toggle-theme'
import { Button } from './ui/button'
import Link from 'next/link'
import {
    Sheet,
    SheetContent,
    SheetHeader,
    SheetTrigger,
} from "@/components/ui/sheet"
export default function NavBar() {
    return (
        <header className='container mx-auto'>
            <nav className='flex items-center px-4 py-4 justify-between'>
                <div className="text-2xl font-bold">
                    <Link href="/">MA.</Link>
                </div>
                <div className='flex space-x-4 items-center'>
                    <ul className="hidden md:flex space-x-4 items-center">
                        <li><Link href="/">Home</Link></li>
                        <li><Link href="/blog">Blog</Link></li>
                        <li><Button variant={"outline"}>Sign In</Button></li>
                    </ul>
                    <ToggleTheme />
                    <Sheet>
                        <SheetTrigger><Menu className='md:hidden h-6 w-6' /></SheetTrigger>
                        <SheetContent>
                            <SheetHeader>
                                <div className='flex flex-col space-y-4'>
                                    <Link href="/">Home</Link>
                                    <Link href="/blog">Blog</Link>
                                    <Button variant={"outline"}>Sign In</Button>
                                </div>
                            </SheetHeader>
                        </SheetContent>
                    </Sheet>
                </div>

            </nav>

        </header>
    )
}
