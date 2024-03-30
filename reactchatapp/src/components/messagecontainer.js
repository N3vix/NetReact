import { useMemo, useState } from 'react';
import { Button, Form, InputGroup } from 'react-bootstrap';
import Moment from 'moment';

import { BACKEND_BASE_URL, USER_ID } from '../constants'

const MessageContainer = ({ messages, editMessage, deleteMessage }) => {

    const userId = useMemo(() => USER_ID());

    const [editMessageId, setEditMessageId] = useState(null);
    const [editMessageContent, setEditMessageContent] = useState(null);

    const startMessageEditing = async (messageId, messageContent) => {
        setEditMessageId(messageId);
        setEditMessageContent(messageContent);
    }

    const cancelMessageEditing = async () => {
        setEditMessageId(null);
        setEditMessageContent(null);
    }

    const applyUpdatedMessage = async () => {
        editMessage(editMessageId, editMessageContent)
        cancelMessageEditing();
    }

    return <div>
        {
            messages.map((msg, index) =>
                <div style={{ backgroundColor: "#eee", margin: "10px 0px", borderRadius: 6 }}>
                    {userId === msg.senderId
                        ? <div style={{ float: 'right' }}>
                            {editMessageId === msg.id
                                ? <Button onClick={() => cancelMessageEditing()} style={{ marginRight: 5, backgroundColor: "red" }}>E</Button>
                                : <Button onClick={() => startMessageEditing(msg.id, msg.content)} style={{ marginRight: 5 }}>E</Button>}

                            <Button onClick={() => deleteMessage(msg.id)}>X</Button>
                        </div>
                        : ""}
                    <div style={{ padding: 10 }}>
                        <div style={{ float: 'left', marginRight: 10 }}>{msg.senderId}</div>
                        <div>{Moment(msg.timestamp).format("D MMM YYYY hh:mm:ss")}</div>
                        {editMessageId === msg.id
                            ? <InputGroup className="mb-3">
                                <Form.Control
                                    onChange={e => setEditMessageContent(e.target.value)}
                                    value={editMessageContent}
                                    placeholder="edit..."></Form.Control>
                                <Button onClick={() => applyUpdatedMessage()} variant="primary" disabled={!editMessageContent}>Apply</Button>
                            </InputGroup>
                            : <div style={{ marginLeft: 10 }}>{msg.content}</div>}
                        {msg.editedTimestamp
                            ? <div
                                title={Moment(msg.editedTimestamp).format("D MMM YYYY hh:mm:ss")}
                                style={{ fontStyle: 'italic', fontWeight: 5, fontSize: 12 }}>(edited)</div>
                            : ""}
                        {msg.image
                            ? <img alt="preview image" src={`${BACKEND_BASE_URL}/Attachments/${msg.image}`}
                                style={{ marginLeft: 10, maxWidth: 400, maxHeight: 400 }} />
                            : ""}
                    </div>
                </div>)
        }
    </div>
}

export default MessageContainer;