import { useState } from "react";
import { useParams } from "react-router-dom";
import ListsList from "../Lists/ListsList";
import LabelsList from "../Labels/LabelsList";
import BoardActivityList from "./BoardActivityList";

const BoardTable = () => {
    const [activeList, setActiveList] = useState('lists');
    const { boardId } = useParams();

    return (
        <div className="flex flex-col h-full">
            <div className="flex mb-4">
                <button
                    className={`flex-1 px-4 py-2 border ${activeList === 'lists' ? 'bg-blue-600 text-white border-blue-600' : 'bg-gray-200 text-gray-800 border-gray-300'}`}
                    onClick={() => setActiveList('lists')}>Lists</button>
                <button
                    className={`flex-1 px-4 py-2 border ${activeList === 'labels' ? 'bg-blue-600 text-white border-blue-600' : 'bg-gray-200 text-gray-800 border-gray-300'}`}
                    onClick={() => setActiveList('labels')}>Labels</button>
                <button
                    className={`flex-1 px-4 py-2 border ${activeList === 'activity' ? 'bg-blue-600 text-white border-blue-600' : 'bg-gray-200 text-gray-800 border-gray-300'}`}
                    onClick={() => setActiveList('activity')}>Activities
                </button>
            </div>

            {/* Render the selected list */}
            <div className="flex-grow overflow-auto">
                {activeList === 'lists' && <ListsList />}
                {activeList === 'labels' && <LabelsList />}
                {activeList === 'activity' && <BoardActivityList/>}
            </div>
        </div>
    )
}

export default BoardTable;