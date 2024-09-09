import React, { useState, useContext } from "react";
import { WorkspaceContext } from "../Side/WorkspaceContext";
import { BiDotsHorizontal } from "react-icons/bi";
import { BoardContext } from "../BoardContent/Board";
import { deleteData } from "../../Services/FetchService";

const ListDropdown = ({ listId, onAddCardClick }) => {
  const [isOpen, setIsOpen] = useState(false);
  const boardContext = useContext(BoardContext);
  const handleDeleteList = async (listId) =>{
    try {
        const response = await deleteData('http://localhost:5157/backend/list/DeleteList',{listId : listId});
        boardContext.getLists();
    } catch (error) {
        console.error('Error deleting list:', error.message);
        boardContext.getLists();
        boardContext.getTasks();
    }
  };

  const toggleDropdown = () => {
    setIsOpen(!isOpen);
  };
  boardContext.getLists();
  const closeDropdown = () => {
    setIsOpen(false);
  };

  return (
    <div className="relative">
      <div
        className="hover:bg-gray-300 w-8 h-8 rounded-md grid place-content-center cursor-pointer"
        onClick={toggleDropdown}
      >
        <BiDotsHorizontal className="w-5 h-5 text-white" />
      </div>

      {isOpen && (
        <div className="absolute right-0 mt-2 bg-white rounded-md shadow-lg border w-40 z-10">
          <div
            onClick={() => {
              onAddCardClick();
              closeDropdown();
            }}
            className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 cursor-pointer"
          >
            Add card
          </div>
          <div
            onClick={() => {
              handleDeleteList(listId);
              closeDropdown();
            }}
            className="block px-4 py-2 text-sm text-red-600 hover:bg-gray-100 cursor-pointer"
          >
            Delete list
          </div>
        </div>
      )}
    </div>
  );
};

export default ListDropdown;

