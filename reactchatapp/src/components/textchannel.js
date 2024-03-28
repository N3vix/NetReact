import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

import { BACKEND_BASE_URL, USER_TOKEN, FETCH_POST, FETCH_POST_FORM } from '../constants'

import ChatRoom from './chatroom';
import { Button } from 'react-bootstrap';

const TextChannel = ({ conn }) => {
    const { serverId, channelId } = useParams();

    const [messages, setMessages] = useState([]);

    useEffect(() => {
        initMessages();
        if (conn) {
            conn.on("ReceiveSpecificMessage", (messageId) => loadSpecificMessage(messageId));
        }

        return () => {
            setMessages([])
        };
    }, [conn]);

    const initMessages = () => {
        FETCH_POST("/ChannelMessages/Get", JSON.stringify({ channelId, take: "40" }))
            .then(r => r.json())
            .then(data => {
                setMessages(data);
            })
            .catch(error => console.log(error))
    }

    const sendMessage = async (message, image) => {
        const formData = new FormData();
        formData.append("channelId", channelId);
        formData.append("content", message);
        formData.append("image", image);
        FETCH_POST_FORM("/ChannelMessages/Add", formData)
            .then(r => r.succ.text())
            .then(data => {
                conn.invoke("SendMessage", data);
            })
            .catch(error => console.log(error))
    }

    const loadPreviousMessages = async () => {
        FETCH_POST("/ChannelMessages/GetBefore", JSON.stringify({ channelId, take: "40", DateTime: messages[0].timestamp }))
            .then(r => r.json())
            .then(data => {
                setMessages(messages => [...data, ...messages])
            })
            .catch(error => console.log(error))
    }

    const loadSpecificMessage = async (messageId) => {
        FETCH_POST("/ChannelMessages/GetById", JSON.stringify({ messageId }))
            .then(r => r.json())
            .then(message => {
                setMessages(messages => [...messages, message])
            })
            .catch(error => console.log(error))
    }

    return <div>
        <div>
            <h3>Server: {serverId}</h3>
            <h3>Channel: {channelId}</h3>
        </div>
        <Button onClick={loadPreviousMessages}>Load previous messages</Button>

        {conn
            ? <ChatRoom messages={messages} sendMessage={sendMessage}></ChatRoom>
            : ""
        }
    </div>
}

export default TextChannel;