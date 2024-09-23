import { NextAuthOptions } from "next-auth";
import GoogleProvider from "next-auth/providers/google";
import FacebookProvider from "next-auth/providers/facebook";
import LinkedIn from "next-auth/providers/linkedin";
import Github from "next-auth/providers/github";
import Instagram from "next-auth/providers/instagram";
import CredentialsProvider from "next-auth/providers/credentials";
import { UserDTO } from "@/DTO/UserDTO";
import { SocialMediaAccountDTO } from "@/DTO/SocialMediaAccountDTO";
import { AuthLogin } from "@/services/authenticationServices"

const googleClientId = process.env.GOOGLE_CLIENT_ID;
const googleClientSecret = process.env.GOOGLE_CLIENT_SECRET;
const facebookClientId = process.env.FACEBOOK_CLIENT_ID;
const facebookClientSecret = process.env.FACEBOOK_CLIENT_SECRET;
const linkedinClientId = process.env.LINKEDIN_CLIENT_ID;
const linkedinClientSecret = process.env.LINKEDIN_CLIENT_SECRET;
const githubClientId = process.env.GITHUB_CLIENT_ID;
const githubClientSecret = process.env.GITHUB_CLIENT_SECRET;
const instagramClientId = process.env.INSTAGRAM_CLIENT_ID;
const instagramClientSecret = process.env.INSTAGRAM_CLIENT_SECRET;

export const authOptions: NextAuthOptions = {
  pages: {
    signIn: "/login"
  },
  providers: [
    GoogleProvider({
      clientId: googleClientId as string,
      clientSecret: googleClientSecret as string,
    }),
    FacebookProvider({
      clientId: facebookClientId as string,
      clientSecret: facebookClientSecret as string,
    }),
    LinkedIn({
      clientId: linkedinClientId as string,
      clientSecret: linkedinClientSecret as string
    }),
    Github({
      clientId: githubClientId as string,
      clientSecret: githubClientSecret as string
    }),
    Instagram({
      clientId: instagramClientId as string,
      clientSecret: instagramClientSecret as string,
    }),
    CredentialsProvider({
      name: "credentials",
      credentials: {
        email: { label: "Email" },
        password: { label: "Password", type: "password" },
      },
      async authorize(credentials) {

        return null
      }
    }),
  ],
  callbacks: {
    async signIn({ user, account, profile, email, credentials }) {
      if (!account) {
        return false
      }
      const provider = account.provider;
      const userDto = new UserDTO()
      if (user.name && user.email && user.id && user.image) {
        userDto.Name = user.name
        const accountDto = new SocialMediaAccountDTO()
        accountDto.Email = user.email
        accountDto.Provider = provider
        accountDto.SocialId = user.id
        accountDto.Username = user.name
        userDto.Accounts.push(accountDto)
        console.log(userDto)
        await AuthLogin(userDto)
      }
      console.log("Usuário logou com o provedor:", provider);
      return true;
    },
  },
}