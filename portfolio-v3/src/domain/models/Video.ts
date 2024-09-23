import { Section } from "./Section";

export class Video{
    public Id: number;
    public Guid: string;
    public Sequence: number;
    public UrlVideo: string;
    public Caption: string;
    public SectionId: number;
    public Section: Section;
    constructor() {
        this.Id = 0;
        this.Guid = "";
        this.Sequence = 0;
        this.UrlVideo = "";
        this.Caption = "";
        this.SectionId = 0;
        this.Section = new Section();
    }
}