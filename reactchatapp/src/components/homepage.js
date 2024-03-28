import { useState, useEffect } from 'react';
import { Link, Outlet } from 'react-router-dom';
import { Button } from 'react-bootstrap';

import './homepage.css'
import { BACKEND_BASE_URL, USER_TOKEN } from '../constants'

const Homepage = () => {

    const [servers, setServers] = useState([]);

    useEffect(() => {
        fetch(BACKEND_BASE_URL + "/Servers/GetAddedServers", {
            method: "GET",
            headers: {
                "Authorization": "Bearer " + USER_TOKEN()
            }
        })
            .then(r => r.json())
            .then(data => setServers(data))
            .catch(error => console.log(error))
    }, [])

    const handleLogout = () => {
        localStorage.removeItem("accessToken");
        window.location.href = "/";
    };

    return <div className="page">
        <div className="sidebar">
            <div className="nav-scrollable">
                <nav className="flex-column">
                    <div className="nav-item px-3">
                        <Link className="nav-link" to="/">Home</Link>
                    </div>

                    {servers.map((server, index) =>
                        <div className="nav-item px-3">
                            <Link className="nav-link" key={index} to={server.id}>{server.name}</Link>
                        </div>)}


                    <Button variant="success" type="button" onClick={handleLogout}>Log out</Button>
                </nav>
            </div>
        </div>

        <main>
            <article className="content">
                <Outlet />
            </article>
        </main>
    </div>
}

export default Homepage;