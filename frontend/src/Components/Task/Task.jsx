import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import React from "react";

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
    return(
        <div className="bg hover:bg-slate-500 p-2 rounded-lg mb-2"
            ref={setNodeRef}
            {...attributes}
            {...listeners}
            style={{
                transition,
                transform: CSS.Translate.toString(transform)
                }}>
            <p>{task.title}</p>
        </div>
    );
};

export default Task;