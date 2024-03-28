import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

import { BACKEND_BASE_URL, USER_TOKEN, FETCH_GET } from '../constants'
import TextChannel from './textchannel';
import VoiceChannel from './voicechannel';

const Channel = () => {
    const { serverId, channelId } = useParams();

    const [isTextChannel, setIsTextChannel] = useState(true)

    useEffect(() => {
        FETCH_GET("/Channels/GetChannel" + "?id=" + channelId)
            .then(r => r.json())
            .then(data => setIsTextChannel(data.type === 0))
            .catch(error => console.log(error))
    }, [channelId]);

    const [conn, setConnection] = useState();

    function createHubConnection() {
        const newConnection = new HubConnectionBuilder()
            .withUrl(BACKEND_BASE_URL + "/chat?access_token=" + USER_TOKEN())
            .configureLogging(LogLevel.Information)
            .build();
        newConnection.on("JoinSpecificChat", (userId, msg) => {
            console.log("msg: ", msg)
            // setMessages(messages => [...messages, { userId, msg }])
        });

        setConnection(newConnection);
    }

    useEffect(() => {
        createHubConnection();
    }, [channelId]);

    useEffect(() => {
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
        };
    }, [conn]);

    return (isTextChannel
        ? <TextChannel conn={conn} />
        : <VoiceChannel conn={conn}/>)
}

export default Channel;