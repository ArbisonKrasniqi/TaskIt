import React, { useContext } from 'react'
import { BackgroundContext } from "./BackgroundsList";
import CustomButton from "../Buttons/CustomButton";
import { deleteData } from "../../../Services/FetchService";

const BackgroundsTable = () => {
    const backgroundsContext = useContext(BackgroundContext);

    const handleBackgroundDelete = (id) => {
        async function deleteBackground() {
            try {
                const data = { backgroundId: id};
                const response = await deleteData('http://localhost:5157/backend/background/DeleteBackgroundByID', data);
                console.log(response);

                const updateBackgrounds = backgroundsContext.backgrounds.filter(background => background.backgroundId !== id);
                backgroundsContext.setBackgrounds(updateBackgrounds);
            } catch (error) {
                backgroundsContext.setErrorMessage(error.message + id);
                backgroundsContext.setShowBackgroundsErrorModal(true);
                backgroundsContext.getBackgrounds();
            }
        }
        deleteBackground();
    };

    const handleBackgroundEdit = (background) => {
        backgroundsContext.setBackgroundToUpdate(background);
        backgroundsContext.setShowUpdateInfoModal(true);

    };

    return (
        <div className='overflow-x-auto'>
            <table className='w-full text-sm text-left rtl:text-right text-gray-500 dark:text-gray-400'>
                <thead className='text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400'>
                    <tr>
                        <th className='px-6 py-3'>Background ID</th>
                        <th className='px-6 py-3'>Creator ID</th>
                        <th className='px-6 py-3'>Title</th>
                        <th className='px-6 py-3'>Image</th>
                        <th className='px-6 py-3'>Date Created</th>
                        <th className='px-6 py-3'>Is Active</th>
                        <th className='px-6 py-3'>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {backgroundsContext.backgrounds ? (backgroundsContext.backgrounds.map((background, index) => (
                        <tr key={index} className='bg-white border-b dark:bg-gray-800 dark:border-gray-700'>
                            <td className='px-6 py-4'>{background.backgroundId}</td>
                            <td className='px-6 py-4'>{background.creatorId}</td>
                            <td className='px-6 py-4'>{background.title}</td>
                            <td className='px-6 py-4 w-[200px] h-auto'><img 
                                src={`data:image/jpeg;base64,${background.imageDataBase64}`} 
                                alt="Background" 
                            /></td>
                            <td className='px-6 py-4'>{background.dateCreated}</td>
                            <td className='px-6 py-4'>{(background.isActive === true) ? ("true") : ("false")}</td>

                            <td className='px-6 py-4'>
                                 <CustomButton color="orange" type='button' text="Edit" onClick={() => handleBackgroundEdit(background)} />
                                 <CustomButton color="red" type='button' text="Delete" onClick={() => handleBackgroundDelete(background.backgroundId)} />
                               
                            </td>
                        </tr>
                    ))) : (
                        <tr className='bg-white border-b dark:bg-gray-800 dark:border-gray-700'>
                            <td className='px-6 py-4'></td>
                            <td className='px-6 py-4'></td>
                            <td className='px-6 py-4'></td>
                            <td className='px-6 py-4'></td>
                            <td className='px-6 py-4'></td>
                            <td className='px-6 py-4'></td>
                            <td className='px-6 py-4'></td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
}

export default BackgroundsTable;