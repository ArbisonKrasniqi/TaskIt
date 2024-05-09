import React, {useState} from 'react';
import InputField from '../Components/Sign-up/InputField';
import Button from '../Components/Sign-up/Button';
import ErrorMessage from '../Components/Sign-up/ErrorMessage';

const SignUpPage = () =>{

    const [formData, setFormData] = useState({
        name: '',
        surname: '',
        email: '',
        password: '',
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

            <form onSubmit={handleSubmit}>
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
                <InputField 
                    type="email"
                    name="email"
                    placeholder="Email"
                    value={formData.email}
                    onChange={handleInputChange}
                />
                <InputField 
                    type="password"
                    name="password"
                    placeholder="Password"
                    value={formData.password}
                    onChange={handleInputChange}
                />
           {error && <ErrorMessage content={error}/>}
           <Button type="submit" name="Register"/>

            </form>




    );
}
export default SignUpPage