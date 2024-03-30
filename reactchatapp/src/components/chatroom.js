import { Col, Row } from "react-bootstrap";
import MessageContainer from './messagecontainer';
import SendMessageForm from "./sendmessageform";

const ChatRoom = ({ messages, sendMessage, editMessage, deleteMessage }) =>
    <div>
        <Row>
            <Col sm={12}>
                <MessageContainer messages={messages} editMessage={editMessage} deleteMessage={deleteMessage} />
            </Col>
            <Col sm={12} style={{position: "sticky", bottom: 0}}>
                <SendMessageForm sendMessage={sendMessage} />
            </Col>
        </Row>
    </div>

export default ChatRoom;