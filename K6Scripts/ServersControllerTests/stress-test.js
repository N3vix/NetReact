import http from 'k6/http';
import { sleep } from 'k6';
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";

import * as config from '../globalConfig.js';

export const options = {
    stages: [
        { duration: '5s', target: 5 },
        { duration: '30s', target: 5 },
        { duration: '5s', target: 100 },
        { duration: '30s', target: 100 },
        { duration: '5s', target: 400 },
        { duration: '30s', target: 400 },
        { duration: '5s', target: 800 },
        { duration: '30s', target: 800 },
        { duration: '5s', target: 1600 },
        { duration: '30s', target: 1600 },
        { duration: '5s', target: 3200 },
        { duration: '30s', target: 3200 },
        { duration: '5s', target: 6400 },
        { duration: '30s', target: 6400 },
        { duration: '1m', target: 800 },
        { duration: '5m', target: 800 },
        { duration: '5s', target: 0 },
    ],
    thresholds: {
        http_req_duration: ['p(95)<600'],
    },
};

export const setup = () => {
    return { token: config.LOGIN() };
}

export default (data) => {
    http.get(config.API_SERVERS_URL, config.BUILD_BEARER_HEADER(data.token));
    sleep(1);
};

export function handleSummary(data) {
    return {
        "summary.html": htmlReport(data),
    };
}