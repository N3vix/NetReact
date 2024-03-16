import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

import { BACKEND_BASE_URL, USER_TOKEN } from '../constants'

import ChatRoom from './chatroom';
import { Button } from 'react-bootstrap';

const Channel = () => {
    const { serverId, channelId } = useParams();

    const [conn, setConnection] = useState();
    const [messages, setMessages] = useState([]);

    function createHubConnection() {
        const newConnection = new HubConnectionBuilder()
            .withUrl(BACKEND_BASE_URL + "/chat?access_token=" + USER_TOKEN())
            .configureLogging(LogLevel.Information)
            .build();
        newConnection.on("JoinSpecificChat", (userId, msg) => {
            console.log("msg: ", msg)
            // setMessages(messages => [...messages, { userId, msg }])
        });

        newConnection.on("ReceiveSpecificMessage", (msg) => {
            setMessages(messages => [...messages, msg])
        });

        setConnection(newConnection);
    }

    function initMessage() {
        fetch(BACKEND_BASE_URL + "/ChannelMessages/Get", {
            method: "POST",
            headers: {
                "Authorization": "Bearer " + USER_TOKEN(),
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ channelId, take: "40" })
        })
            .then(r => r.json())
            .then(data => {
                setMessages(data);
            })
            .catch(error => console.log(error))
    }

    useEffect(() => {
        createHubConnection();
    }, [channelId]);

    useEffect(() => {
        initMessage();
        if (conn) {
            try {
                conn
                    .start()
                    .then(() => conn.invoke("JoinSpecificChat", { serverId, channelId }))
                    .catch((err) => {
                        console.log(`Error: ${err}`);
                    });

            } catch (error) {
                console.log(error);
            }
        }

        return () => {
            conn?.stop();
            setMessages([])
        };
    }, [conn]);

    const sendMessage = async (message) => {
        try {
            await conn.invoke("SendMessage", message);
        } catch (error) {
            console.log(error)
        }
    }

    const loadPreviousMessages = async () => {
        fetch(BACKEND_BASE_URL + "/ChannelMessages/GetBefore", {
            method: "POST",
            headers: {
                "Authorization": "Bearer " + USER_TOKEN(),
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ channelId, take: "40", DateTime: messages[0].timestamp })
        })
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
            : "" // : <WaitingRoom joinChatRoom={joinChatRoom}></WaitingRoom>
        }
    </div>
}

export default Channel;