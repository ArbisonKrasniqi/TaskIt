import { useContext, useState } from "react";
import { TaskMembersContext } from "./TaskMemberList";
import { DashboardContext } from "../../../Pages/dashboard";
import { deleteData } from "../../../Services/FetchService";
import CustomButton from "../Buttons/CustomButton";

const TaskMemberTable = () => {
    const taskMembersContext = useContext(TaskMembersContext); 
    const dashboardContext = useContext(DashboardContext);
    const [searchQuery, setSearchQuery] = useState('');

    const HandleTaskMemberDelete = (id) => {
        async function deleteTaskMember() {
            try {
                const data = {taskMemberId: id};
                const response = await deleteData('/backend/TaskMembers/DeleteMember', data);
                console.log(response);

                const updatedTaskMembers = (taskMembersContext.taskMembers || []).filter(taskMember => taskMember.taskMemberid != id);
                taskMembersContext.setTaskMembers(updatedTaskMembers);
            } catch (error) {
                dashboardContext.setDashboardErrorMessage(error.message + id);
                dashboardContext.setShowDashboardErrorModal(true);
                taskMembersContext.getTaskMembers();
            }
        }
        deleteTaskMember();
    }

    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value.toLowerCase());
    }

    const taskMembers = taskMembersContext.taskMembers || [];
    const filteredTaskMembers = taskMembers.filter(taskMember => {
        const taskMemberIdMatch = taskMember.taskMemberId.toString().toLowerCase().includes(searchQuery);
        const userIdMatch = taskMember.userId.toString().toLowerCase().includes(searchQuery);
        const taskIdMatch = taskMember.taskId.toString().toLowerCase().includes(searchQuery);
        const dateJoinedMatch = taskMember.dateJoined.toString().toLowerCase().includes(searchQuery);

        return taskMemberIdMatch || userIdMatch || taskIdMatch || dateJoinedMatch;
    });

    return (
        <div className="overflow-x-auto">
            {/* Search Bar */}
            <div className="mb-4 pt-4 flex justify-center">
                <input
                    type="text"
                    placeholder="Search for workspaces by ID, title, description, or owner ID"
                    value={searchQuery}
                    onChange={handleSearchChange}
                    className="p-2 border rounded w-[400px] bg-gray-700 text-white"
                />
            </div>

            <div className="relative overflow-x-auto max-h-[500px]">
                <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
                    <thead className="sticky top-0 text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                        <tr>
                            <th className="px-6 py-3">TaskMember ID</th>
                            <th className="px-6 py-3">Task Id</th>
                            <th className="px-6 py-3">User Id</th>
                            <th className="px-6 py-3">Date Assigned</th>
                            <th className="px-6 py-3">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredTaskMembers.length > 0 ? (
                            filteredTaskMembers.map((taskMember, index) => (
                                <tr
                                    key={index}
                                    className="bg-white border-b dark:bg-gray-800 dark:border-gray-700"
                                >
                                    <td className="px-6 py-4">{taskMember.taskMemberId}</td>
                                    <td className="px-6 py-4">{taskMember.taskId}</td>
                                    <td className="px-6 py-4">{taskMember.userId}</td>
                                    <td className="px-6 py-4">{taskMember.dateJoined}</td>
                                    <td className="px-6 py-4">
                                        <CustomButton
                                            color="red"
                                            text="Delete"
                                            onClick={() => {
                                                HandleTaskMemberDelete(taskMember.taskMemberId);
                                            }}
                                        />
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                <td className="px-6 py-4" colSpan={5}>No task members found</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    );
}





export default TaskMemberTable;