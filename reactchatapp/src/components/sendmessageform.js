import { useState, useRef } from "react"
import { Button, Form, InputGroup } from "react-bootstrap";

const SendMessageForm = ({ sendMessage }) => {

    const hiddenFileInput = useRef(null);

    const [msg, setMessage] = useState('');
    const [image, setImage] = useState(null);

    const openFileSelection = event => {
        hiddenFileInput.current.click();
    };

    const onFileSelectionChange = event => {
        setImage(event.target.files[0]);
    };

    return <Form onSubmit={e => {
        e.preventDefault();
        sendMessage(msg, image);
        setMessage('');
        setImage(null);
    }}>
        {image
            ? <img alt="preview image" src={URL.createObjectURL(image)} style={{ maxWidth: 400, maxHeight: 400 }} />
            : ""}
        <InputGroup className="mb-3">
            <InputGroup.Text>Chat</InputGroup.Text>
            <Button variant="primary" type="button" onClick={openFileSelection}>+</Button>
            <Form.Control onChange={e => setMessage(e.target.value)} value={msg} placeholder="type a msg"></Form.Control>
            <Button variant="primary" type="submit" disabled={!msg}>Send</Button>

            <input type="file" onChange={onFileSelectionChange} ref={hiddenFileInput} style={{ display: 'none' }} />
        </InputGroup>

    </Form>
}

export default SendMessageForm;