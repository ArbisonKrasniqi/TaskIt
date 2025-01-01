import React from 'react';
import{ useNavigate} from 'react-router-dom';

const ModalWelcome = ({redirectTo,signedUpUserName})=>{

    const navigate = useNavigate();

    const handleContinue = () =>{
        navigate(redirectTo);
    }



    return(
        <div className='fixed z-10 inset-0 overflow-y-auto flex items-center justify-center bg-black bg-opacity-50 '>
            <div className='bg-white rounded-lg px-4 py-6'>
                <h2 className='text-center text-2xl mb-4 font-bold'>Welcome, {signedUpUserName}!</h2>
                <p className="text-center mb-4">Your Sign Up was sucessful , continue to our page to start managing your tasks</p>
                <button onClick={handleContinue} className="block mx-auto mt-5 bg-blue-500 hover:bg-blue-600 text-white font-semibold py-2 px-4 rounded focus:outline-none focus:ring focus:ring-blue-300">Continue</button>
            </div>
        </div>


    );

};
export default ModalWelcome;