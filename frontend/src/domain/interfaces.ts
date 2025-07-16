export interface Tool {
    id: string;
    name: string;
    description?: string;
  }
  
  export interface BlogType {
    id: string;
    name: string;
  }
  
  export interface Category {
    id: string;
    name: string;
  }
  
  export interface SubCategory {
    id: string;
    name: string;
    categoryId: string;
    category: Category;
  }
  
  export interface LinkType {
    id: string;
    name: string;
  }
  
  export interface Link {
    id: string;
    linkTypeId: string;
    content: string;
    linkType: LinkType;
  }
  
  export interface Like {
    id: string;
    postId: string;
    userId: string;
  }
  
  export interface Comment {
    id: string;
    content: string;
    postId: string;
    userId: string;
    createdAt: string;
    updatedAt: string;
  }
  
  export interface Author {
    id: string;
    name?: string;
    email: string;
    image?: string;
  }
  
  export interface Post {
    id: string;
    title: string;
    slug: string;
    description: string;
    keywords: string;
    content: string;
    tools: Tool[];
    imageUrl: string;
    types: BlogType[];
    isPublished: boolean;
    createdAt: string;
    updatedAt: string;
    authorId: string;
    author: Author;
    Categories: Category[];
    likes: Like[];
    comments: Comment[];
    links: Link[];
  }
  