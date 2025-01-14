import React, { useContext, useState, useEffect } from "react";
import { TaskModalsContext } from "./TaskModal";
import { WorkspaceContext } from "../Side/WorkspaceContext";
import { MainContext } from "../../Pages/MainContext";
import { deleteData, getDataWithId, postData } from "../../Services/FetchService";
import { BoardContext } from "../BoardContent/Board";
import { useParams } from "react-router-dom";

const MembersModal = () => {
    const { toggleMembersModal, assignedMembers, setAssignedMembers } = useContext(TaskModalsContext);
    const { memberDetails, getInitials } = useContext(WorkspaceContext);
    const [searchTerm, setSearchTerm] = useState("");
    const [originalBoardMembers, setOriginalBoardMembers] = useState(memberDetails || []);  // Per me i rujt members mos me i prek direkt
    const [filteredBoardMembers, setFilteredBoardMembers] = useState([]);
    const boardContext = useContext(BoardContext);
    const {workspaceId, boardId, taskId} = useParams();
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
                taskId: taskId,
            };
            var addedMember = await postData('http://localhost:5127/backend/TaskMembers/AddTaskMember', data);
            
            // Shtoje anetarin ne assignedMembers edhe largoje prej filteredBoardMembers
            setAssignedMembers([...assignedMembers, member]);
            boardContext.getTasks();
        } catch (error) {
            console.error("Error adding member to task");
        }
    };

    const removeMemberFromTask = async (member) => {
        try {
            const userId = member.id;
            await deleteData(`http://localhost:5127/backend/TaskMembers/RemoveTaskMember?userId=${userId}&taskId=${taskId}`);
            
            // Largoje prej assignedMembers edhe shto prap ne filteredBoardMembers
            setAssignedMembers(assignedMembers.filter(m => m.id !== member.id));
            boardContext.getTasks();
        } catch (error) {
            console.error("Error removing member from task");
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
    


    return (
        <div className="fixed inset-0 flex items-center justify-center z-50 bg-black bg-opacity-50">
          <div className="bg-gray-900 w-[300px] max-w-md p-5 rounded-md shadow-lg">
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
    
            {filteredTaskMembers.length > 0 && (
              <div className="mb-4">
                <h3 className="text-xs font-semibold text-gray-400 mb-2">Task Members:</h3>
                {filteredTaskMembers.map((member) => (
                  <div key={member.id} className="flex items-center p-2 hover:bg-gray-800 rounded-md mb-2">
                    <div className="flex-shrink-0 w-10 h-10 rounded-full flex items-center justify-center text-sm text-white bg-gradient-to-r from-orange-400 to-orange-600">
                      {getInitials(member.firstName, member.lastName)}
                    </div>
                    <span className="text-sm ml-3 font-medium text-gray-300">{member.firstName} {member.lastName}</span>
                    <button
                      onClick={() => removeMemberFromTask(member)}
                      className="ml-auto text-xs text-gray-500 hover:text-red-500"
                    >
                      X
                    </button>
                  </div>
                ))}
              </div>
            )}
    
            {boardMembersToShow.length > 0 && (
              <div>
                <h3 className="text-xs font-semibold text-gray-400 mb-2">Workspace Members:</h3>
                {boardMembersToShow.map((member) => (
                  <div
                    key={member.id}
                    className="flex items-center p-2 hover:bg-gray-800 rounded-md mb-2 cursor-pointer"
                    onClick={() => addMemberToTask(member)}
                  >
                    <div className="flex-shrink-0 w-10 h-10 rounded-full flex items-center justify-center text-sm text-white bg-gradient-to-r from-orange-400 to-orange-600">
                      {getInitials(member.firstName, member.lastName)}
                    </div>
                    <span className="text-sm ml-3 font-medium text-gray-300">{member.firstName} {member.lastName}</span>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>
      );
}

export default MembersModal;
