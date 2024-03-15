import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css'
import { BrowserRouter, Route, Routes, useParams } from 'react-router-dom';
import { Button } from 'react-bootstrap';
import LoginSignup from './components/loginsignup';
import Homepage from './components/homepage';
import Server from './components/server';
import Channel from './components/channel';

function App() {
  const token = localStorage.getItem('accessToken');

  const handleLogout = () => {
    localStorage.removeItem("accessToken");
    window.location.href = "/";
  };

  if (!token) {
    return <LoginSignup />
  }

  return (
    <div>
      <Button variant="success" type="button" onClick={handleLogout}>Log out</Button>
      <BrowserRouter>
        <Routes>
          <Route path="/" Component={Homepage}>
            <Route path="/:serverId" Component={Server}>
              <Route path="/:serverId/:channelId" Component={Channel}/>
            </Route>
          </Route>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
