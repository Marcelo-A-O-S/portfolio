import { Section } from "./Section";

export class Title{
    public Id: number;
    public Guid: string;
    public Sequence: number;
    public Content: string;
    public SectionId: number;
    public Section: Section;
    constructor(){
        this.Id = 0;
        this.Guid = "";
        this.Sequence = 0;
        this.Content = "";
        this.SectionId = 0;
        this.Section = new Section()
    }
}