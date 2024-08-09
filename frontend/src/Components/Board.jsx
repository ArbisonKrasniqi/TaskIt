import { useParams } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { getDataWithId } from '../Services/FetchService';
import { useLocation } from 'react-router-dom';

const Board = () => {
    const { id } = useParams();
    const [board, setBoard] = useState(null);
    const location = useLocation();

    const fetchBoard = async (id) => {
        const boardData = await getDataWithId('/backend/board/GetBoardByID?id', id);
        const data = boardData.data;
        console.log(data);
        setBoard(data);
    }

    useEffect(() => {
        fetchBoard(id);
    }, [id])

        if (!board) return <div>Loading...</div>;
    return (
        <div>
            <h1>Board {board.title}</h1>
            <h1>{location.pathname}</h1>
        </div>
    );
}

export default Board;