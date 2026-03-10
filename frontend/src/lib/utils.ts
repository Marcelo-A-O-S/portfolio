import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}
export function getCookie(name: string) {
  return document.cookie
    .split("; ")
    .find(row => row.startsWith(name + "="))
    ?.split("=")[1]
}
export function stringToBoolean(value: string): boolean {
  return value.toLocaleLowerCase() === "true";
}
export function generatePagination(currentPage: number, totalPages: number) {
  const pages: (number | "ellipsis")[] = []
  const delta = 2;
  const left = currentPage - delta;
  const right = currentPage + delta;
  for (let i = 1; i <= totalPages; i++) {
    if (i === 1 || i == totalPages ||
      (i >= left && i <= right)
    ) {
      pages.push(i);
    } else {
      if (pages[pages.length - 1] !== "ellipsis") {
        pages.push("ellipsis");
      }
    }
  }
  return pages;
}
