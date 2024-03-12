import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css'
import { BrowserRouter, Route, Routes, useParams } from 'react-router-dom';
import { Button } from 'react-bootstrap';
import LoginSignup from './components/loginsignup';
import Homepage from './components/homepage';

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
            <Route path=":id" Component={Child}/>
          </Route>
        </Routes>
      </BrowserRouter>
    </div>
  );
}

const Child = () => {
  // We can use the `useParams` hook here to access
  // the dynamic pieces of the URL.
  let { id } = useParams();

  return (
    <div>
      <h3>ID: {id}</h3>
    </div>
  );
}

export default App;
