import { Text } from "./Text";
import { Title } from "./Title";
import { Image } from "./Image";
import { Video } from "./Video";
export class Section{
    public Id: number;
    public Guid: string;
    public Sequence: number;
    public Titles: Array<Title>;
    public Texts: Array<Text>;
    public Images: Array<Image>;
    public Videos: Array<Video>;
    constructor(){
        this.Id = 0;
        this.Guid = "";
        this.Sequence = 0;
        this.Titles = new Array<Title>();
        this.Texts = new Array<Text>();
        this.Images = new Array<Image>();
        this.Videos = new Array<Video>();
    }
}