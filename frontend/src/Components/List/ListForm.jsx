import React, { useContext, useState } from "react";
import { MainContext } from "../../Pages/MainContext";
import { postData } from "../../Services/FetchService";
import { BoardContext } from "../BoardContent/Board";

const ListForm = ({ }) => {
    const mainContext = useContext(MainContext);
    const boardContext = useContext(BoardContext);
    const [listTitle, setListTitle] = useState("");
    const [boardId, setBoardId] = useState(mainContext ? mainContext.boardId : "");
    const [errorMessage, setErrorMessage] = useState("");

    const handleTitleChange = (e) => {
        setListTitle(e.target.value);
    };

    const handleCreateList = async (e) => {
        e.preventDefault();

        if (listTitle.length < 2 || listTitle.length > 20) {
            setErrorMessage("List title must be between 2 and 20 characters.");
            return;
        }

        const newList = {
            title: listTitle,
            boardId: boardId,
        };
        console.log('Creating list with data:', newList);

        try {
            const response = await postData("http://localhost:5157/backend/list/CreateList", newList);
            boardContext.setLists([... boardContext.lists, response.data]);
            setListTitle("");
        } catch (error) {
            console.log("Error response data: ", error.message);
        }
    };

    return (
        <div className="flex-shrink-0 p-4 h-auto w-[275px]">
            <form >
                <input
                    type="text"
                    placeholder="List Title"
                    name="listTitle"
                    id="listTitle"
                    value={listTitle}
                    onChange={handleTitleChange}
                    className="appearance-none bg-slate-400 text-black w-full p-2 mb-2 border-none rounded placeholder-gray-700 focus:outline-none focus:border-transparent focus:bg-slate-300"
                />

                <button
                    type="submit"
                    className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                    onClick={handleCreateList}
                >
                    Add List
                </button>

                {errorMessage && (
                    <p className="text-red-500">{errorMessage}</p>
                )}
            </form>
        </div>
    );
};

export default ListForm;