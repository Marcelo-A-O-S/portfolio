import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* config options here */
  output: "standalone",
  images:{
    remotePatterns:[{
      hostname: "avatars.githubusercontent.com",
      protocol: "https"
    },{
      hostname: "lh3.googleusercontent.com",
      protocol: "https"
    },{
      hostname: "media.licdn.com",
      protocol: "https"
    }]
  }
};

export default nextConfig;
