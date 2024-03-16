import { useState, useEffect } from 'react';
import { useParams, Link, Outlet } from 'react-router-dom';

import { BACKEND_BASE_URL, USER_TOKEN } from '../constants'

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
    }, [serverId])

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
    </div>
}

export default Server;