import React, { useContext, createContext} from 'react';
import UpdateListButton from './Buttons/UpdateListButton';
import CustomButton from '../Buttons/CustomButton';
import { ListsContext } from './ListsList';
import { deleteData } from '../../../Services/FetchService';

export const UpdateContext = createContext();

const ListsTable = () => {

    const listsContext = useContext(ListsContext);

    const HandleListDelete = (id) => {
        async function deleteList() {
            try {
                const data = {
                    listId: id
                };
                const response = await deleteData('/backend/list/DeleteList', data);
                console.log(response);
                const updatedLists = listsContext.lists.filter(list => list.listId !== id);
                listsContext.setLists(updatedLists);
            } catch (error) {
                listsContext.setErrorMessage(error.message + id);
                listsContext.setShowListsErrorModal(true);
                listsContext.getLists();
            }
        }
        deleteList();
    }
    return (
      <div className='overflow-x-auto'>
        <table className="w-full text-sm text-left rtl:text-right text-gray-500 dar:text-gray-400">
        <thead className="text-sx text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
            <tr>
                <th className="px-6 py-3" >ID</th>
                <th className="px-6 py-3" >Title</th>
                <th className="px-6 py-3" >Board Id</th>
                <th className="px-6 py-3" >Date Created</th>    
                <th className="px-6 py-3" >Actions</th>       
            </tr>     
        </thead>

        <tbody>
            {listsContext.lists ? (listsContext.lists.map((list, index) => (
                <tr key={index} className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                    <td className="px-6 py-4">{list.listId}</td>
                    <td className="px-6 py-4">{list.title}</td>
                    <td className="px-6 py-4">{list.boardId}</td>
                    <td className="px-6 py-4">{list.dateCreated}</td>
                    <td className="px-6 py-4"> <UpdateContext.Provider value={list}> <UpdateListButton/>  </UpdateContext.Provider> <CustomButton color="red" text="Delete" onClick={()=>{ HandleListDelete(list.listId)}}/> </td>
                </tr>
            ))): (
                <tr className="bg-white border-b dark:bg-gray-800 dark:border-gray-700">
                    <td className="px-6 py-4"></td>
                    <td className="px-6 py-4"></td>
                    <td className="px-6 py-4"></td>
                    <td className="px-6 py-4"></td>
                </tr>
            )}
        </tbody>


        </table>
      </div>  
    );
}

export default ListsTable