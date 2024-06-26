import http from 'k6/http';
import { sleep } from 'k6';
import * as config from '../globalConfig.js';

export const options = {
    vus: 1,
    duration: '1m',
    thresholds: {
        http_req_duration: ['p(95)<1000']
    },
};

export default function () {
    http.get(config.API_SERVERS_URL);
    sleep(1);
}