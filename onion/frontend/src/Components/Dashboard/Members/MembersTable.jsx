import React, { useState, useContext, createContext } from 'react';
import { MembersContext } from "./MembersList";
import CustomButton from '../Buttons/CustomButton';
import { deleteData } from '../../../Services/FetchService';
import UpdateMemberButton from "./Buttons/UpdateMemberButton";
import { DashboardContext } from '../../../Pages/dashboard';

export const UpdateContext = createContext();

const MembersTable = () => {
    const membersContext = useContext(MembersContext);
    const dashboardContext = useContext(DashboardContext);
    const [searchQuery, setSearchQuery] = useState('');

    const formatDate = (dateStr) => {
        const date = new Date(dateStr);
        return isNaN(date.getTime()) ? 'Date not available' : date.toLocaleDateString();
    };

    const handleMemberDelete = (id) => {
        async function deleteMember() {
            try {
                const data = { MemberId: id };
                const response = await deleteData('/backend/Members/DeleteMember', data);
                console.log(response);
                const updatedMembers = membersContext.members.filter(member => member.memberId !== id);
                membersContext.setMembers(updatedMembers);
            } catch (error) {
                dashboardContext.setDashboardErrorMessage(error.message + id);
                dashboardContext.setShowDashboardErrorModal(true);
                membersContext.getMembers();
            }
        }
        deleteMember();
    };

    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value.toLowerCase());
    };

    // Ensure membersContext.members is always an array
    const members = membersContext.members || [];
    const filteredMembers = members.filter(member => {
        const memberIdMatch = member.memberId.toString().toLowerCase().includes(searchQuery);
        const userIdMatch = member.userId.toString().toLowerCase().includes(searchQuery);
        const dateJoinedMatch = formatDate(member.dateJoined).toLowerCase().includes(searchQuery);
        const workspaceIdMatch = member.workspaceId.toString().toLowerCase().includes(searchQuery);

        return memberIdMatch || userIdMatch || dateJoinedMatch || workspaceIdMatch;
    });

    return (
        <div className="flex flex-col h-full bg-gray-800 text-white">
            {/* Search Bar */}
            <div className="mb-4 pt-4 flex justify-center">
                <input
                    type="text"
                    placeholder="Search for members by ID, user ID, date joined, or workspace ID"
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
                            <th className="px-6 py-3">Member ID</th>
                            <th className="px-6 py-3">User ID</th>
                            <th className="px-6 py-3">Date Joined</th>
                            <th className="px-6 py-3">Workspace ID</th>
                            <th className="px-6 py-3">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredMembers.length > 0 ? (
                            filteredMembers.map((member, index) => (
                                <tr
                                    key={index}
                                    className="bg-white border-b dark:bg-gray-800 dark:border-gray-700"
                                >
                                    <td className="px-6 py-4">{member.memberId}</td>
                                    <td className="px-6 py-4">{member.userId}</td>
                                    <td className="px-6 py-4">{formatDate(member.dateJoined)}</td>
                                    <td className="px-6 py-4">{member.workspaceId}</td>
                                    <td className="px-6 py-4 flex space-x-2">
                                        <UpdateContext.Provider value={member}>
                                            <UpdateMemberButton />
                                        </UpdateContext.Provider>
                                        <CustomButton
                                            color="red"
                                            text="Delete"
                                            onClick={() => handleMemberDelete(member.memberId)}
                                        />
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                <td className="px-6 py-4" colSpan={5}>No members found</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default MembersTable;
