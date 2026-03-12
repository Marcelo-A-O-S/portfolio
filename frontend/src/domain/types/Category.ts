import { CategoryContents } from "./CategoryContents"
export type Category = {
    id: string,
    categoryContents: CategoryContents[],
    createdAt: Date
}