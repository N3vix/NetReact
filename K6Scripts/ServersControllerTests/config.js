import * as config from '../globalConfig.js';

const API_GET_ALL_SERVERS_URL = `${config.API_BASE_URL}Servers/GetAllServers`;
const API_GET_ADDED_SERVERS_URL = `${config.API_BASE_URL}Servers/GetAddedServers`;

export {
    API_GET_ALL_SERVERS_URL,
    API_GET_ADDED_SERVERS_URL,
}