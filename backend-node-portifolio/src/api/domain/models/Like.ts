import { Comment } from "./Comment";
import { Post } from "./Post";
import { User } from "./User";

export class Like{
    public Id: number;
    public Guid: string;
    public UserId: number;
    public User: User;
    public PostId: number;
    public Post: Post;
    public CommentId: number;
    public Comment: Comment;
    constructor() {
        this.Id = 0;
        this.Guid = "";
        this.UserId = 0;
        this.User = new User();
        this.PostId = 0;
        this.Post = new Post();
        this.CommentId = 0;
        this.Comment = new Comment()
    }
}