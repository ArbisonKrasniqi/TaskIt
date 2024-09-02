import React, { useState, useContext, useEffect } from 'react';
import { postData } from '../../../../Services/FetchService';

const BackgroundCreationModal = ({ open, onClose, onBackgroundCreated }) => {

    const [creatorId, setCreatorId] = useState('c25595c3-b64f-4420-a9f0-96025f90908c');
    const [backgroundTitle, setBackgroundTitle] = useState('');
    const [imageFile, setImageFile] = useState(null);
    const [errorMessage, setErrorMessage] = useState('');

    

    const handleImageUpload = (e) => {
        const file = e.target.files[0];
        if (file) {
            setImageFile(file);  // Directly storing the file object
        }
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onBackgroundCreated({ title: backgroundTitle, imageFile });
        setBackgroundTitle('');
        setImageFile(null);
        onClose();
    };

    const handleBackgroundCreate = async () => {
    if (backgroundTitle.length < 2 || backgroundTitle.length > 280) {
        setErrorMessage('Background title must be between 2 and 280 characters!');
        return;
    }

    const formData = new FormData();
    formData.append('title', backgroundTitle);
    formData.append('imageFile', imageFile);

    console.log("Creating background with data: ", formData);

    try {
        const response = await postData('http://localhost:5157/backend/background/CreateBackground', formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            }
        });

        console.log("Background created successfully");
        onBackgroundCreated(response.data);
        onClose();
    } catch (error) {
        console.error("Error response data:", error.response.data);
    }
};


    if (!open) return null;



    return (
        <div className="absolute inset-0 bg-gray-600 bg-opacity-50 flex justify-center items-center z-50">
        <div className="bg-white p-6 rounded-lg shadow-lg w-[400px]">
            <h2 className="text-lg font-semibold mb-4">Create Background</h2>
            <form onSubmit={handleSubmit}>
            <div className="mb-4">
                <label htmlFor="backgroundTitle" className="block text-sm font-medium text-gray-700">
                Background Title
                </label>
                <input
                type="text"
                id="backgroundTitle"
                value={backgroundTitle}
                onChange={(e) => setBackgroundTitle(e.target.value)}
                className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm p-2 focus:ring-blue-500 focus:border-blue-500"
                placeholder="Enter background title"
                required
                />
            </div>
            <div className="mb-4">
                <label htmlFor="backgroundImage" className="block text-sm font-medium text-gray-700">
                Upload Image
                </label>
                <input
                type="file"
                id="backgroundImage"
                accept="image/*"
                onChange={handleImageUpload}
                className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm p-2 focus:ring-blue-500 focus:border-blue-500"
                required
                />
            </div>
            <div className="flex justify-end">
                <button
                type="button"
                onClick={onClose}
                className="bg-gray-300 hover:bg-gray-400 text-gray-800 font-semibold py-2 px-4 rounded mr-2"
                >
                Cancel
                </button>
                <button
                type="submit"
                className="bg-blue-500 hover:bg-blue-600 text-white font-semibold py-2 px-4 rounded"
                onClick={handleBackgroundCreate}
                >
                Create
                </button>
            </div>
            </form>
        </div>
        </div>
    );
};

export default BackgroundCreationModal;
