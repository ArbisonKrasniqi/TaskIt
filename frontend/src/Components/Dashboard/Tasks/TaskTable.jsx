import { useState } from "react";
import ChecklistsList from "../Checklists/ChecklistsList";
import LabelsList from "../Labels/LabelsList";

const TaskTable = () => {

    const [activeList, setActiveList] = useState('checklists') //Default checklists
    
    return (
        <div className="flex flex-col h-full">
            <div className="flex mb-4">
                <button
                    className={`flex-1 px-4 py-2 border ${activeList === 'boards' ? 'bg-blue-600 text-white border-blue-600' : 'bg-gray-200 text-gray-800 border-gray-300'}`}
                    onClick={() => setActiveList('checklists')}>Checklists</button>
                <button
                    className={`flex-1 px-4 py-2 border ${activeList === 'labels' ? 'bg-blue-600 text-white border-blue-600' : 'bg-gray-200 text-gray-800 border-gray-300'}`}
                    onClick={() => setActiveList('labels')}>Labels</button>
                <button
                    className={`flex-1 px-4 py-2 border ${activeList === 'invites' ? 'bg-blue-600 text-white border-blue-600' : 'bg-gray-200 text-gray-800 border-gray-300'}`}
                    onClick={() => setActiveList('assignedUsers')}>Assigned Users</button>
            </div>

            {/* Render the selected list */}
            <div className="flex-grow overflow-auto">
                {activeList === 'checklists' && <ChecklistsList />}
                {activeList === 'labels' && <LabelsList />}
                {/*activeList === 'assignedUsers' && <InvitesList />*/}
            </div>
        </div>
    )
}

export default TaskTable;