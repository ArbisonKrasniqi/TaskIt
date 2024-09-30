import React,{ useState, useContext, useEffect} from 'react'
import { TaskModalsContext } from './TaskModal'
import { MdOutlineEdit } from "react-icons/md";
import { useParams } from 'react-router-dom';
import { deleteData, getDataWithId, postData } from '../../Services/FetchService';
import { BoardContext } from '../BoardContent/Board';


function LabelsModal() {
    const boardContext = useContext(BoardContext);
    const { toggleLabelsModal, toggleEditLabelModal, assignedLabels, setAssignedLabels, fetchAssignedLabels, getTaskById } = useContext(TaskModalsContext);
    const [searchLabel, setSearchLabel] = useState("");
    const {boardId,taskId} = useParams();
    const [labels, setLabels] = useState([]);

    const [secondAssignedLabels, setSecondAssignedLabels] = useState(assignedLabels);

    useEffect(() => {
        const getLabels = async () => {
            try {
                if (boardId) {
                    const labelsResponse = await getDataWithId('http://localhost:5157/backend/label/GetLabelsByBoardId?boardId',boardId);
                    setLabels(labelsResponse.data);
                }
            } catch (error) {
              if (error.response) {
                console.error(error.response.data);
              }
              console.error("Internal server error");
                
            }
        };
        getLabels();
    },[assignedLabels]);

    const getAssignedLabels = async () => {
      try {
          const assignedLabelResponse = await getDataWithId('http://localhost:5157/backend/label/GetLabelsByTaskId?taskId',taskId);
          setSecondAssignedLabels(assignedLabelResponse.data.map(label => label.labelId));
      } catch (error) {
          console.error("Error fetching assigned labels");
          
      }
    }
    useEffect(() => {
        if (taskId) {
            getAssignedLabels();
        }
    },[taskId])
    
    const filterLabels = labels.filter( label =>
        label.name.toLowerCase().includes(searchLabel.toLowerCase())
    );

    const handleAssignLabelCheckbox = async (label) => {
        if (!label.name) {
            console.log("The label should be named before using!");
            
            return;
        }
        const isChecked = secondAssignedLabels.includes(label.labelId);

        if (isChecked) {
            try {
                const labelData = {
                    labelId: label.labelId,
                    taskId: taskId
                }
                await deleteData('http://localhost:5157/backend/taskLabel/RemoveLabelFromTask',labelData);
                //fetchAssignedLabels();
                getTaskById();
                setSecondAssignedLabels(prev => prev.filter(id => id !== label.labelId));
                boardContext.getTasks();
                
            } catch (error) {
                console.error("Error unassigning label");
                
            }
        } else {
            try {
                const labelData = {
                    labelId: label.labelId,
                    taskId: taskId
                }
                await postData('http://localhost:5157/backend/taskLabel/AssignLabelToTask',labelData);
                //fetchAssignedLabels();
                getTaskById();
                setSecondAssignedLabels(prev => [...prev, label.labelId]);
                boardContext.getTasks();
            } catch (error) {
                console.error("Error assigning label");
                
            }
        }
    }

    return (
        <div className="fixed inset-0 flex items-center justify-center z-50 bg-black bg-opacity-50">
          <div className="bg-gray-900 w-[300px] max-w-md p-5 rounded-md shadow-lg overflow-y-auto" style={{ maxHeight: '80vh' }}>
            <div className="flex justify-between items-center mb-4">
              <h2 className="text-sm font-semibold text-gray-400">Labels</h2>
              <button
                onClick={toggleLabelsModal}
                className="text-gray-500 hover:bg-gray-800 w-6 h-6 rounded-full flex justify-center items-center"
              >
                X
              </button>
            </div>
    
            <input
              type="search"
              value={searchLabel}
              onChange={(e) => setSearchLabel(e.target.value)}
              placeholder="Search labels"
              className="w-full p-3 mb-4 bg-gray-900 border border-gray-700 rounded-sm text-white"
            />
    
            {filterLabels.length > 0 && (
              <div className="mb-4">
                <h3 className="text-xs font-semibold text-gray-400 mb-">Labels:</h3>
                {filterLabels.map(labels => (
                  <div key={labels.labelId} className="flex items-center py-2 h-[50px] rounded-md mb-1">
                    <input 
                      type="checkbox"
                      className='w-6 h-6'
                      checked={secondAssignedLabels.includes(labels.labelId)}
                      onChange={() => handleAssignLabelCheckbox(labels)}
                    />
                    <span 
                      className="text-sm font-medium rounded-sm text-white w-full h-full flex items-center pl-2 mx-1"
                      style={{ backgroundColor: labels.color }}
                      title={`Color: ${labels.color}, Title: "${labels.name.length === 0 ? '' : labels.name}"`}
                    >
                      {labels.name.length === 0 ? '' : labels.name}
                    </span>
                    <button
                      className="ml-auto text-xl text-gray-500 rounded-xs w-8 h-8 flex justify-center items-center hover:bg-gray-800"
                      onClick={() => toggleEditLabelModal(labels)}
                    >
                      <MdOutlineEdit/>
                    </button>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>
      );
}

export default LabelsModal;