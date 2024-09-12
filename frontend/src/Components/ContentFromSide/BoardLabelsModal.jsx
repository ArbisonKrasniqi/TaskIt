
import { MdOutlineEdit } from "react-icons/md";
import { useContext, useState } from "react";
import { BoardSettingsContext } from "./BoardSettings";
const BoardLabelsModal = () =>{
    
    const [selectedLabel, setSelectedLabel] = useState(null);
    const { toggleLabelsModal, toggleEditLabelModal, labels } = useContext(BoardSettingsContext);
   


    return(
    <div className="absolute  inset-0 flex items-center justify-center z-50 bg-black bg-opacity-50">
         <div className="bg-gray-900 w-1/4 p-5 rounded-md shadow-lg">
                <div className="flex justify-between items-center mb-4">
                    <h2 className="text-sm font-semibold text-gray-400">Labels</h2>
                    <button
                        onClick={toggleLabelsModal}
                        className="text-gray-500 hover:bg-gray-800 w-6 h-6 rounded-full flex justify-center items-center"
                    >
                        X
                    </button>
                </div>

                {labels.length > 0 && (
                    <div className="mb-4">
                        <h3 className="text-xs font-semibold text-gray-400 mb-">Labels:</h3>
                        {labels.map(label => (
                            <div key={label.labelId} className="flex items-center py-2 h-[50px] rounded-md mb-1">
                                 <span 
                                    className="text-sm font-medium rounded-sm text-white w-full h-full flex items-center pl-2 mx-1"
                                    style={{ backgroundColor: label.color }}
                                    title={`Color: ${label.color}, Title: "${label.name.length === 0 ? '' : label.name}"`}
                                >
                                    {label.name.length === 0 ? '' : label.name}
                                </span>
                                <button
                                    className="ml-auto text-xl text-gray-500 rounded-xs w-8 h-8 flex justify-center items-center hover:bg-gray-800"
                                    onClick={() => toggleEditLabelModal(label)}
                                >
                                    <MdOutlineEdit/>
                                </button>
                            </div>
                        ))}
                    </div>
                )}
        
        </div>
    </div>    
        );

}

export default BoardLabelsModal;