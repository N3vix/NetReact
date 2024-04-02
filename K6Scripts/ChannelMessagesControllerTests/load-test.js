import http from 'k6/http';
import { sleep } from 'k6';
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";
import { FormData } from 'https://jslib.k6.io/formdata/0.0.2/index.js';

import * as globalConfig from '../globalConfig.js';
import * as config from './config.js';

const messageIds = [];

export const options = {
    scenarios: {
        add_message_test: {
            executor: "ramping-vus",
            startVUs: 0,
            stages: globalConfig.BUILD_LOAD_STAGES(),
            tags: { test_type: 'api' },
            exec: "addMessageTest"
        },
        // get_added_servers_test: {
        //     executor: "ramping-vus",
        //     startVUs: 0,
        //     stages: globalConfig.BUILD_LOAD_STAGES(),
        //     tags: { test_type: 'api' },
        //     exec: "getAddedServersTest"
        // },
    },
    thresholds: {
        "http_req_duration{test_type:api}": ['p(95)<600'],
    },
};

export const setup = () => {
    return {
        token: globalConfig.LOGIN(),
        string: ""
    };
}

export const addMessageTest = (data) => {
    const formData = new FormData();
    formData.append("channelId", "3b15d66c-fea5-4287-9deb-0aff1edc1f47");
    formData.append("content", Math.random().toString());

    const resData = http.post(
        config.API_ADD_MESSAGE_URL,
        formData.body(),
        {
            headers: {
                'Authorization': `Bearer ${data.token}`,
                'Content-Type': 'multipart/form-data; boundary=' + formData.boundary
            }
        }).json();
    messageIds.push(resData["messageId"]);
    console.log(JSON.stringify(resData));
    sleep(1);
}

// export const getAddedServersTest = (data) => {
//     http.get(globalConfig.API_GET_ADDED_SERVERS_URL, globalConfig.BUILD_BEARER_HEADER(data.token));
//     sleep(1);
// }

export function handleSummary(data) {
    return {
        "summary.html": htmlReport(data),
    };
}