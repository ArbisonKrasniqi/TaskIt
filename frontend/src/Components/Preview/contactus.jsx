import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './contactus.css'
import Swal from 'sweetalert2'
import { IconBase } from 'react-icons';

const ContactUs = () => {
    const [menuOpen, setMenuOpen] = useState(false);

    const toggleMenu = () => {
        setMenuOpen(!menuOpen);
    };

    const navigate = useNavigate();
    const onSubmit = async (event) => {
        event.preventDefault();
        const formData = new FormData(event.target);
    
        formData.append("access_key", "6b29339c-a880-4f94-af01-36637075a827");
    
        const object = Object.fromEntries(formData);
        const json = JSON.stringify(object);
    
        const res = await fetch("https://api.web3forms.com/submit", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Accept: "application/json"
          },
          body: json
        }).then((res) => res.json());
    
        if (res.success) {
            Swal.fire({
                title: "Sent!",
                text: "Message has been sent successfully!",
                icon: "success"
            });
          
        }
      };


    

    return(
        <div>
            <div className="flex items-center justify-between mx-auto px-4 py-2.5">
                <button  onClick={() => navigate(`/Preview`)} style={{ textAlign: 'left', padding: 30, color: 'dark-blue', fontSize: '40px', fontWeight: 'bold' }}>TaskIt</button>
                {/* Desktop Menu */}
                <div className="hidden md:flex md:space-x-8">
                    <button  onClick={() => navigate(`/AboutUs`)} className="py-2 px-3  hover:bg-black-100 rounded md:hover:bg-transparent md:border-0 dark:text-gray-400 md:dark:hover:text-black dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent ">About Us</button>
                    <button  onClick={() => navigate(`/ContactUs`)} className="py-2 px-3  hover:bg-black-100 rounded md:hover:bg-transparent md:border-0 dark:text-gray-400 md:dark:hover:text-black dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent ">Contact Us</button>
                </div>

   

                {/* Desktop Login Button */}
                <div className="hidden md:flex">
                    <button className="flex items-center justify-center px-5 py-3 rounded-md text-base font-medium text-center border border-transparent shadow-md" style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
                        <span className="text-white">Login</span>
                    </button>
                </div>

                {/* Mobile Menu Button */}
                <div className="md:hidden">
                    <button onClick={toggleMenu} className="text-black">
                        <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/1000/svg">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M4 6h16M4 12h16M4 18h16"></path>
                        </svg>
                    </button>
                </div>
            </div>
            {/* Mobile Dropdown Menu */}
            {menuOpen && (
                <div className="md:hidden text-white" style={{ backgroundImage: 'linear-gradient(150deg, #2E3440, #414B5C)'}} >
                    <ul className="flex flex-col items-start p-4 space-y-4 rounded-lg">
                        <li><button  onClick={() => navigate(`/AboutUs`)} className="text-gray-400">About Us</button></li>
                        <li><button onClick={() => navigate('/ContactUs')} className="text-gray-400">Contact Us</button></li>
                        <li>
                            <button className="w-full py-3 px-5 text-center text-white rounded-md" style={{ backgroundImage: 'linear-gradient(115deg,  #1a202c, #2d3748)'}}>
                                Login
                            </button>
                        </li>
                    </ul>
                </div>
            )}

            <section className="contact">
                <form onSubmit={onSubmit}>
                    <h2>Contact Us</h2>
                    <div className='input-box'>
                        <label>Full Name</label>
                        <input type="text" className="field" placeholder='Enter your full name' name='fullname' required/>
                    </div>
                    <div className='input-box'>
                        <label>Email</label>
                        <input type="email" className="field" placeholder='Enter your email' name='email' required/>
                    </div>
                    <div className='input-box'>
                        <label>Your Message</label>
                        <textarea name="message" className="field mess" placeholder='Enter your message' required></textarea>
                    </div>
                    <button type='submit'>Send Message</button>
                
                </form>
            </section>
        </div>

        
    );
}

export default ContactUs;