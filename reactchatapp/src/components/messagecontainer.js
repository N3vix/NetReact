import { useMemo } from 'react';
import { Button } from 'react-bootstrap';
import Moment from 'moment';

import { BACKEND_BASE_URL, USER_ID } from '../constants'

const MessageContainer = ({ messages, editMessage, deleteMessage }) => {

    const userId = useMemo(() => USER_ID());

    return <div>
        {
            messages.map((msg, index) =>
                <div style={{ backgroundColor: "#eee", margin: "10px 0px", borderRadius: 6 }}>
                    {userId === msg.senderId
                        ? <div style={{ float: 'right' }}>
                            <Button onClick={() => editMessage(msg.id)} style={{ marginRight: 5 }}>E</Button>
                            <Button onClick={() => deleteMessage(msg.id)}>X</Button>
                        </div>
                        : ""}
                    <div style={{ padding: 10 }}>
                        <div style={{ float: 'left', marginRight: 10 }}>{msg.senderId}</div>
                        <div >{Moment(msg.timestamp).format("D MMM YYYY hh:mm:ss")}</div>
                        <div style={{ marginLeft: 10 }}>{msg.content}</div>
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