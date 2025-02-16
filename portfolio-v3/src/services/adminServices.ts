import https from 'https';
import axios from "axios";
const agent = new https.Agent({
    rejectUnauthorized: false
  });
const api = axios.create({
    baseURL:process.env.NEXT_PUBLIC_HOST,
    httpsAgent: agent,
})