import { useState } from "react";
import { Button, Col, Form, Row } from "react-bootstrap";

const WaitingRoom = ({ joinChatRoom }) => {

    return <Form onSubmit={e => {
        e.preventDefault();
        joinChatRoom()
    }}>
        <Row className="px-5 py-5">
            <Col sm={12}>
                <Button variant="success" type="submit">Join</Button>
            </Col>
        </Row>

    </Form>
}

export default WaitingRoom;