import React, { useState } from 'react';
import { IoMdCheckboxOutline } from 'react-icons/io';
import { FaEllipsisH } from 'react-icons/fa';

const Checklist = () => {
  const [checked, setChecked] = useState({ checkbox1: false, checkbox2: false });

  const handleChange = (event) => {
    setChecked({ ...checked, [event.target.name]: event.target.checked });
  };

  return (
    <div className="my-4">
      <div className="flex flex-row">
        <div className="w-1/12 h-6 justify-center flex items-center my-2 ml-4 mr-3 text-2xl ">
          <IoMdCheckboxOutline />
        </div>
        <div className="flex flex-row justify-between w-11/12">
          <span className="h-10 items-center flex font-semibold">Checklist</span>
          <button className="font-semibold p-2 h-10 rounded-[4px] text-sm bg-gray-600 bg-opacity-30 w-[70px]">
            Delete
          </button>
        </div>
      </div>
      <div className="flex flex-row items-center">
        <label className="w-1/12 h-6 justify-center flex items-center my-2 ml-4 mr-3">
          <input
            name="checkbox1"
            checked={checked.checkbox1}
            onChange={handleChange}
            type="checkbox"
            className="hidden"
          />
          <div
            className={`w-5 h-5 rounded border-2 flex items-center justify-center transition-colors ${
              checked.checkbox1
                ? 'bg-blue-200 border-blue-200 text-gray-800' // Customize the background and tick color when checked
                : 'bg-gray-800 border-gray-700'
            }`}
          >
            {checked.checkbox1 && (
              <svg
                className="w-4 h-4"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="4"
                  d="M5 13l4 4L19 7"
                />
              </svg>
            )}
          </div>
        </label>
        <div className="flex items-center w-11/12">
          <span
            className={`text-sm flex my-2 items-center justify-between w-[100%] hover:bg-gray-700 rounded-xl p-2 ${
              checked.checkbox1 ? 'line-through' : ''
            }`}
          >
            Sidebar
            <button className="w-[5%] h-6 rounded-full flex justify-center items-center text-sm bg-gray-600 bg-opacity-75 outline-white">
              <FaEllipsisH />
            </button>
          </span>
        </div>
      </div>
      <div className="flex flex-row items-center">
        <label className="w-1/12 h-6 justify-center flex items-center my-2 ml-4 mr-3">
          <input
            name="checkbox2"
            checked={checked.checkbox2}
            onChange={handleChange}
            type="checkbox"
            className="hidden"
          />
          <div
            className={`w-5 h-5 rounded border-2 flex items-center justify-center transition-colors ${
              checked.checkbox2
                ? 'bg-blue-200 border-blue-200 text-gray-800' // Customize the background and tick color when checked
                : 'bg-gray-800 border-gray-700'
            }`}
          >
            {checked.checkbox2 && (
              <svg
                className="w-4 h-4"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="4"
                  d="M5 13l4 4L19 7"
                />
              </svg>
            )}
          </div>
        </label>
        <div className="flex items-center w-11/12">
          <span
            className={`text-sm flex my-2 items-center justify-between w-[100%] hover:bg-gray-700 rounded-xl p-2 ${
              checked.checkbox2 ? 'line-through' : ''
            }`}
          >
            NavBar
            <button className="w-[5%] h-6 rounded-full flex justify-center items-center text-sm bg-gray-600 bg-opacity-75 outline-white">
              <FaEllipsisH />
            </button>
          </span>
        </div>
      </div>
    </div>
  );
};

export default Checklist;
