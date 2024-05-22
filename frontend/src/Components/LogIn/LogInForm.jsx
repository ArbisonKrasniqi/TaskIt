import React,{useState} from 'react';
import InputField from '../Sign-up/InputField.jsx';
import ErrorMessage from '../Sign-up/ErrorMessage.jsx';
import Button from '../Sign-up/Button.jsx';
import { StoreTokens } from '../../Services/TokenService.jsx';
import { postData } from '../../Services/FetchService.jsx';

const LogInForm =  () =>{
    const [formData,setFormData] = useState({
        email:'',
        password:'',
    });

    const[error, setError] =useState('');

    const handleInputChange = (e)=>{
        const {name, value} = e.target;

        setFormData({
            ...formData,
            [name]: value,
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await postData('/backend/user/login', formData);
            StoreTokens(response.data.accessToken, response.data.refreshToken);
        } catch (error) {
            console.error("Login failed: ", error);
        }
    };

    const handleClick = () => {
        window.location.href = '';
      };
    

    return(
        <div className='container mx-auto mt-20 h-[1000px]'>
            <div className='flex flex-col lg:flex-row w-full lg:w-8/12 h-screen lg:h-96 bg-white roundes=x; mx-auto shadow-lg overflow-hidden inset-x-20'>
                <div className='flex lg:flex-row justify-center items-center w-full lg:w-7/12 py-16 px-12 '>
                    <form onSubmit={handleSubmit} >
                    <div>
                    <h2 className='text-3xl mb-10 text-center font-sans font-bold'>Log in to continue</h2>
                    </div>
                        <div className='mt-5'>
                            <InputField
                            type="text"
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
                        {error && <ErrorMessage content={error}/>}

                        <div className='mt-6 w-2/3 mx-auto'>
                            <Button type="submit" name="LogIn"/>
                        </div>

                    </form>


                </div>
             

                <div className='w-full lg:w-5/12 flex flex-col items-center justify-center p-12 bg-no-repeat bg-cover bg-center' style={{backgroundImage: 'linear-gradient(115deg, #7F9CF5, #3B82F6)'}}>
                    <h1 className='text-white text-3xl mb-3 font-sans font-bold'>Dont have an account?</h1>
                    <p className='text-white mt-5 font-sans' onClick={handleClick}>Sign up below to start organizing your projects and collaborating with your team</p>
                    <button type='submit' className='bg-white text-blue px-4 py-2 mt-20 rounded-md w-1/2'>Sign Up</button>

                </div>

            </div>

        </div>

    );
}
export default LogInForm