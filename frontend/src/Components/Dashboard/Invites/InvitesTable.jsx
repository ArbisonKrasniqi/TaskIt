import React, { useState, useContext, createContext } from "react";
import UpdateInviteButton from './Buttons/UpdateInviteButton';
import { InvitesContext } from './InvitesList';
import CustomButton from "../Buttons/CustomButton";
import { deleteData } from "../../../Services/FetchService";
import { DashboardContext } from "../../../Pages/dashboard";

export const UpdateContext = createContext();

const InvitesTable = () => {
    const invitesContext = useContext(InvitesContext);
    const [searchQuery, setSearchQuery] = useState('');
    const dashboardContext = useContext(DashboardContext);

    const formatDate = (dateStr) => {
        const date = new Date(dateStr);
        return isNaN(date.getTime()) ? 'Date not available' : date.toLocaleDateString();
    };

    const handleInviteDelete = (id) => {
        async function deleteInvite() {
            try {
                const data = { InviteId: id };
                const response = await deleteData('/backend/invite/DeleteInviteById', data);
                console.log(response.data);
                const updatedInvites = invitesContext.invites.filter(invite => invite.inviteId !== id);
                invitesContext.setInvites(updatedInvites);
            } catch (error) {
                dashboardContext.setDashboardErrorMessage(error.message + id);
                dashboardContext.setShowDashboardErrorModal(true);
                invitesContext.getInvites();
            }
        }
        deleteInvite();
    };

    const handleSearchChange = (event) => {
        setSearchQuery(event.target.value.toLowerCase());
    };

    // Ensure invitesContext.invites is always an array
    const invites = invitesContext.invites || [];
    const filteredInvites = invites.filter(invite => {
        const inviteIdMatch = invite.inviteId.toString().toLowerCase().includes(searchQuery);
        const workspaceIdMatch = invite.workspaceId.toString().toLowerCase().includes(searchQuery);
        const inviterIdMatch = invite.inviterId.toString().toLowerCase().includes(searchQuery);
        const inviteeIdMatch = invite.inviteeId.toString().toLowerCase().includes(searchQuery);
        const inviteStatusMatch = invite.inviteStatus.toLowerCase().includes(searchQuery);
        const dateSentMatch = formatDate(invite.dateSent).toLowerCase().includes(searchQuery);

        return inviteIdMatch || workspaceIdMatch || inviterIdMatch || inviteeIdMatch || inviteStatusMatch || dateSentMatch;
    });

    return (
        <div className="flex flex-col h-full bg-gray-800 text-white">
            {/* Search Bar */}
            <div className="mb-4 pt-4 flex justify-center">
                <input
                    type="text"
                    placeholder="Search for invites by ID, workspace ID, inviter ID, invitee ID, status, or date sent"
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
                            <th className="px-6 py-3">Invite ID</th>
                            <th className="px-6 py-3">Workspace ID</th>
                            <th className="px-6 py-3">Inviter ID</th>
                            <th className="px-6 py-3">Invitee ID</th>
                            <th className="px-6 py-3">Invite Status</th>
                            <th className="px-6 py-3">Date Sent</th>
                            <th className="px-6 py-3">Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {filteredInvites.length > 0 ? (
                            filteredInvites.map((invite, index) => (
                                <tr key={index} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                    <td className="px-6 py-4">{invite.inviteId}</td>
                                    <td className="px-6 py-4">{invite.workspaceId}</td>
                                    <td className="px-6 py-4">{invite.inviterId}</td>
                                    <td className="px-6 py-4">{invite.inviteeId}</td>
                                    <td className="px-6 py-4">{invite.inviteStatus}</td>
                                    <td className="px-6 py-4">{formatDate(invite.dateSent)}</td>
                                    <td className="px-6 py-4 flex space-x-2">
                                        <UpdateContext.Provider value={invite}>
                                            <UpdateInviteButton />
                                        </UpdateContext.Provider>
                                        <CustomButton
                                            color="red"
                                            text="Delete"
                                            onClick={() => handleInviteDelete(invite.inviteId)}
                                        />
                                    </td>
                                </tr>
                            ))
                        ) : (
                            <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                                <td className="px-6 py-4" colSpan={7}>No invites found</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default InvitesTable;
