import http from 'k6/http';

const API_BASE_URL = 'https://localhost:7153/';
const API_GET_ALL_SERVERS_URL = `${API_BASE_URL}Servers/GetAllServers`;
const API_GET_ADDED_SERVERS_URL = `${API_BASE_URL}Servers/GetAddedServers`;
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
        { duration: '5s', target: 6400 },
        { duration: '1m', target: 6400 },
        { duration: '5s', target: 0 },
    ];
}

export {
    API_GET_ALL_SERVERS_URL,
    API_GET_ADDED_SERVERS_URL,
    LOGIN, BUILD_BEARER_HEADER,
    BUILD_LOAD_STAGES,
    BUILD_SPIKE_STAGES
};