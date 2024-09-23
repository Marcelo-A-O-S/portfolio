import { Category } from "./Category";
import { DevelopmentTool } from "./DevelopmentTool";
import { Like } from "./Like";
import { Link } from "./Link";
import { Section } from "./Section";
import { Comment } from "./Comment";

export class Post{
    public Id: number;
    public Guid: string;
    public Sections: Section[];
    public Categories: Array<Category>;
    public DevelopmentTools : Array<DevelopmentTool>;
    public DateCreate: Date;
    public Links: Array<Link>;
    public Likes: Array<Like>;
    public Comments: Comment[];
    constructor(){
        this.Id = 0;
        this.Guid = "";
        this.Sections = new Array<Section>();
        this.Categories = new Array<Category>();
        this.DateCreate = new Date();
        this.DevelopmentTools = new Array<DevelopmentTool>();
        this.Links = new Array<Link>();
        this.Likes = new Array<Like>();
        this.Comments = new Array<Comment>()
    }
}