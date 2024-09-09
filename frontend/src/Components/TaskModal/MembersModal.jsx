import React, { useContext, useState } from "react";
import { TaskModalsContext } from "./TaskModal";


const MembersModal = ({ taskId }) => {
    const { toggleMembersModal, assignedMembers, setAssignedMembers } = useContext(TaskModalsContext);
    const { memberDetails, getInitials } = useContext(WorkspaceContext);
    const [searchTerm, setSearchTerm] = useState("");
    const [originalBoardMembers, setOriginalBoardMembers] = useState(memberDetails || []);  // Per me i rujt members mos me i prek direkt
    const [filteredBoardMembers, setFilteredBoardMembers] = useState([]);

    useEffect(() => {
        const updatedBoardMembers = originalBoardMembers.filter(boardMember =>
            !assignedMembers.some(taskMember => taskMember.id === boardMember.id)
        );
        setFilteredBoardMembers(updatedBoardMembers);
    }, [assignedMembers, originalBoardMembers]);

    const addMemberToTask = async (member) => {
        try {
            const data = {
                userId: member.id,
                taskId: 1,
            };
            await postData('http://localhost:5157/backend/TaskMembers/AddTaskMember', data);
            
            // Shtoje anetarin ne assignedMembers edhe largoje prej filteredBoardMembers
            setAssignedMembers([...assignedMembers, member]);
        } catch (error) {
            console.error("Error adding member to task", error);
        }
    };

    const removeMemberFromTask = async (member) => {
        try {
            const userId = member.id;
            await deleteData(`http://localhost:5157/backend/TaskMembers/RemoveTaskMember?userId=${userId}&taskId=1`);
            
            // Largoje prej assignedMembers edhe shto prap ne filteredBoardMembers
            setAssignedMembers(assignedMembers.filter(m => m.id !== member.id));
        } catch (error) {
            console.error("Error removing member from task", error);
        }
    };

    const filteredTaskMembers = assignedMembers.filter(member => {
        const fullName = `${member.firstName?.toLowerCase()} ${member.lastName?.toLowerCase()}`;
        return fullName.includes(searchTerm.toLowerCase());
    });
    
    const boardMembersToShow = filteredBoardMembers.filter(member => {
        const fullName = `${member.firstName?.toLowerCase()} ${member.lastName?.toLowerCase()}`;
        return fullName.includes(searchTerm.toLowerCase());
    });
    

    const filteredBoardMembers = boardMembers.filter(member =>
        member.name.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <div className="absolute  inset-0 flex items-center justify-center z-50 bg-black bg-opacity-50">
            <div className="bg-gray-900 w-1/3 p-5 rounded-md shadow-lg">
                <div className="flex justify-between items-center mb-4">
                    <h2 className="text-sm font-semibold text-gray-400">Members</h2>
                    <button
                        onClick={toggleMembersModal}
                        className="text-gray-500 hover:bg-gray-800 w-6 h-6 rounded-full flex justify-center items-center"
                    >
                        X
                    </button>
                </div>

                <input
                    type="search"
                    value={searchTerm}
                    onChange={(e) => setSearchTerm(e.target.value)}
                    placeholder="Search members"
                    className="w-full p-3 mb-4 bg-gray-900 border border-gray-700 rounded-sm text-white"
                />

                {filteredCardMembers.length > 0 && (
                    <div className="mb-4">
                        <h3 className="text-xs font-semibold text-gray-400 mb-2">Task Members:</h3>
                        {filteredCardMembers.map(member => (
                            <div key={member.id} className="flex items-center p-2 hover:bg-gray-800 rounded-md mb-2">
                                <img src={member.avatar} alt={member.name} className="w-8 h-8 rounded-full mr-3" />
                                <span className="text-sm font-medium text-white">{member.name}</span>
                                <button
                                    onClick={() => removeMemberFromCard(member)}
                                    className="ml-auto text-xs text-gray-500"
                                >
                                    X
                                </button>
                            </div>
                        ))}
                    </div>
                )}

                {filteredBoardMembers.length > 0 && (
                    <div>
                        <h3 className="text-xs font-semibold text-gray-400 mb-2">Workspace Members:</h3>
                        {filteredBoardMembers.map(member => (
                            <div
                                key={member.id}
                                className="flex items-center p-2 hover:bg-gray-800 rounded-md mb-2 cursor-pointer"
                                onClick={() => addMemberToCard(member)}
                            >
                                <img src={member.avatar} alt={member.name} className="w-8 h-8 rounded-full mr-3" />
                                <span className="text-sm font-medium text-gray-300">{member.name}</span>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
}

export default MembersModal;
