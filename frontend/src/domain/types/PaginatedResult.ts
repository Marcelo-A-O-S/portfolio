export type PaginatedResult<T> = {
    items: Array<T>,
    totalPages: number,
    currentPage: number,
    totalItems: number
}