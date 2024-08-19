import React, { useState, useContext, useEffect } from 'react';
import { putData } from '../../../../Services/FetchService';
import { BackgroundContext } from '../BackgroundsList';
import CustomButton from '../../Buttons/CustomButton';

const UpdateBackgroundModal = (props) => {
    const backgroundsContext = useContext(BackgroundContext);
    const backgroundToUpdate = backgroundsContext.backgroundToUpdate;

    const [title, setTitle] = useState(backgroundToUpdate.title);
    const [imageUrl, setImageUrl] = useState(backgroundToUpdate.imageUrl);
    const [isActive, setIsActive] = useState(backgroundToUpdate.isActive);
    const [creatorId, setCreatorId] = useState(backgroundToUpdate.creatorId);

    useEffect(() => {
        setTitle(backgroundToUpdate.title);
        setImageUrl(backgroundToUpdate.imageUrl);
        setIsActive(backgroundToUpdate.isActive);
    }, [backgroundToUpdate]);

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const data = {
                backgroundId: backgroundToUpdate.backgroundId,
                creatorId: creatorId,
                title: title,
                imageUrl: imageUrl,
                isActive: isActive,
                dateCreated: backgroundToUpdate.dateCreated
            };

            console.log('Sending data: ',data);

            const response = await putData('http://localhost:5157/backend/background/UpdateBackground', data);
            console.log('Response: ',response);

            const updatedBackgrounds = backgroundsContext.backgrounds.map(background => {
                if (background.backgroundId === data.backgroundId) {
                    return{
                        ...background,
                        title: title,
                        imageUrl: imageUrl,
                        isActive: isActive,
                        creatorId: creatorId
                    };
                }
                return background;
            });

            backgroundsContext.setBackground(updatedBackgrounds);
            props.setShowUpdateInfoModal(false);
        } catch (error) {
            console.log('Error updating background: ',error);
            backgroundsContext.setErrorMessage(error.message);
            backgroundsContext.setShowBackgroundsErrorModal(true);
            backgroundsContext.getBackgrounds();
            props.setShowUpdateInfoModal(false);
        }
    };

    return (
        <div className='fixed top-0 left-0 w-full h-full flex items-center justify-center z-50 bg-gray-900 bg-opacity-50'>
            <form onSubmit={handleSubmit} className='bg-white border-b dark:bg-gray-800 dark:border-gray-700 text-gray-400 p-8 rounded-lg shadow-md w-1/3 h-auto'>
                <div className='grid md:grid-cols-2 md:gap-6'>
                    <div className='mb-6'>
                        <label htmlFor="title" className='block mb-2 text-sm font-medium text-gray-900 dark:text-white'>Title</label>
                        <input value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            type='text'
                            id='title'
                            className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500' />
                    </div>
                    <div className="mb-6">
                        <label htmlFor="imageUrl" className='block mb-2 text-sm font-medium text-gray-900 dark:text-white'>Image Url</label>
                        <input value={imageUrl}
                               onChange={(e) => setImageUrl(e.target.value)}
                               type="text"
                               id='imageUrl'
                               className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500' />
                    </div>
                    <div className="mb-6">
                        <label htmlFor="isActive" className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Is Active</label>
                        <input value={isActive}
                               onChange={(e) => setIsActive(e.target.value)}
                               type="boolean"
                               id="isActive"
                               className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500' />
                    </div><div className="mb-6">
                        <label htmlFor="creatorId" className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Creator Id</label>
                        <input value={creatorId}
                               onChange={(e) => setCreatorId(e.target.value)}
                               readOnly
                               type="text"
                               id="creatorId"
                               className='bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500' />
                    </div>
                </div>

                <div className='flex justify-around'>
                    <CustomButton onClick={() => props.setShowUpdateInfoModal(false)} type="button" text="Close" color="longRed" />
                    <CustomButton type="submit" text="Update" color="longGreen" />
                </div>
            </form>
        </div>
    );
}

export default UpdateBackgroundModal;