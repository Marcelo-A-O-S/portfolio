import { Config } from ".."
import { App } from "./app"

export const start = async (config:Config)=>{
    new App({
        port: config.port
    }).getApp()
}