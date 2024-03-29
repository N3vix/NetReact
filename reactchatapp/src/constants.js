export const BACKEND_BASE_URL = 'https://localhost:7153';
export const USER_TOKEN_KEY = 'accessToken'

export const USER_TOKEN = () => localStorage.getItem(USER_TOKEN_KEY);

export const FETCH_GET = (urlSuffix) => {
    return fetch(BACKEND_BASE_URL + urlSuffix, {
        method: "GET",
        headers: {
            "Authorization": "Bearer " + USER_TOKEN()
        }
    });
};

export const FETCH_POST = (urlSuffix, body) => {
    return fetch(BACKEND_BASE_URL + urlSuffix, {
        method: "POST",
        headers: {
            "Authorization": "Bearer " + USER_TOKEN(),
            'Content-Type': 'application/json'
        },
        body: body
    });
};

export const FETCH_POST_FORM = (urlSuffix, body) => {
    return fetch(BACKEND_BASE_URL + urlSuffix, {
        method: "POST",
        headers: {
            "Accept": "application/json",
            "Authorization": "Bearer " + USER_TOKEN(),
        },
        body: body
    });
};