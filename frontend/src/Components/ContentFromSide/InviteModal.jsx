import React, { useState, useContext } from 'react';
import { WorkspaceContext } from '../Side/WorkspaceContext';
const InviteModal = ({ isOpen, onClose }) => {
    const [email, setEmail] = useState('');
    const [username, setUsername] = useState('');
    
    const handleInvite = () => {
        // Funksioni për dërgimin e ftesës
        console.log('Inviting:', email || username);
        onClose(); // Mbyll modalin pas dërgimit të ftesës
    };

    return (
        <div
            className={`fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center transition-opacity ${isOpen ? 'opacity-100' : 'opacity-0 pointer-events-none'}`}
            aria-hidden={!isOpen}
        >
            <div className="bg-white p-6 rounded-lg shadow-lg w-96">
                <h2 className="text-lg font-semibold mb-4">Invite User</h2>
                <input
                    type="text"
                    placeholder="Email or Username"
                    value={email || username}
                    onChange={(e) => {
                        const value = e.target.value;
                        if (value.includes('@')) {
                            setEmail(value);
                            setUsername('');
                        } else {
                            setUsername(value);
                            setEmail('');
                        }
                    }}
                    className="w-full p-2 border border-gray-400 rounded mb-4"
                />
                <div className="flex justify-end">
                    <button
                        onClick={onClose}
                        className="bg-gray-300 text-gray-800 px-4 py-2 rounded mr-2"
                    >
                        Cancel
                    </button>
                    <button
                        onClick={handleInvite}
                        className="bg-blue-500 text-white px-4 py-2 rounded"
                    >
                        Send Invite
                    </button>
                </div>
            </div>
        </div>
    );
};

export default InviteModal;
