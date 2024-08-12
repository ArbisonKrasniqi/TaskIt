import React, { useState } from 'react';

const TaskComment = () => {

    return (
        <textarea
      placeholder="Write a comment..."
      className="bg-gray-900 bg-opacity-50 p-2 rounded-md resize-none focus:outline-none focus:ring-2 focus:ring-blue-500 overflow-hidden w-[490px] h-[40px]"
    />
    );
};

export default TaskComment;
