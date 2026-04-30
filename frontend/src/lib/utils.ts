import { clsx, type ClassValue } from "clsx"
import { ReadonlyURLSearchParams } from "next/navigation";
import { twMerge } from "tailwind-merge"
import type { Element, Root } from "hast";
import { visit } from "unist-util-visit"
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
export function createPageURL(page: number, searchParams: ReadonlyURLSearchParams) {
  const params = new URLSearchParams(searchParams)
  params.set("page", page.toString())
  return `?${params.toString()}`
}
export function updateFilter(key: string, searchParams: ReadonlyURLSearchParams, value?: string) {
  const params = new URLSearchParams(searchParams)
  params.set("page", "1")
  if (!value || value === "ALL") {
    params.delete(key)
  } else {
    params.set(key, value)
  }
  return `?${params.toString()}`;
}
export function rehypePrefixImageHost(host: string) {
  return function plugin() {
    return function transformer(tree: Root) {
      visit(tree, "element", (node) => {
        if (
          node.tagName === "img" &&
          node.properties?.src &&
          typeof node.properties.src === "string"
        ) {
          node.properties.class = "object-cover w-full"
          node.properties.src = `${host}/${node.properties.src}`;
        }
      })
    }
  }
}