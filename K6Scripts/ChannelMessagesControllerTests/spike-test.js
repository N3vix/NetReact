import http from 'k6/http';
import { sleep } from 'k6';
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";

import * as globalConfig from '../globalConfig.js';
import * as config from './Config.js';

export const options = {
    scenarios: {
        get_all_servers_test: {
            executor: "ramping-arrival-rate",
            // startVUs: 0,
            startRate: 50,
            preAllocatedVUs: 50,
            maxVUs: 400,
            timeUnit: '1s',
            stages: globalConfig.BUILD_SPIKE_STAGES(),
            tags: { test_type: 'api' },
            exec: "getMessagesBeforeTest"
        },
        // get_added_servers_test: {
        //     executor: "ramping-arrival-rate",
        //     startTime: '1m10s',
        //     // startVUs: 0,
        //     startRate: 50,
        //     preAllocatedVUs: 50,
        //     maxVUs: 400,
        //     timeUnit: '1s',
        //     stages: globalConfig.BUILD_SPIKE_STAGES(),
        //     tags: { test_type: 'api' },
        //     exec: "getAddedServersTest",
        // },
    },
    thresholds: {
        "http_req_duration{test_type:api}": ['p(95)<600'],
    },
};

export const setup = () => {
    const token = globalConfig.LOGIN();
    const postBody = {
        channelId: "3b15d66c-fea5-4287-9deb-0aff1edc1f47",
        take: 1
    };
    const initialMsg = http.post(config.API_GET_MESSAGES_URL, JSON.stringify(postBody), globalConfig.BUILD_BEARER_HEADER(token)).json()[0];
    return {
        token: token,
        initialMsg: initialMsg
    };
}

export const getMessagesBeforeTest = (data) => {
    const postBody = {
        channelId: "3b15d66c-fea5-4287-9deb-0aff1edc1f47",
        take: 40,
        dateTime: data.initialMsg.timestamp
    };
    const resp = http.post(config.API_GET_BEFORE_MESSAGE_URL, JSON.stringify(postBody), globalConfig.BUILD_BEARER_HEADER(data.token));
}

// export const getAddedServersTest = (data) => {
//     http.get(config.API_GET_ADDED_SERVERS_URL, globalConfig.BUILD_BEARER_HEADER(data.token));
// }

export function handleSummary(data) {
    return {
        "summaryCached.html": htmlReport(data),
    };
}