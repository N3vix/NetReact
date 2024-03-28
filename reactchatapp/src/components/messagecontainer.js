import { BACKEND_BASE_URL } from '../constants'

const MessageContainer = ({ messages }) =>
    <div>
        {
            messages.map((msg, index) =>
                <div style={{backgroundColor: "#eee", margin: "10px 0px"}}>
                    <div>{msg.senderId}</div>
                    <div style={{marginLeft: 10}}>{msg.content}</div>
                    {msg.image
                        ? <img alt="preview image" src={`${BACKEND_BASE_URL}/Attachments/${msg.image}`} style={{ maxWidth: 400, maxHeight: 400 }} />
                        : ""}
                </div>)
        }
    </div>

export default MessageContainer;