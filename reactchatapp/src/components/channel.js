import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';

const Channel = () => {
    const { serverId, channelId } = useParams();

    return <div>
        <h3>Channel: {channelId}</h3>
    </div>
}

export default Channel;