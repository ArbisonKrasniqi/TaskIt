import { useContext } from "react";
import { WorkspacesContext } from "../WorkspacesList";

const WorkspacesErrorModal = () => {
    const workspacesContext = useContext(WorkspacesContext);

    const handleCloseModal = () => {
        workspacesContext.setShowWorkspacesErrorModal(false);
    };

    return(
        <>
            <div className="fixed top-0 left-0 w-full h-full flex items-center justify-center z-50 bg-gray-900 bg-opacity-50">
            <div className="bg-white dark:bg-gray-700 dark:text-gray-400 p-8 rounded-lg shadow-md w-72 h-auto flex flex-col items-center justify-center">
            <p className="text-center text-gray-400 text-lg font-bold mb-4">{workspacesContext.errorMessage}</p>
            <button onClick={handleCloseModal} className="w-[50%] focus:outline-none text-white bg-red-700 hover:bg-red-800 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 me-2 mb-2 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900">Okay</button>
            </div>
            </div>
        
        </>
    );
}
export default WorkspacesErrorModal