import { useEffect, useRef } from 'react'

const Video = ({ srcObject }) => {

    const ref = useRef();
    useEffect(() => {
        ref.current.srcObject = srcObject;
    }, [srcObject]);

    return <video ref={ref} autoPlay playsInline style={{height: 200, width:200}}/>
}

export default Video;