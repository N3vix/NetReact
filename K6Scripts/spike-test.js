import http from 'k6/http';
import { sleep } from 'k6';
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";

import * as config from './config.js';

export const options = {
    scenarios: {
        get_all_servers_test: {
            executor: "ramping-arrival-rate",
            // startVUs: 0,
            startRate: 50,
            preAllocatedVUs: 50,
            maxVUs: 400,
            timeUnit: '1s',
            stages: config.BUILD_SPIKE_STAGES(),
            tags: { test_type: 'api' },
            exec: "getAllServersTest"
        },
        get_added_servers_test: {
            executor: "ramping-arrival-rate",
            startTime: '1m10s',
            // startVUs: 0,
            startRate: 50,
            preAllocatedVUs: 50,
            maxVUs: 400,
            timeUnit: '1s',
            stages: config.BUILD_SPIKE_STAGES(),
            tags: { test_type: 'api' },
            exec: "getAddedServersTest",
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
    // sleep(1);
}

export const getAddedServersTest = (data) => {
    http.get(config.API_GET_ADDED_SERVERS_URL, config.BUILD_BEARER_HEADER(data.token));
    // sleep(1);
}

export function handleSummary(data) {
    return {
        "summary.html": htmlReport(data),
    };
}