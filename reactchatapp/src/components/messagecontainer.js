const MessageContainer = ({ messages }) =>
    <div>
        {
            messages.map((msg, index) =>
                <table stripped bordered>
                    <tr key={index}>
                        <td>{msg.msg} = {msg.username}</td>
                    </tr>
                </table>)
        }
    </div>

export default MessageContainer;