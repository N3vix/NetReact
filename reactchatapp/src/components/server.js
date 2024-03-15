import { useState, useEffect } from 'react';
import { useParams, Link, Outlet } from 'react-router-dom';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { Row, Col, Container, Button } from 'react-bootstrap';

import { BACKEND_BASE_URL, USER_TOKEN } from '../constants'
import ChatRoom from './chatroom';
import WaitingRoom from './waitingroom';

const Server = () => {
    const { serverId } = useParams();

    const [channels, setChannels] = useState([]);

    useEffect(() => {
        fetch(BACKEND_BASE_URL + "/Channels/GetChannels" + "?serverId=" + serverId, {
            method: "GET",
            headers: {
                "Authorization": "Bearer " + USER_TOKEN()
            }
        })
            .then(r => r.json())
            .then(data => setChannels(data))
            .catch(error => console.log(error))
    }, [])

    // const [conn, setConnection] = useState();
    // const [messages, setMessages] = useState([]);

    // const joinChatRoom = async (chatroom) => {
    //     try {
    //         const newConnection = new HubConnectionBuilder()
    //             .withUrl(BACKEND_BASE_URL + "/chat?access_token=" + USER_TOKEN())
    //             .configureLogging(LogLevel.Information)
    //             .build();
    //         newConnection.on("JoinSpecificChat", (userId, msg) => {
    //             console.log("msg: ", msg)
    //             setMessages(messages => [...messages, { userId, msg }])
    //         });

    //         newConnection.on("ReceiveSpecificMessage", (userId, msg) => {
    //             setMessages(messages => [...messages, { userId, msg }])
    //         });

    //         await newConnection.start();
    //         await newConnection.invoke("JoinSpecificChat", { id, chatroom });

    //         setConnection(newConnection);

    //     } catch (error) {
    //         console.log(error);
    //     }
    // }

    // useEffect(() => {
    //     setConnection(null)
    // })

    // const sendMessage = async (message) => {
    //     try {
    //         await conn.invoke("SendMessage", message);
    //     } catch (error) {
    //         console.log(error)
    //     }
    // }

    return <div>
        <h3>ID: {serverId}</h3>

        <div className="page">
            <div className="sidebar">
                <div className="nav-scrollable">
                    <nav className="flex-column">
                        {channels.map((channel, index) =>
                            <div className="nav-item px-3">
                                <Link className="nav-link" key={index} to={channel.id}>{channel.name}</Link>
                            </div>)}
                    </nav>
                </div>
            </div>

            <main>
                <article className="content px-4">
                    <Outlet />
                </article>
            </main>
        </div>

        {/* {conn
            ? <ChatRoom messages={messages} sendMessage={sendMessage}></ChatRoom>
            : <WaitingRoom joinChatRoom={joinChatRoom}></WaitingRoom>
        } */}
    </div>
}

export default Server;