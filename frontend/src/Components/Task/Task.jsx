import React from "react";

const Task = ({task, onDelete}) => {
    return(
        // <div>
        //     <h4 className="hover:bg-gray-100 p-2 rounded-lg mb-2">
        //         {task.title}
        //     </h4>
        //     {/* <p>{task.description}</p> */}
        // </div>
        <div className="bg hover:bg-slate-500 p-2 rounded-lg mb-2">

            <p>{task.title}</p>
            {/* <button onClick={() => onDelete(task.index)}
                className="text-red-500 hover:text-red-700">
                Delete
            </button> */}
        </div>
    );
};

export default Task;