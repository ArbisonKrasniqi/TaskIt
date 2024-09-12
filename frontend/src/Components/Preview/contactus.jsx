import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
//import './contactus.css'
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
        <div classname="min-h-screen">
            <div className="flex items-center justify-between mx-auto px-4 py-2.5">
                <button onClick={() => navigate(`/Preview`)}  style={{ textAlign: 'left', padding: 30, color: 'dark-blue', fontSize: '40px', fontWeight: 'bold' }}>TaskIt</button>
                {/* Desktop Menu */}
                <div className="hidden md:flex md:space-x-8">
                    <button onClick={() => navigate(`/AboutUs`)} className="py-2 px-3 hover:bg-black-100 rounded dark:text-gray-400 dark:hover:text-black dark:hover:bg-gray-700 dark:hover:text-white">About Us</button>
                    <button onClick={() => navigate(`/ContactUs`)} className="py-2 px-3 hover:bg-black-100 rounded dark:text-gray-400 dark:hover:text-black dark:hover:bg-gray-700 dark:hover:text-white">Contact Us</button>
                </div>

                {/* Desktop Login Button */}
                <div className="hidden md:flex">
                    <button onClick={() => navigate(`/Login`)} className="flex items-center justify-center px-5 py-3 rounded-md text-base font-medium text-center border border-transparent shadow-md bg-gradient-to-r from-gray-800 to-gray-900 text-white">
                        Login
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
                <div className="md:hidden text-white bg-gradient-to-r from-gray-700 to-gray-900">
                    <ul className="flex flex-col items-start p-4 space-y-4 rounded-lg">
                        <li><button onClick={() => navigate(`/AboutUs`)} className="text-gray-400">About Us</button></li>
                        <li><button onClick={() => navigate('/ContactUs')} className="text-gray-400">Contact Us</button></li>
                        <li>
                            <button onClick={() => navigate(`/Login`)} className="w-full py-3 px-5 text-center text-white rounded-md bg-gradient-to-r from-gray-800 to-gray-900">
                                Login
                            </button>
                        </li>
                    </ul>
                </div>
            )}

            <section className="flex justify-center items-center min-h-[70vh]">
                <form onSubmit={onSubmit} className="max-w-[550px] w-full p-6 rounded-lg shadow-lg bg-white">
                    <h2 className="text-3xl text-center mb-6">Contact Us</h2>
                    <div className="mb-6">
                        <label className="block text-gray-700">Full Name</label>
                        <input type="text" className="w-full h-12 bg-transparent border-2 border-gray-300 rounded-lg p-4 mt-2 focus:outline-none focus:border-gray-500" placeholder='Enter your full name' name='fullname' required/>
                    </div>
                    <div className="mb-6">
                        <label className="block text-gray-700">Email</label>
                        <input type="email" className="w-full h-12 bg-transparent border-2 border-gray-300 rounded-lg p-4 mt-2 focus:outline-none focus:border-gray-500" placeholder='Enter your email' name='email' required/>
                    </div>
                    <div className="mb-6">
                        <label className="block text-gray-700">Your Message</label>
                        <textarea name="message" className="w-full h-48 bg-transparent border-2 border-gray-300 rounded-lg p-4 mt-2 focus:outline-none focus:border-gray-500 resize-none" placeholder='Enter your message' required></textarea>
                    </div>
                    <button type='submit' className="w-full h-12 bg-gradient-to-r from-gray-800 to-gray-900 text-white rounded-lg shadow-lg mt-4">
                        Send Message
                    </button>
                </form>
            </section>
        </div>
    );
}

export default ContactUs;
