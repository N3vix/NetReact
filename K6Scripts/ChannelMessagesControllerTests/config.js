import * as config from '../globalConfig.js';

const API_ADD_MESSAGE_URL = `${config.API_BASE_URL}ChannelMessages/Add`;
const API_GET_MESSAGE_URL = `${config.API_BASE_URL}ChannelMessages/GetById`;
const API_GET_MESSAGES_URL = `${config.API_BASE_URL}ChannelMessages/Get`;
const API_GET_BEFORE_MESSAGE_URL = `${config.API_BASE_URL}ChannelMessages/GetBefore`;
const API_UPDATE_MESSAGE_URL = `${config.API_BASE_URL}ChannelMessages/Update`;
const API_DELETE_MESSAGE_URL = `${config.API_BASE_URL}ChannelMessages/Delete`;

export {
    API_ADD_MESSAGE_URL,
    API_GET_MESSAGE_URL,
    API_GET_MESSAGES_URL,
    API_GET_BEFORE_MESSAGE_URL,
    API_UPDATE_MESSAGE_URL,
    API_DELETE_MESSAGE_URL
}