import { useState } from "react";
import { Container, Form, Button } from "react-bootstrap";

import "./loginsignup.css"
import user_icon from "./assets/person.png";
import email_icon from "./assets/email.png";
import password_icon from "./assets/password.png";
import "./homepage.css"
import { BACKEND_BASE_URL, USER_TOKEN_KEY, USER_ID_KEY } from '../constants'

const LoginSignup = () => {

    const signUpActionName = "Sign Up";
    const loginActionName = "Login";
    const [action, setAction] = useState(signUpActionName);
    const [error, setError] = useState();

    const [username, setUsername] = useState();
    const [email, setEmail] = useState();
    const [password, setPassword] = useState();

    return <Container className="container">
        <div className="header">
            <div className="text">{action}</div>
            <div className="underline"></div>
        </div>
        <Form className="inputs" onSubmit={e => {
            e.preventDefault();
            OnSignUp();
        }}>
            {action === loginActionName
                ? ""
                : <Form.Group className="input">
                    <img src={user_icon} alt="" />
                    <Form.Control type="text" placeholder="Name" onChange={e => setUsername(e.target.value)} />
                </Form.Group>}

            <Form.Group className="input">
                <img src={email_icon} alt="" />
                <Form.Control type="email" placeholder="Email" onChange={e => setEmail(e.target.value)} />
            </Form.Group>
            <Form.Group className="input">
                <img src={password_icon} alt="" />
                <Form.Control type="password" placeholder="Password" onChange={e => setPassword(e.target.value)} />
            </Form.Group>

            {action === signUpActionName
                ? ""
                : <div className="forgot-password">Lost password? <span>Click Here!</span></div>}
            <div className="submit-container">
                <Button
                    className={action === loginActionName ? "submit gray" : "submit"}
                    type={action === loginActionName ? "button" : "submit"}
                    onClick={() => setAction(signUpActionName)}>
                    {signUpActionName}
                </Button>
                <Button
                    className={action === signUpActionName ? "submit gray" : "submit"}
                    type={action === signUpActionName ? "button" : "submit"}
                    onClick={() => setAction(loginActionName)}>
                    {loginActionName}
                </Button>
            </div>
            {error
                ? <p>{error}</p>
                : ""}
        </Form>
    </Container>

    async function OnSignUp() {
        setError(null);
        if (action === signUpActionName) {
            const response = await SignUp();
            if (response.success) {
                localStorage.setItem(USER_TOKEN_KEY, response['token']);
                localStorage.setItem(USER_ID_KEY, response['userId']);
                window.location.href = "/";
                // localStorage.setItem('user', JSON.stringify(response['user']));
            }
            else {
                setError(response.errors[0]);
            }
        } else {
            const response = await Login();
            if (response.success) {
                localStorage.setItem(USER_TOKEN_KEY, response['token']);
                localStorage.setItem(USER_ID_KEY, response['userId']);
                window.location.href = "/";
                // localStorage.setItem('user', JSON.stringify(response['user']));
            }
            else {
                setError(response.errors[0]);
            }
        }
    }

    async function SignUp() {
        return fetch(BACKEND_BASE_URL + "/AuthManagement/Register", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ name: username, email, password })
        }).then(data => data.json())
    }

    async function Login() {
        return fetch(BACKEND_BASE_URL + "/AuthManagement/Login", {
            method: "POST",
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ email, password })
        }).then(data => data.json())
    }
}

export default LoginSignup;