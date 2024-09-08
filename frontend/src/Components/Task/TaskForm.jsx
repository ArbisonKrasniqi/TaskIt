import React, { useState } from "react";

const TaskForm = ({onAddTask}) => {
    const[title, setTitle] = useState('');
    // const[description, setDescription] = useState(null);

    const handleSubmit = (e) =>{
        e.preventDefault();
        if(title.trim()){
            onAddTask({index: Date.now(), title});
            setTitle('');
            // setDescription('');
        }
    };

    return(
        <form onSubmit={handleSubmit}>
            <input type="text"
            placeholder="New task" 
            color="black"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            className="bg-slate-400 border-none w-full p-2 mb-2 border rounded text-black placeholder-gray-700 focus:outline-none focus:border-transparent focus:bg-slate-300"

            />

            {/* <textarea 
            placeholder="Description"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            /> */}

            <button type="submit"
            className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
            >
                Add Task
            </button>
        </form>
    );
};
export default TaskForm;