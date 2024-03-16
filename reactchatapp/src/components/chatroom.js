import { Col, Row } from "react-bootstrap";
import MessageContainer from './messagecontainer';
import SendMessageForm from "./sendmessageform";

const ChatRoom = ({ messages, sendMessage }) =>
    <div>
        <Row>
            <Col sm={12}>
                <MessageContainer messages={messages} />
            </Col>
            <Col sm={12} style={{position: "sticky", bottom: 0}}>
                <SendMessageForm sendMessage={sendMessage} />
            </Col>
        </Row>
    </div>

export default ChatRoom;