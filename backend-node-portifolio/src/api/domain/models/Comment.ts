import { Like } from "./Like";
import { Post } from "./Post";

export class Comment{
    public Id: number;
    public Guid: string;
    public Content: string;
    public PostId:number;
    public Post: Post;
    public DateCreate: Date;
    public ParentId: number;
    public Parent: Comment;
    public Replies: Array<Comment>;
    public Likes: Array<Like>;
    constructor(){
        this.Id = 0;
        this.Guid = "";
        this.Content = "";
        this.PostId = 0;
        this.Post = new Post()
        this.DateCreate = new Date();
        this.ParentId = 0;
        this.Parent = new Comment();
        this.Replies = [];
        this.Likes = new Array<Like>();
    }
}