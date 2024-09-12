import { useContext, useState } from "react";
import { DashboardContext } from "../../../Pages/dashboard";
import { TaskActivityContext } from "./TaskActivityList";

const TaskActivityTable = () => {
    const [searchQuery, setSearchQuery] = useState('');
    const dashboardContext = useContext(DashboardContext);
    const taskActivityContext = useContext(TaskActivityContext);


    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value.toLowerCase());
    }

    const activities = taskActivityContext.activities || [];
    const filteredActivity = activities.filter(activity => {
        const activityIdMatch = activity.taskActivityId.toString().toLowerCase().includes(searchQuery);
        const taskIdMatch = activity.taskId.toString().toLowerCase().includes(searchQuery);
        const userIdMatch = activity.userId.toString().toLowerCase().includes(searchQuery);
        const userFirstNameMatch = activity.userFirstName.toString().toLowerCase().includes(searchQuery);
        const userLastNameMatch = activity.userLastName.toString().toLowerCase().includes(searchQuery);
        const actionTypeMatch = activity.actionType.toString().toLowerCase().includes(searchQuery);
        const entityNameMatch = activity.entityName.toString().toLowerCase().includes(searchQuery);
        const ActionDateMatch = activity.actionDate.toString().toLowerCase().includes(searchQuery);
        return activityIdMatch || taskIdMatch || userIdMatch || userFirstNameMatch || userLastNameMatch || actionTypeMatch || entityNameMatch || ActionDateMatch
    });
    return (
        <div className="flex flex-col h-full bg-gray-800 text-white">
            {/* Search Bar */}
            <div className="mb-4 pt-4 flex justify-center">
                <input
                    type="text"
                    placeholder="Search for activities"
                    value={searchQuery}
                    onChange={handleSearchChange}
                    className="p-2 border rounded w-[400px] bg-gray-700 text-white"
                />
            </div>

            {/* Scrollable Table Container */}
            <div className="flex-grow overflow-x-auto">
                <table className="w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400">
                    <thead className="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
                        <tr>
                            <th className="px-6 py-3">Activity ID</th>
                            <th className="px-6 py-3">Task ID</th>
                            <th className="px-6 py-3">User ID</th>
                            <th className="px-6 py-3">Activity</th>
                            <th className="px-6 py-3">Action Date</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredActivity.length > 0 ? (
                            filteredActivity.map((activity, index) => (
                                <tr key={index} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                    <td className="px-6 py-4">{activity.taskActivityId}</td>
                                    <td className="px-6 py-4">{activity.taskId}</td>
                                    <td className="px-6 py-4">{activity.userId}</td>
                                    <td className="px-6 py-4">{`${activity.userFirstName} ${activity.userLastName} ${activity.actionType} ${activity.entityName}`}</td>
                                    <td className="px-6 py-4">{activity.actionDate}</td>
                                </tr>
                            ))
                        ) : (
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                <td className="px-6 py-4" colSpan={7}>No activity found</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    )
    
}

export default TaskActivityTable;