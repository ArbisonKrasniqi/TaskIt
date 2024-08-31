import { useContext } from "react";
import { TasksContext } from "../TasksList";
import CustomButton from "../../Buttons/CustomButton";

const TasksErrorModal = () => {
    const tasksContext = useContext(TasksContext);

    const handleCloseModal = () => {
        tasksContext.setShowTasksErrorModal(false);
    };

    return(
        <>
            <div className="fixed top-0 left-0 w-full h-full flex items-center justify-center z-50 bg-gray-900 bg-opacity-50">
            <div className="bg-white dark:bg-gray-700 dark:text-gray-400 p-8 rounded-lg shadow-md w-72 h-auto flex flex-col items-center justify-center">
            <p className="text-center text-gray-400 text-lg font-bold mb-4">{tasksContext.errorMessage}</p>
            <CustomButton onClick={handleCloseModal} type="button" text="Okay" color="longRed"/>
            </div>
            </div>
        </>
    );
}

export default TasksErrorModal