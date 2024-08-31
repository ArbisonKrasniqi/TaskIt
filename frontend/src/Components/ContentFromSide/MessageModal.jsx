import { useEffect } from "react";

const MessageModal = ({message, isOpen, duration, onClose})=>{
    useEffect(()=>{
        if(isOpen){
            const timer = setTimeout(onClose, duration);

            return()=>clearTimeout(timer);
        }
    },[isOpen, duration, onClose]);

    if(!isOpen) return null;

    return(

        <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
            <div className="bg-white p-5 rounded-lg shadow-lg">
                <p>{message}</p>
            </div>
        </div>



    );
}
export default MessageModal