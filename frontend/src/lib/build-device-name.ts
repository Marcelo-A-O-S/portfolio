import { headers } from "next/headers";

export async function buildDeviceName() {
    const h = await headers();
    const platformRaw = h.get("sec-ch-ua-platform") ?? "";
    const browserRaw = h.get("sec-ch-ua") ?? "";
    const mobileRaw = h.get("sec-ch-ua-mobile");
    const plataform = platformRaw.replace(/"/g, "");
    const isMobile = mobileRaw === "?1";
    const parts = browserRaw.split(",");
    let browser = "Unknown";
    for (const part of parts) {
        const name = part.split(";")[0].replace(/"/g, "");
        if (name.includes("Edge")
            || name.includes("Chrome")
            || name.includes("Firefox")
            || name.includes("Safari")
            || name.includes("Opera")
            || name.includes("Brave")) {
            browser = name;
            break;
        }
    }
    let deviceName = `${plataform} - ${browser}`;
    if (isMobile) {
        deviceName += " (Mobile)"
    }
    return deviceName;
}