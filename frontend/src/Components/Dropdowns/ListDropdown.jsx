import React, { useState, useContext } from "react";
import { WorkspaceContext } from "../Side/WorkspaceContext";
import { BiDotsHorizontal } from "react-icons/bi";
import { BoardContext } from "../BoardContent/Board";
import { deleteData } from "../../Services/FetchService";
import { useParams } from "react-router-dom";

const ListDropdown = ({ listId, onAddCardClick }) => {
  const {boardId} = useParams();
  const [isOpen, setIsOpen] = useState(false);
  const boardContext = useContext(BoardContext);
  const workspaceContext = useContext(WorkspaceContext);
  const handleDeleteList = async (listId) =>{
    if (boardId && workspaceContext.board) {
      try {
        const response = await deleteData('http://localhost:5157/backend/list/DeleteList',{listId : listId});
        boardContext.setLists(prevLists => prevLists.filter(l => l.listId != listId));
    } catch (error) {
        console.error('Error deleting list');
        boardContext.getLists();
        boardContext.getTasks();
    }
  }
  };

  const toggleDropdown = () => {
    setIsOpen(!isOpen);
  };

  const closeDropdown = () => {
    setIsOpen(false);
  };

  return (
    <div className="relative">
      <div
        className="hover:bg-gray-700 w-8 h-8 rounded-md grid place-content-center cursor-pointer"
        onClick={toggleDropdown}
      >
        <BiDotsHorizontal className="w-5 h-5 text-white" />
      </div>

      {isOpen && (
        <div className="absolute right-0 mt-2 bg-gray-800 rounded-md shadow-lg border-gray-900 w-40 z-10">
          <div
            onClick={() => {
              onAddCardClick();
              closeDropdown();
            }}
            className="block px-4 py-2 text-sm text-gray-500 hover:bg-gray-700 cursor-pointer"
          >
            Add card
          </div>
          <div
            onClick={() => {
              handleDeleteList(listId);
              closeDropdown();
            }}
            className="block px-4 py-2 text-sm text-red-600 hover:bg-gray-700 cursor-pointer"
          >
            Delete list
          </div>
        </div>
      )}
    </div>
  );
};

export default ListDropdown;

