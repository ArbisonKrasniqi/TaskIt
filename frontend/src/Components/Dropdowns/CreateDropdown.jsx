import React from 'react';

const CreateDropdown = (props) => {

    return (
        <div className='relative'>
            <button
                onClick={props.toggleDropdown}
                className='bg-blue-300 text-white px-4 py-2 rounded focus:outline-none'>
                Create
            </button>

            {props.isOpen && (
                <div className='absolute right-0 mt-2 w-48 bg-white rounded-lg shadow-lg'>
                    <button className='block w-full text-left px-4 py-2 text-gray-800 hover:bg-gray-200'>
                        TaskIt Workspace
                    </button>
                </div>
            )}
        </div>
    );
}

export default CreateDropdown;
