import { useState } from "react";
import { Button, Col, Form, Row } from "react-bootstrap";

const WaitingRoom = ({ joinChatRoom }) => {

    const [chatroom, setChatroom] = useState();

// joinChatRoom("57");

// return ""
    return <Form onSubmit={e => {
        e.preventDefault();
        joinChatRoom(chatroom)
    }}>

        <Row className="px-5 py-5">
            <Col sm={12}>
                <Form.Group>
                    <Form.Control
                        placeholder="Chatroom"
                        onChange={e => setChatroom(e.target.value)} />
                </Form.Group>
            </Col>
            <Col sm={12}>
                <hr />
                <Button variant="success" type="submit">Join</Button>
            </Col>
        </Row>

    </Form>
}

export default WaitingRoom;