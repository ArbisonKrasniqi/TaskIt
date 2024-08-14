import React, { useState } from "react";

const ListForm = ({onAddList}) =>{
    const [title, setTitle] = useState(null);

    const handleSubmit = (e) => {
        e.preventDefault();
        if(title.trim()){
            onAddList(title);
            setTitle('');
        }
    };

    return(
        <form onSubmit={handleSubmit}>

            <input type="text"
            placeholder="List Title"
            value={title}
            onChange={(e) => setTitle(e.target.value)} 
            className=" appearance-none bg-slate-400 text-black w-full p-2 mb-2 border-none rounded placeholder-gray-700 focus:outline-none focus:border-transparent focus:bg-slate-300"/>

            <button type="submit"
            className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600">
                Add List
            </button>
        </form>
    );
};
export default ListForm;