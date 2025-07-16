import React from 'react'
import { blogs } from '@/config/data-blogs'
import BlogCard from '@/components/blog-card';
export default function BlogPage() {
  const posts = blogs;
  return (
    <main className="container mx-auto">
      <div className='flex w-full flex-col items-center space-x-4 space-y-4 px-4 py-8 '>
        <h1 className="text-2xl font-bold">Blogs</h1>
        <div className='w-full grid grid-cols-1 space-x-2 sm:grid-cols-2 md:grid-cols-3'>
          {posts.map((post) => (
            <BlogCard key={post.id} post={post} />
          ))}
        </div>
      </div>
      
    </main>
  )
}
