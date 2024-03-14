import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { Row, Col, Container, Button } from 'react-bootstrap';

import { BACKEND_BASE_URL, USER_TOKEN } from '../constants'
import ChatRoom from './chatroom';
import WaitingRoom from './waitingroom';

const Server = () => {
    const { id } = useParams();

    const [conn, setConnection] = useState();
    const [messages, setMessages] = useState([]);

    const joinChatRoom = async (chatroom) => {
        try {
            const newConnection = new HubConnectionBuilder()
                .withUrl(BACKEND_BASE_URL + "/chat?access_token=" + USER_TOKEN())
                .configureLogging(LogLevel.Information)
                .build();
            newConnection.on("JoinSpecificChat", (userId, msg) => {
                console.log("msg: ", msg)
                setMessages(messages => [...messages, { userId, msg }])
            });

            newConnection.on("ReceiveSpecificMessage", (userId, msg) => {
                setMessages(messages => [...messages, { userId, msg }])
            });

            await newConnection.start();
            await newConnection.invoke("JoinSpecificChat", { id, chatroom });

            setConnection(newConnection);

        } catch (error) {
            console.log(error);
        }
    }

    const sendMessage = async (message) => {
        try {
            await conn.invoke("SendMessage", message);
        } catch (error) {
            console.log(error)
        }
    }

    return <div>
        <h3>ID: {id}</h3>
        {conn
            ? <ChatRoom messages={messages} sendMessage={sendMessage}></ChatRoom>
            : <WaitingRoom joinChatRoom={joinChatRoom}></WaitingRoom>
        }
    </div>
}

export default Server;