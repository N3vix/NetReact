import "./homepage.css"

import { useState, useEffect } from 'react';
import { Row, Col, Container, Button } from 'react-bootstrap';
import { Routes, Route, Link, Outlet } from 'react-router-dom';

import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import ChatRoom from './chatroom';
import WaitingRoom from './waitingroom';

const Homepage = () => {
    const apiUrl = "https://localhost:7153/";
    const token = localStorage.getItem('accessToken');

    const [servers, setServers] = useState([]);

    useEffect(() => {
        fetch(apiUrl + "Servers/GetAddedServers", {
            method: "GET",
            headers: {
                "Authorization": "Bearer " + token
            }
        })
            .then(r => r.json())
            .then(data => setServers(data))
            .catch(error => console.log(error))
    }, [])

    return <div className="nav-scrollable">
        <nav className="flex-column">
            <div className="nav-item px-3">
                <Link className="nav-link" to="/">Home</Link>
            </div>

            {servers.map((server, index) =>
                <div className="nav-item px-3">
                    <Link className="nav-link" key={index} to={server.serverId}>{server.name}</Link>
                </div>)}
        </nav>

        <Outlet />
    </div>

    // const [conn, setConnection] = useState();
    // const [messages, setMessages] = useState([]);

    // const joinChatRoom = async (username, chatroom) => {
    //     try {
    //         const conn = new HubConnectionBuilder()
    //             .withUrl("https://localhost:7153/chat?access_token=" + token)
    //             .configureLogging(LogLevel.Information)
    //             .build();
    //         conn.on("JoinSpecificChat", (username, msg) => {
    //             console.log("msg: ", msg)
    //             setMessages(messages => [...messages, { username, msg }])
    //         });

    //         conn.on("ReceiveSpecificMessage", (username, msg) => {
    //             setMessages(messages => [...messages, { username, msg }])
    //         });

    //         await conn.start();
    //         await conn.invoke("JoinSpecificChat", { username, chatroom });

    //         setConnection(conn);
    //     } catch (error) {
    //         console.log(error);
    //     }
    // }

    // const sendMessage = async (message) => {
    //     try {
    //         await conn.invoke("SendMessage", message);
    //     } catch (error) {
    //         console.log(error)
    //     }
    // }

    // const handleLogout = () => {
    //     localStorage.removeItem("accessToken");
    //     window.location.href = "/";
    // };

    // return <div>
    //     <Container>
    //         <Row className='px-5 my-5'>
    //             <Col sm='12'>
    //                 <h1 className='font-weight-light'>Welcome to the Chat App</h1>
    //             </Col>
    //         </Row>
    //         {!conn
    //             ? <WaitingRoom joinChatRoom={joinChatRoom}></WaitingRoom>
    //             : <ChatRoom messages={messages} sendMessage={sendMessage}></ChatRoom>
    //         }
    //     </Container>
    // </div>
}

export default Homepage;