import React, { useState , useContext } from 'react';
import { putData } from '../../../../Services/FetchService';
import { UpdateContext } from '../WorkspacesTable';
import { WorkspacesContext } from '../WorkspacesList';
import CustomButton from '../../Buttons/CustomButton';
import { DashboardContext } from '../../../../Pages/dashboard';
const UpdateWorkspaceModal= (props) => {

    const updateContext = useContext(UpdateContext);
    const workspacesContext = useContext(WorkspacesContext);
    const dashboardContext = useContext(DashboardContext);

    //perdorim useState qe me  mujt me i ndryshu te dhenat
    const [title, setTitle] = useState(updateContext.title);
    const [description, setDescription] = useState(updateContext.description);
    const [ownerId, setOwnerId] = useState(updateContext.ownerId);

    const handleSubmit = async (e) => {

        e.preventDefault();

        try{
            //e krijojme nje objekt qe i ka te dhenat qe i perdorim ne API per me update
            const data = {
                workspaceId: updateContext.workspaceId, //id nuk mundet me ndryshu e marrim ashtu siq eshte

                //ndryshojme te dhenat tjera
                title: title,
                description: description,
                ownerId: ownerId
            };

            const response = await putData('/backend/workspace/UpdateWorkspace', data);
            console.log(response.data);
            const updatedWorkspaces = workspacesContext.workspaces.map(workspace => {
                if(workspace.workspaceId === updateContext.workspaceId){
                    return {
                        ...workspace,
                        //atributeve tjera nuk ja ndryshon vleren
                        title: title,
                        description: description,
                        ownerId: ownerId
                    };
                }else{
                    return workspace; //per workspaces tjere mos bej ndryshime
                }
            });

            workspacesContext.setWorkspaces(updatedWorkspaces); //ndrysho listen e workspaces me listen e re te updateuar


            props.setShowUpdateInfoModal(false);

        }catch(error){
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
            workspacesContext.getWorkspaces();

            props.setShowUpdateInfoModal(false);
        }
    }

        return(   <div className="fixed top-0 left-0 w-full h-full flex items-center justify-center z-50 bg-gray-900 bg-opacity-50">
        <form onSubmit={handleSubmit} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 text-gray-400 p-8 rounded-lg shadow-md w-1/3 h-auto">
            <div className="mb-6">
                <label htmlFor="default-input" className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Title</label>
                <input value={title}
                       onChange={(e) => setTitle(e.target.value)}
                       type="text"
                       id="title"
                       className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"></input>
            </div>
            <div className="grid md:grid-cols-2 md:gap-6">
                <div className="mb-6">
                    <label htmlFor="default-input" className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Description</label>
                    <input value={description}
                           onChange={(e) => setDescription(e.target.value)}
                           type="text"
                           id="description"
                           className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"></input>
                </div>
                <div className="mb-6">
                    <label htmlFor="default-input" className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Owner Id</label>
                    <input value={ownerId}
                           onChange={(e) => setOwnerId(e.target.value)}
                           type="text"
                           id="ownerId"
                           className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"></input>
                </div>
            </div>
            <div className="flex justify-around">
                {/*Nese shtypet butoni close, atehere mbyll modal duke vendosur vleren false*/}
                <CustomButton onClick={() => props.setShowUpdateInfoModal(false)} type="button" text="Close" color="longRed"/>
                <CustomButton type="submit" text="Update" color="longGreen"/>
            </div>
        </form>
    </div>
);

}
export default UpdateWorkspaceModal;