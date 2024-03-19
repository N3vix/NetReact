import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

import { BACKEND_BASE_URL, USER_TOKEN, FETCH_POST } from '../constants'

import ChatRoom from './chatroom';
import { Button } from 'react-bootstrap';

const TextChannel = ({ conn }) => {
    const { serverId, channelId } = useParams();
    
    const [messages, setMessages] = useState([]);

    useEffect(() => {
        initMessage();
        if (conn) {
            conn.on("ReceiveSpecificMessage", (msg) => {
                setMessages(messages => [...messages, msg])
            });
        }

        return () => {
            setMessages([])
        };
    }, [conn]);

    const initMessage = () => {
        FETCH_POST("/ChannelMessages/Get", JSON.stringify({ channelId, take: "40" }))
            .then(r => r.json())
            .then(data => {
                setMessages(data);
            })
            .catch(error => console.log(error))
    }

    const sendMessage = async (message) => {
        try {
            await conn.invoke("SendMessage", message);
        } catch (error) {
            console.log(error)
        }
    }

    const loadPreviousMessages = async () => {
        FETCH_POST("/ChannelMessages/GetBefore", JSON.stringify({ channelId, take: "40", DateTime: messages[0].timestamp }))
            .then(r => r.json())
            .then(data => {
                setMessages(messages => [...data, ...messages])
            })
            .catch(error => console.log(error))
    }

    return <div>
        <h3>Channel: {channelId}</h3>
        <Button onClick={loadPreviousMessages}>Load previous messages</Button>

        {conn
            ? <ChatRoom messages={messages} sendMessage={sendMessage}></ChatRoom>
            : ""
        }
    </div>
}

export default TextChannel;