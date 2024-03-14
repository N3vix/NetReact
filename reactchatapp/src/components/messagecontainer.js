const MessageContainer = ({ messages }) =>
    <div>
        {
            messages.map((msg, index) =>
                <table stripped="true" bordered="true">
                    <tr key={index}>
                        <td>{msg.msg} = {msg.userId}</td>
                    </tr>
                </table>)
        }
    </div>

export default MessageContainer;