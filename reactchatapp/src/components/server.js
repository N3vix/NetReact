import { useState, useEffect } from 'react';
import { useParams, Link, Outlet } from 'react-router-dom';

import { FETCH_GET } from '../constants'

const Server = () => {
    const { serverId } = useParams();

    const [channels, setChannels] = useState([]);

    useEffect(() => {
        FETCH_GET("/Channels/GetChannels" + "?serverId=" + serverId)
            .then(r => r.json())
            .then(data => setChannels(data))
            .catch(error => console.log(error))
    }, [serverId])

    return <div>

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