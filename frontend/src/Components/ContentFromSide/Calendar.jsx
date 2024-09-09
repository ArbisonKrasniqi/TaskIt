import React from 'react';
import {useContext} from 'react';
import { WorkspaceContext } from '../Side/WorkspaceContext';

const Calendar = () => {
    // Get today's date
    const today = new Date();
    const {tasks} = useContext(WorkspaceContext);
    // Function to format the date (e.g., "Mon, Aug 18")
    const formatDate = (date) => {
        const options = { weekday: 'short', month: 'short', day: 'numeric' };
        return date.toLocaleDateString('en-US', options);
    };

    // Function to format the month and year (e.g., "August 2024")
    const formatMonthYear = (date) => {
        const options = { month: 'long', year: 'numeric' };
        return date.toLocaleDateString('en-US', options);
    };

    // Generate an array of the next 7 days including today
    const days = Array.from({ length: 7 }, (_, i) => {
        const date = new Date(today);
        date.setDate(today.getDate() + i);
        return date;
    });
    const toDateKey = (date) => date.toISOString().split('T')[0];
    return (
        <div className="p-4 min-h-screen h-full" style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
            <div className="font-semibold font-sans text-gray-400 flex flex-wrap justify-normal flex-col">
                <h2 className="text-2xl m-5">Calendar</h2>
                <h3 className="text-xl m-5">{formatMonthYear(today)}</h3>
                <hr className="w-full border-gray-400"></hr>
                <div className="grid grid-cols-7 gap-2 mt-10">
                {days.map((date, index) => {
                        const dateKey = toDateKey(date);
                        const tasksForDate = tasks.filter(task => task.dueDate.split('T')[0] === dateKey);

                        return (
                            <div
                                key={index}
                                className={`text-center p-2 rounded border-l-2 hover:cursor-pointer ${
                                    index === 0 ? 'bg-blue-500 text-white border-blue-500' : 'bg-gray-200 border-gray-400 text-gray-500'
                                }`}
                            >
                                <div className="mb-4 turncate">
                                    {formatDate(date)}
                                </div>
                                <div className="text-sm text-left p-2 flex flex-col">
                                    {tasksForDate.length > 0 ? (
                                        tasksForDate.map((task, i) => (
                                            <p key={i}  className="truncate text-black">{task.taskTitle}</p>
                                        ))
                                    ) : (
                                        <p className="truncate">No tasks</p>
                                    )}
                                </div>
                            </div>
                        );
                    })}
                </div>
                </div>
            </div>
     
    );
};

export default Calendar;
