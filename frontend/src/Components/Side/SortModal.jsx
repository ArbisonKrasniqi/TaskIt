import React from 'react';
import { FaSortAlphaDown, FaSortAmountDown } from 'react-icons/fa';

const SortModal = ({ open, onClose, selectedSort, onSortChange }) => {
  if (!open) return null;

  return (
    <div className="fixed z-50 inset-0 flex items-center justify-start p-4 ml-60 zIndex: 1000 text-black">
      <div className="bg-white border border-gray-300 rounded-lg shadow-lg w-50">
        <div className="p-3 flex justify-between items-center">
          <h3 className="text-gray-700 font-semibold">Sort Options</h3>
          <button onClick={onClose} className="text-gray-500 hover:text-gray-700">
            X
          </button>
        </div>
        <ul>
          <li
            onClick={() => {onSortChange('Alphabetically'); onClose();}}
            className={`p-2 m-1 cursor-pointer ${selectedSort === 'Alphabetically' ? 'bg-blue-300 hover:bg-blue-400' : ''} hover:bg-gray-500`}
          >
            <FaSortAlphaDown className="inline mr-2" />
            Sort Alphabetically
          </li>
          <li
            onClick={() =>{ onSortChange('Most Recent');  onClose(); }}
            className={`p-2 m-1 cursor-pointer ${selectedSort === 'Most Recent' ? 'bg-blue-300 hover:bg-blue-400' : ''} hover:bg-gray-500`}
          >
            <FaSortAmountDown className="inline mr-2" />
            Sort by Most Recent
          </li>
        </ul>
      </div>
    </div>
  );
};

export default SortModal;
