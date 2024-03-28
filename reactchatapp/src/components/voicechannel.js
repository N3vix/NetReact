import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { Button } from 'react-bootstrap';

import Video from './video';

const VoiceChannel = ({ conn }) => {
    const { serverId, channelId } = useParams();

    const [peerConn, setPeerConn] = useState();
    const [streams, setStreams] = useState([]);

    const getPeerConnection = () => {
        const config = { iceServers: [{ urls: ['stun:stun.l.google.com:19302'] }] };
        return new RTCPeerConnection(config);
    }

    useEffect(() => {
        if (conn) {
            // setPc(getPeerConnection());
            conn.on("JoinVoiceChat", (desc, msg) => {
                if (desc.candidate) {
                    peerConn.addIceCandidate(desc);
                }
                else if (desc.sdp) {
                    desc.type = "offer";
                    peerConn.setRemoteDescription(desc);
                    peerConn.createAnswer()
                        .then((answer) =>  peerConn.setLocalDescription(answer))
                        .then(() => conn.invoke("SendAnswer", JSON.stringify(peerConn.localDescription)))
                }
            });
        }

        return () => {
        };
    }, [conn]);

    const join = async () => {
        const stream = await getUserMedia();
        // setStreams(streams => [...streams, stream])
        const tracks = stream.getTracks();
        tracks.forEach((track) => {
            peerConn.addTrack(track, stream)
        })
        peerConn.ontrack = evt => { 
            setStreams(streams => [...streams, new MediaStream([evt.track])])
            // setStreams(evt.streams)
        };
        peerConn.onicecandidate = evt => evt.candidate && conn.invoke("SendIce", JSON.stringify(evt.candidate))
        peerConn.onclose = () => { console.log("pc close"); };

        await conn.invoke("JoinVoiceChat");
    }

    const getUserMedia = async () => {
        const config = {
            audio: true,
            video: true
        }
        return await navigator.mediaDevices.getUserMedia(config);
    }

    return <div>
        <h3>Channel: {channelId}</h3>
        <Button onClick={join}>Join</Button>


        <nav className="flex-column">
            {streams.map((stream, index) =>
                <Video srcObject={stream} />)}
        </nav>
    </div>
}

export default VoiceChannel;