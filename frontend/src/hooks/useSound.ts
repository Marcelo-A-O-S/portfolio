import { Howl } from "howler";
const sounds = {
    menuOpen: new Howl({
        src: ["/sounds/swoosh-menu.mp3"],
        volume: 0.3
    }),
    menuClose: new Howl({
        src: ["/sounds/swoosh-menu.mp3"],
        volume: 0.2
    }),
    ambient: new Howl({
        src: ["/sounds/music-ambient.mp3"],
        volume: 0.1,
        loop: true
    }),
    linkHover: new Howl({
        src: ["/sounds/hover-link-menu.wav"],
        volume: 0.1
    }),
}
export function useSound(){

    return{
        playMenuOpen: () => sounds.menuOpen.play(),
        playMenuClose: () => sounds.menuClose.play(),
        playMusicAmbient:() => sounds.ambient.play(),
        stopMusicAmbient:()=> sounds.ambient.stop(),
        playLinkHover: () => sounds.linkHover.play()
    }
}