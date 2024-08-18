import React from 'react';

const LoadingModal = () => {
    return (
        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-80 z-50">
            <div className="flex flex-col items-center">
                <div className="loader ease-linear rounded-full border-4 border-t-4 border-gray-200 h-12 w-12 mb-4"></div>
                <h2 className="text-white text-xl font-semibold">Loading...</h2>
            </div>
        </div>
    );
};

export default LoadingModal;