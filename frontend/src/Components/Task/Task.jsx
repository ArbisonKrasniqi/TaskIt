import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import React from "react";
import { IoCloseSharp } from "react-icons/io5";
import { BoardContext } from "../BoardContent/Board";
import { deleteData } from "../../Services/FetchService";
import { useContext } from "react";
import { WorkspaceContext } from "../Side/WorkspaceContext";

const Task = ({task}) => {
    const {
        attributes,
        listeners,
        setNodeRef,
        transform,
        transition,
        isDragging,
    } = useSortable({
        id: task.uniqueId,
        data: {
            type: 'task',
        },
    });
    
    const workspaceContext = useContext(WorkspaceContext);
    const boardContext = useContext(BoardContext);
    const labels = task.labels;
    const members = task.taskMembers;

    
    return(
        <div className={`flex justify-between items-center bg-gray-900 text-white hover:bg-slate-600 w-full border-black p-0 h-auto rounded-lg mb-2 ${isDragging ? 'opacity-0' : ''} group`}
            ref={setNodeRef}
            {...attributes}
            style={{
                transition,
                transform: CSS.Translate.toString(transform)
                }}>

            {/*labels title and members*/}                    
            <div  {...listeners} className="w-full h-auto m-0 pl-3 p-0 flex items-center flex-col">
                <div className="pl-0 pr-0.5 pt-2 w-full flex flex-row flex-wrap">
                    {labels && labels.map((label, index) => (
                        <div
                        key={index}
                        className="h-[8px] w-[40px] rounded-[4px] m-0 mr-1 mb-1"
                        style={{ backgroundColor: label.color }} // Assuming each label has a color property
                        />
                    ))}
                </div>
                <div className="w-full pl-0 pt-1 p-2 min-h-[40px] h-auto">{task.title}</div>
                <div className="w-full h-auto flex justify-start flex-row flex-wrap">
                    {members && members.map((member, index) => (
                        <div
                        key={member.id}
                        className="flex-shrink-0 w-8 h-8 rounded-full overflow-hidden mb-2 mr-2 bg-gradient-to-r from-pink-400 to-purple-600 flex items-center justify-center text-white text-sm"
                    >
                        {workspaceContext.getInitials(member.firstName, member.lastName)}
                    </div>
                    ))}  
                </div>
            </div>

            {/*Delete*/} 
            <div className="text-gray-500 w-[25px] h-auto flex justify-center items-center pl-0 pt-0 pb-0 pr-2 opacity-0 group-hover:opacity-100 transition-opacity duration-150"> <IoCloseSharp onClick={(e) => {e.stopPropagation(); boardContext.handleTaskDelete(task.taskId);}}className="hover:text-red-500 w-7 h-7 transition-colors duration-150"/></div>
        </div>
    );
};

export default Task;