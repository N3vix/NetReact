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
            conn.on("AddMessage", (messageId) => loadSpecificMessage(messageId));
            conn.on("EditMessage", (messageId) => editSpecificMessage(messageId));
            conn.on("DeleteMessage", (messageId) => deleteSpecificMessage(messageId));
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
            .catch(error => {
                console.log(error)
            })
    }

    const sendMessage = async (content, image) => {
        const formData = new FormData();
        formData.append("channelId", channelId);
        formData.append("content", content);
        if (image)
            formData.append("image", image);
        FETCH_POST_FORM("/ChannelMessages/Add", formData)
            .then(r => r.json())
            .then(data => {
                conn.invoke("SendMessage", data["messageId"]);
            })
            .catch(error => console.log(error))
    }

    const editMessage = async (messageId, content) => {
        FETCH_POST("/ChannelMessages/Update", JSON.stringify({ messageId, content }))
            .then(r => r.text())
            .then(data => {
                if (/^true$/i.test(data))
                    conn.invoke("EditMessage", messageId);
            })
            .catch(error => console.log(error))
    }

    const deleteMessage = async (messageId) => {
        FETCH_POST("/ChannelMessages/Delete", JSON.stringify({ messageId }))
            .then(r => r.text())
            .then(data => {
                if (/^true$/i.test(data))
                    conn.invoke("DeleteMessage", messageId);
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

    const editSpecificMessage = async (messageId) => {
        FETCH_POST("/ChannelMessages/GetById", JSON.stringify({ messageId }))
            .then(r => r.json())
            .then(message => {
                setMessages(messages => {
                    const index = messages.findIndex(message => message.id === messageId)
                    messages[index] = message;
                    return Array.from(messages);
                })
            })
            .catch(error => console.log(error))
    }

    const deleteSpecificMessage = async (messageId) => {
        setMessages(messages => messages.filter(message => message.id !== messageId))
    }

    return <div>
        <div>
            <h3>Server: {serverId}</h3>
            <h3>Channel: {channelId}</h3>
        </div>
        <Button onClick={loadPreviousMessages}>Load previous messages</Button>

        {conn
            ? <ChatRoom messages={messages} sendMessage={sendMessage} editMessage={editMessage} deleteMessage={deleteMessage}></ChatRoom>
            : ""
        }
    </div>
}

export default TextChannel;