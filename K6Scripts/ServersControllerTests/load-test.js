import http from 'k6/http';
import { sleep } from 'k6';
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";

import * as config from '../globalConfig.js';

export const options = {
    scenarios: {
        get_all_servers_test: {
            executor: "ramping-vus",
            startVUs: 0,
            stages: config.BUILD_LOAD_STAGES(),
            tags: { test_type: 'api' },
            exec: "getAllServersTest"
        },
        get_added_servers_test: {
            executor: "ramping-vus",
            startVUs: 0,
            stages: config.BUILD_LOAD_STAGES(),
            tags: { test_type: 'api' },
            exec: "getAddedServersTest"
        },
    },
    thresholds: {
        "http_req_duration{test_type:api}": ['p(95)<600'],
    },
};

export const setup = () => {
    return { token: config.LOGIN() };
}

export const getAllServersTest = (data) => {
    http.get(config.API_GET_ALL_SERVERS_URL, config.BUILD_BEARER_HEADER(data.token));
    sleep(1);
}

export const getAddedServersTest = (data) => {
    http.get(config.API_GET_ADDED_SERVERS_URL, config.BUILD_BEARER_HEADER(data.token));
    sleep(1);
}

export function handleSummary(data) {
    return {
        "summary.html": htmlReport(data),
    };
}