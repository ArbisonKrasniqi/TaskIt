import React, { useState, useContext} from 'react';
import { putData } from '../../../../Services/FetchService';
import { UpdateContext } from '../ListsTable';
import { ListsContext } from '../ListsList';
import CustomButton from '../../Buttons/CustomButton';
import { DashboardContext } from '../../../../Pages/dashboard';

const UpdateListModal = (props) => {
    const updateContext = useContext(UpdateContext);
    const listsContext = useContext(ListsContext);
    const dashboardContext = useContext(DashboardContext);

    const [title, setTitle] = useState(updateContext.title);

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const data = {
                listId: updateContext.listId,
                title: title,
            };

            const response = await putData('/backend/list/UpdateList', data);
            console.log(response.data);
            const updatedLists = listsContext.lists.map(list => {
                if(list.listId === updateContext.listId){
                    return {
                        ... list,
                        title: title,
                    };
                } else {
                    return list;
                }
            });

            listsContext.setLists(updatedLists);
            props.setShowUpdateInfoModal(false);
        } catch (error) {
            dashboardContext.setDashboardErrorMessage(error.message);
            dashboardContext.setShowDashboardErrorModal(true);
            listsContext.getLists();

            props.setShowUpdateInfoModal(false);
        }
    }

    return(
        <div className="fixed top-0 left-0 w-full h-full flex items-center justify-center z-50 bg-gray-900 bg-opacity-50">
            <form onSubmit={handleSubmit} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700 text-gray-400 p-8 rounded-lg shadow-md w-1/3 h-auto">
                <div className="mb-6">
                    <label htmlFor="default-input" className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Title</label>
                    <input value={title}
                        onChange={(e) => setTitle(e.target.value)}
                        type="text"
                        id="title"
                        className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"></input>
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

export default UpdateListModal;