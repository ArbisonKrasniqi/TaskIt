import React, { useContext, useState } from 'react';
import Calendar from 'react-calendar';
import 'react-calendar/dist/Calendar.css'; // Import default styles for the calendar
import { TaskModalsContext } from './TaskModal';
import { putData } from '../../Services/FetchService';

const DateCalendarModal = () => {
    const { closeCalendar, taskData,getTaskById } = useContext(TaskModalsContext);
    const [date, setDate] = useState(new Date());

    const SaveDueDate = async () => {
        try {
            const utcDate = new Date(Date.UTC(
                date.getFullYear(),
                date.getMonth(),
                date.getDate()
            ));

            const data = {
                taskId: taskData.taskId,
                title: taskData.title,
                description: taskData.description,
                dateAdded: taskData.dateAdded,
                dueDate: utcDate,
                listId: taskData.listId,
            };

            const response = await putData('http://localhost:5157/backend/task/UpdateTask', data);
            console.log("UPDATED: ", response.data);
            getTaskById();
            closeCalendar();
        } catch (error) {
            console.error("Error updating the due date of the task: ", error);
        }
    };

    const tileClassName = ({ date, view }) => {
        if (view === 'month' && taskData.dueDate) {
            const dueDate = new Date(taskData.dueDate);
            if (date.getDate() === dueDate.getDate() &&
                date.getMonth() === dueDate.getMonth() &&
                date.getFullYear() === dueDate.getFullYear()) {
                return 'react-calendar__tile--due-date'; // Apply the custom class
            }
        }
        return '';
    };
    

    return (
        <>
            <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50 z-50">
                <div className="relative bg-white p-6 rounded shadow-lg w-96">
                    <button className="absolute top-0 right-0 p-2 text-black" 
                            onClick={closeCalendar}>
                        X
                    </button>
                    <h2 className="text-xl mb-4">Select Date</h2>
                    <Calendar
                        onChange={setDate}
                        value={date}
                        tileClassName={tileClassName}
                    />
                    <div className="mt-4 font-semibold">
                        <p>Selected Date: {date.toDateString()}</p>
                    </div>
                    <button 
                        className='bg-blue-500 hover:bg-blue-400 w-full m-1 text-white py-2 rounded'
                        onClick={SaveDueDate}
                    >Save</button>
                </div>
            </div>
        </>
    );
};

export default DateCalendarModal;
