import React, {useState} from 'react';
import InputField from './InputField.jsx';
import Button from './Button.jsx';
import ErrorMessage from './ErrorMessage.jsx';
import ModalWelcome from '../Modal/modalWelcome.jsx';
import { postData } from '../../Services/FetchService.jsx';
import { StoreTokens } from '../../Services/TokenService.jsx';
import { useNavigate } from 'react-router-dom';

const SignUpForm = () =>{
    
    const navigate = useNavigate();
    const [showModalWelcome, setShowModalWelcome]= useState(false);

    //Qiky modal perdoret nese ka signup por ska mujt me sign in.
    const [showModalSuccess, setShowModalSuccess] = useState(false);
    // const [isSignUpSuccessful, setIsSignUpSuccessful] = useState(false);

    const [formData, setFormData] = useState({
        name: '',
        lastName: '',
        email: '',
        password: '',
        confirmPassword: '',
    });

    const[error, setError] = useState('');

    const handleInputChange = (e)=> {
        const {name, value} = e.target;
        setFormData({
            ...formData,
            [name]: value,
        });
    };

    const handleCloseModal = () => {
        setShowModalWelcome(false);
    };

    const handleSubmit = async (e) =>{
        e.preventDefault();
    
        var nameRegex = /^[a-zA-Z\s]+$/;
        if (!nameRegex.test(formData.name.trim())) {
            setError('Please enter a valid name.');
            return false;
        }
    
        var lastNameRegex = /^[a-zA-Z\s]+$/;
        if (!lastNameRegex.test(formData.lastName.trim())) {
            setError('Please enter a valid surname.');
            return false;
        }

        var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]{2,4}$/;
        if(!emailRegex.test(formData.email)){
            setError('Please enter a valid email address.');
            return false;
        }

        var passwordRegex = /^(?=.*[A-Z])(?=.*\d)(?=.*\W).{8,}$/;

        if(!passwordRegex.test(formData.password)){
            let errorMessage = '';
            if (formData.password.length < 8) {
                errorMessage = 'Password must be at least 8 characters long.';
            } else if (!/(?=.*[A-Z])/.test(formData.password)) {
                errorMessage = 'Password must contain at least one uppercase letter.';
            } else if (!/(?=.*\d)/.test(formData.password)) {
                errorMessage = 'Password must contain at least one number.';
            } else if (!/(?=.*\W)/.test(formData.password)) {
                errorMessage = 'Password must contain at least one special character.';
            }
            setError(errorMessage);
            return false;
        }
        

        if(formData.password !== formData.confirmPassword){
            setError('Passwords do not match.');
            return false;
        }
        // If all validations pass, clear the error and submit the form
        setError('');
        // Submit the form

        const registerData = {
            firstName: formData.name,
            lastName: formData.lastName,
            email: formData.email,
            password: formData.password
        };

        const loginData = {
            email: formData.email,
            password: formData.password
        };

        try {
                await postData('/backend/user/register', registerData);
                const loginResponse = await postData('/backend/user/login', loginData);
                StoreTokens(loginResponse.data.accessToken, loginResponse.data.refreshToken);
                setShowModalWelcome(true);
            } catch (error) {
            if (error.response.data[1].description) {
                setError(error.response.data[1].description);
            } else {
                setError("There has been an internal server error! Try again later");
            }            
        }
    };



    return(

            <div className='container mx-auto mt-20'>
                <div className='flex flex-col-reverse lg:flex-row w-8/12 bg-white roundes=x; mx-auto shadow-lg overflow-hidden inset-x-20 '>
                    <div className='w-full lg:w-5/12 flex flex-col items-center justify-center p-12 bg-no-repeat bg-cover bg-center' style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
                    <h1 className='text-white text-3xl mb-3 font-sans font-bold'>Already a member?</h1>
                        <p className='text-white mt-5 font-sans'>Welcome back! Log in and get back to tasking</p>
                        <button onClick={()=> {navigate('/login')}} className='bg-white text-gray-700 px-4 py-2 mt-20 border border-solid border-gray-700 rounded-md w-[50%]  hover:border hover:border-solid hover:border-black hover:text-black font-bold'>Log in</button>
                    
                    </div>
                    <div className='w-full lg:w-7/12 py-16 px-12'>
                            <h2 className='text-3xl mb-10 text-center font-sans font-bold'>Create Account</h2>
                    <form onSubmit={handleSubmit}>
                        <div className='grid grid-cols-2 gap-5'>
                                <InputField 
                                    type="text"
                                    name="name"
                                    placeholder="First Name"
                                    value={formData.name}
                                    onChange={handleInputChange}
                                />

                                <InputField 
                                     type="text"
                                     name="lastName"
                                     placeholder="Last Name"
                                     value={formData.lastName}
                                     onChange={handleInputChange}
                                />
                        </div>
                        <div className='mt-5'>
                                 <InputField  
                                    type="email"
                                    name="email"
                                    placeholder="Email"
                                    value={formData.email}
                                    onChange={handleInputChange}
                                />
                        </div>
                        <div className='mt-5'>
                                <InputField 
                                    type="password"
                                    name="password"
                                    placeholder="Password"
                                    value={formData.password}
                                    onChange={handleInputChange}
                                />
                        </div>
                        <div className='mt-5'>
                                <InputField 
                                    type="password"
                                    name="confirmPassword"
                                    placeholder="Confirm Password"
                                    value={formData.confirmPassword}
                                    onChange={handleInputChange}
                                />
                        </div>
                        {error && <ErrorMessage content={error}/>}
                        <div className='mt-5 flex justify-center'>
                       
                        <Button type="submit" name="Sign Up" ></Button>
                        
                        </div>
                    </form>
                    {showModalWelcome &&(
                        <ModalWelcome 
                        signedUpUserName={formData.name} 
                        redirectTo="/main/workspaces" //the current path is for demo
                        />
                    )}
                    </div>
                </div>

            </div>


    );
}
export default SignUpForm


