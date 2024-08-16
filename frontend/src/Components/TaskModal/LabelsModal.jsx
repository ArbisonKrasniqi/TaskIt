import React,{ useState, useContext} from 'react'
import { TaskModalsContext } from './TaskModal'
import { MdOutlineEdit } from "react-icons/md";

function LabelsModal() {
    const { toggleLabelsModal } = useContext(TaskModalsContext);
    const [searchLabel, setSearchLabel] = useState("");
    const labels = [
        { id: 1, name: "Completed", color: "green", background: "bg-green-800 bg-opacity-80" },
        { id: 2, name: "", color: "yellow", background: "bg-yellow-400 bg-opacity-70"},
        { id: 3, name: "STUCK", color: "red", background: "bg-red-600"}
    ];

    const filterLabels = labels.filter( label =>
        label.name.toLowerCase().includes(searchLabel.toLowerCase()) ||
        label.color.toLowerCase().includes(searchLabel.toLowerCase())
    );

    return (
        <div className="absolute  inset-0 flex items-center justify-center z-50 bg-black bg-opacity-50">
            <div className="bg-gray-900 w-1/3 p-5 rounded-md shadow-lg">
                <div className="flex justify-between items-center mb-4">
                    <h2 className="text-sm font-semibold text-gray-400">Labels</h2>
                    <button
                        onClick={toggleLabelsModal}
                        className="text-gray-500 hover:bg-gray-800 w-6 h-6 rounded-full flex justify-center items-center"
                    >
                        X
                    </button>
                </div>

                <input
                    type="search"
                    value={searchLabel}
                    onChange={(e) => setSearchLabel(e.target.value)}
                    placeholder="Search labels"
                    className="w-full p-3 mb-4 bg-gray-900 border border-gray-700 rounded-sm text-white"
                />

                {filterLabels.length > 0 && (
                    <div className="mb-4">
                        <h3 className="text-xs font-semibold text-gray-400 mb-">Labels:</h3>
                        {filterLabels.map(labels => (
                            <div key={labels.id} className="flex items-center py-2 h-[50px] rounded-md mb-1">
                                <input 
                                    type="checkbox"
                                    className='w-6 h-6'/>
                                <span className={`text-sm font-medium rounded-sm text-white ${labels.background} w-full h-full flex items-center pl-2 mx-1`}
                                    title={`Color: ${labels.color}, title: "${(labels.name.length == 0 )? ("none") : (labels.name)}"`}>
                                    {labels.name}
                                </span>
                                <button
                                    className="ml-auto text-xl text-gray-500 rounded-xs w-8 h-8 flex justify-center items-center hover:bg-gray-800">
                                    <MdOutlineEdit/>
                                </button>
                            </div>
                        ))}
                    </div>
                )}
                <button className='w-full h-8 rounded-sm bg-gray-600 bg-opacity-25 text-gray-400 font-semibold hover:bg-opacity-50'>Create a new label</button>
            </div>
        </div>
    );
}

export default LabelsModal;