import { useContext, useState } from "react";
import { DashboardContext } from "../../../Pages/dashboard";
import { BoardActivityContext } from "./BoardActivityList";

const BoardActivityTable = () => {
    const [searchQuery, setSearchQuery] = useState('');
    const dashboardContext = useContext(DashboardContext);
    const boardActivityContext = useContext(BoardActivityContext);


    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value.toLowerCase());
    }

    const activities = boardActivityContext.activities || [];
    const filteredActivity = activities.filter(activity => {
        const activityIdMatch = activity.boardActivityId.toString().toLowerCase().includes(searchQuery);
        const boardIdMatch = activity.boardId.toString().toLowerCase().includes(searchQuery);
        const userIdMatch = activity.userId.toString().toLowerCase().includes(searchQuery);
        const userNameMatch = activity.userName.toString().toLowerCase().includes(searchQuery);
        const actionTypeMatch = activity.actionType.toString().toLowerCase().includes(searchQuery);
        const entityNameMatch = activity.entityName.toString().toLowerCase().includes(searchQuery);
        const ActionDateMatch = activity.actionDate.toString().toLowerCase().includes(searchQuery);
        return activityIdMatch || boardIdMatch || userIdMatch || userNameMatch || actionTypeMatch || entityNameMatch || ActionDateMatch
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
                            <th className="px-6 py-3">Board ID</th>
                            <th className="px-6 py-3">User ID</th>
                            <th className="px-6 py-3">Activity</th>
                            <th className="px-6 py-3">Action Date</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredActivity.length > 0 ? (
                            filteredActivity.map((activity, index) => (
                                <tr key={index} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                    <td className="px-6 py-4">{activity.boardActivityId}</td>
                                    <td className="px-6 py-4">{activity.boardId}</td>
                                    <td className="px-6 py-4">{activity.userId}</td>
                                    <td className="px-6 py-4">{`${activity.userName} ${activity.userLastName} ${activity.actionType} ${activity.entityName}`}</td>
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

export default BoardActivityTable;