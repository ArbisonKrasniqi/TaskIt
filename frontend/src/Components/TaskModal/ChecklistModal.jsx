import React,{ useState, useContext} from 'react'
import { TaskModalsContext } from './TaskModal'

function ChecklistModal() {
    const { toggleChecklistModal } = useContext(TaskModalsContext);
    

    

    return (
        <div className="absolute inset-0 flex items-center justify-end z-50 bg-black bg-opacity-50">
            <div className="bg-gray-900 w-1/3  p-5 rounded-md shadow-lg">
                <div className="flex justify-between items-center mb-4">
                    <h2 className="text-sm font-semibold text-gray-400">Add checklist</h2>
                    <button
                        onClick={toggleChecklistModal}
                        className="text-gray-500 hover:bg-gray-800 w-6 h-6 rounded-full flex justify-center items-center"
                    >
                        X
                    </button>
                </div>

                <div>
                <h3 className='text-gray-400 text-sm font-semibold'>Title</h3>
                <input
                    type="text"
                    placeholder="Checklist"
                    className="w-full pl-[5px] py-1 mb-4 bg-gray-900 border border-gray-700 rounded-sm text-white"
                />
                <button className='w-16 h-8 rounded-sm bg-blue-400 bg-opacity-90 text-gray-800 font-semibold hover:bg-opacity-80'>Add</button>
                </div>
            </div>
        </div>
    );
}

export default ChecklistModal;