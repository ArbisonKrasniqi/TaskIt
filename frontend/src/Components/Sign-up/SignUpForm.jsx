import React, {useState} from 'react';
import InputField from './InputField.jsx';
import Button from './Button.jsx';
import ErrorMessage from './ErrorMessage.jsx';


const SignUpForm = () =>{

    const [formData, setFormData] = useState({
        name: '',
        surname: '',
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


    const handleSubmit = (e) =>{
        e.preventDefault();
     

        //..
    };



    return(

            <div className='container mx-auto mt-20'>
                <div className='flex flex-col-reverse lg:flex-row w-8/12 bg-white roundes=x; mx-auto shadow-lg overflow-hidden inset-x-20 '>
                    <div className='w-full lg:w-5/12 flex flex-col items-center justify-center p-12 bg-no-repeat bg-cover bg-center' style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
                    <h1 className='text-white text-3xl mb-3 font-sans font-bold'>Already a member?</h1>
                        <p className='text-white mt-5 font-sans'>Welcome back! Log in and get back to tasking</p>
                        <button type='submit' className='bg-white text-gray-700 px-4 py-2 mt-20 border border-solid border-gray-700 rounded-md w-[50%]  hover:border hover:border-solid hover:border-black hover:text-black font-bold'>Log in</button>
                    
                    </div>
                    <div className='w-full lg:w-7/12 py-16 px-12'>
                            <h2 className='text-3xl mb-10 text-center font-sans font-bold'>Create Account</h2>
                    <form onSubmit={handleSubmit}>
                        <div className='grid grid-cols-2 gap-5'>
                                <InputField 
                                    type="text"
                                    name="name"
                                    placeholder="Name"
                                    value={formData.name}
                                    onChange={handleInputChange}
                                />

                                <InputField 
                                     type="text"
                                     name="surname"
                                     placeholder="Surname"
                                     value={formData.surname}
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
                       
                        <Button type="submit" name="Sign Up"></Button>
                        
                        </div>
                    </form>
                    </div>
                </div>

            </div>


    );
}
export default SignUpForm


