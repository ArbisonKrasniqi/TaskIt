import React,{ useRef, useEffect, useContext} from 'react'
import { TaskModalsContext } from './TaskModal'
import { FaAngleLeft } from "react-icons/fa6";

function CreateLabelModal() {
    const { toggleCreateLabelModal, toggleLabelsModal } = useContext(TaskModalsContext);
    const inputRef = useRef(null);
    const colors = [
        { id: 1, name: "Deep Blue", color: "bg-blue-900" },
        { id: 2, name: "Soft Yellow", color: "bg-yellow-300" },
        { id: 3, name: "Muted Red", color: "bg-red-500" },
        { id: 4, name: "Teal Green", color: "bg-teal-500" },
        { id: 5, name: "Vibrant Orange", color: "bg-orange-500" },
        { id: 6, name: "Subtle Purple", color: "bg-purple-400" },
        { id: 7, name: "Warm Pink", color: "bg-pink-400" },
        { id: 8, name: "Emerald Green", color: "bg-green-500" }
    ];
    

    useEffect(() => {
        if (inputRef.current) {
            inputRef.current.focus();
        }
    }, []);
    
    return (
        <div className="absolute  inset-0 flex items-center justify-center z-50 bg-black bg-opacity-50">
            <div className="bg-gray-900 w-1/3 p-5 rounded-md shadow-lg">
                <div className="flex justify-between items-center mb-4">
                    <button className="text-gray-500 hover:bg-gray-800 w-6 h-6 rounded-full flex justify-center items-center"
                    onClick={toggleLabelsModal}>
                        <FaAngleLeft/>
                    </button>
                    <h2 className="text-sm font-semibold text-gray-400">Labels</h2>
                    <button
                        className="text-gray-500 hover:bg-gray-800 w-6 h-6 rounded-full flex justify-center items-center"
                        onClick={toggleCreateLabelModal}
                    >
                        X
                    </button>
                </div>

                <h3 className='text-gray-400 text-sm font-semibold'>Title</h3>
                <input
                    type="text"
                    ref={inputRef}
                    className="w-full pl-[5px] py-1 mb-4 bg-gray-900 border border-gray-700 rounded-sm text-white"
                />
                <h3 className='text-gray-400 text-sm font-semibold'>Select a color</h3>
                <div className="flex flex-wrap gap-2 mb-4">
                    {colors.map((color) => (
                        <div
                            key={color.id}
                            className={`w-11 h-8 rounded-sm cursor-pointer ${color.color}`}
                            title={color.name}
                            onClick={() => console.log(`Selected color: ${color.name}`)} // Add your color selection logic here
                        />
                    ))}
                </div>
                
                <button className='w-1/3 h-8 rounded-sm bg-blue-600 text-gray-900 font-semibold hover:bg-opacity-50'>Create</button>
            </div>
        </div>
    );
}

export default CreateLabelModal;