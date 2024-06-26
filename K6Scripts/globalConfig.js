import http from 'k6/http';

const API_BASE_URL = 'https://localhost:7153/';
const API_LOGIN_URL = `${API_BASE_URL}AuthManagement/Login`

const headerinfo = {
    headers: {
        'Content-Type': 'application/json'
    },
};

const loginParam = JSON.stringify({
    email: 'demouser@test.email',
    password: '_Aa12345678'
});

const LOGIN = () => {
    const res = http.post(API_LOGIN_URL, loginParam, headerinfo)
    const access_token = res.json().token;

    return access_token;
}

const BUILD_BEARER_HEADER = (token) => {
    return {
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        }
    };
}

const BUILD_LOAD_STAGES = () => {
    return [
        { duration: '5s', target: 5 },
        { duration: '30s', target: 5 },
        { duration: '5s', target: 20 },
        { duration: '30s', target: 20 },
        { duration: '5s', target: 5 },
        { duration: '30s', target: 5 },
        { duration: '5s', target: 0 },
    ];
}

const BUILD_SPIKE_STAGES = () => {
    return [
        { duration: '5s', target: 120 },
        { duration: '5s', target: 1600 },
        { duration: '1m', target: 1600 },
        { duration: '5s', target: 0 },
    ];
}

const GET_RANDOM = (min, max) => {
    return Math.floor(Math.random() * (max - min + 1) + min)
}

export {
    API_BASE_URL,
    LOGIN, BUILD_BEARER_HEADER,
    BUILD_LOAD_STAGES,
    BUILD_SPIKE_STAGES,
    GET_RANDOM,
};