export const BACKEND_BASE_URL = 'https://localhost:7153';
export const USER_TOKEN_KEY = 'accessToken'

export const USER_TOKEN = () => localStorage.getItem(USER_TOKEN_KEY);