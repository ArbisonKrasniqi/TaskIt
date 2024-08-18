import { useContext } from "react"
import { WorkspaceContext } from "../Side/WorkspaceContext"



const DeleteWorkspaceModal = ({onClose}, roli) => {
    const {handleDeleteWorkspace,WorkspaceId, userId, handleLeaveWorkspace} = useContext(WorkspaceContext);

    return(
        <div className="fixed z-30 inset-0 flex justify-center items-center transition-colors bg-black/20">
        <div className="bg-white rounded-xl shadow p-6 text-center">
            {roli=="Owner" ? (
                <>
                <p className="text-red-500 font-bold text-l">Are you sure you want to delete this workspace?</p>
                <div className="mt-4 flex justify-center">
                    <button
                        onClick={() => {
                            handleDeleteWorkspace(WorkspaceId); 
                            onClose();
                        }}
                        className="bg-red-600 text-white px-4 py-2 rounded-md hover:bg-red-700 transition-colors duration-300 ease-in-out mr-4"
                    >
                        Delete
                    </button>
                    <button
                        onClick={onClose}
                        className="bg-gray-800 text-white px-4 py-2 rounded-md hover:bg-gray-900 transition-colors duration-300 ease-in-out"
                    >
                        Cancel
                    </button>
                </div> 
                </> ) :
                 (
                    <>
                    <p className="text-red-500 font-bold text-l">Are you sure you want to leave this workspace?</p>
                    <div className="mt-4 flex justify-center">
                        <button
                            onClick={() => {
                                handleLeaveWorkspace(WorkspaceId, userId); 
                                onClose();
                            }}
                            className="bg-red-600 text-white px-4 py-2 rounded-md hover:bg-red-700 transition-colors duration-300 ease-in-out mr-4"
                        >
                            Leave
                        </button>
                        <button
                            onClick={onClose}
                            className="bg-gray-800 text-white px-4 py-2 rounded-md hover:bg-gray-900 transition-colors duration-300 ease-in-out"
                        >
                            Cancel
                        </button>
                    </div> 
                    </>
                 )}
                
        </div>

        </div>

    );

};
export default DeleteWorkspaceModal;
