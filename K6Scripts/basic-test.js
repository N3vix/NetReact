import http from 'k6/http';
import { sleep } from 'k6';
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";

import * as config from './config.js';

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