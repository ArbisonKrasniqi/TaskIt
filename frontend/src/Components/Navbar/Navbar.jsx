    import React, { useState, useEffect,createContext } from 'react';
    import WorkspaceDropdown from '../Dropdowns/WorkspaceDropdown';
    import RecentDropdown from '../Dropdowns/RecentDropDown';
    import StarredDropdown from '../Dropdowns/StarredDropdown';
    import CreateDropdown from '../Dropdowns/CreateDropdown';
    import MoreDropdown from '../Dropdowns/MoreDropdown';
    import PlusDropdown from '../Dropdowns/PlusDropdown';
    import NotificationButton from './Notification/NotificationButton';
    import NavbarProfilePic from '../ProfilePic/NavbarProfilePic';
    import { useNavigate } from 'react-router-dom';

    export const DropdownContext = createContext();

    const Navbar = () => {

        const navigate = useNavigate();

        const [WorkspaceDropdownIsOpen, setWorkspaceDropdownIsOpen] = useState(false);
        const [RecentDropdownIsOpen, setRecentDropdownIsOpen] = useState(false);
        const [StarredDropdownIsOpen, setStarredDropdownIsOpen] = useState(false);
        const [CreateDropdownIsOpen, setCreateDropdownIsOpen] = useState(false);
        const [NotificationDropdownIsOpen, setNotificationDropdownIsOpen] = useState(false);
        const [ProfilePicIsOpen, setProfilePicDropdownIsOpen] = useState(false);
        const [MoreDropdownIsOpen, setMoreDropdownIsOpen] = useState(false);
        const [PlusDropdownIsOpen, setPlusDropdownIsOpen] = useState(false);

        const toggleDropdownWorkspace = () => {
            if (!WorkspaceDropdownIsOpen && width>880) {
                setWorkspaceDropdownIsOpen(true);
                setRecentDropdownIsOpen(false);
                setStarredDropdownIsOpen(false);
                setCreateDropdownIsOpen(false);
                setMoreDropdownIsOpen(false);
                setPlusDropdownIsOpen(false);
                setNotificationDropdownIsOpen(false);
                setProfilePicDropdownIsOpen(false);
            }
            else if (!WorkspaceDropdownIsOpen && width<=880) {
                setWorkspaceDropdownIsOpen(true);
                setRecentDropdownIsOpen(false);
                setStarredDropdownIsOpen(false);
                setPlusDropdownIsOpen(false);
                setNotificationDropdownIsOpen(false);
                setProfilePicDropdownIsOpen(false);
            }
            else{
                setWorkspaceDropdownIsOpen(false);
            }
        }

        const toggleDropdownRecent = () => {
            if (!RecentDropdownIsOpen && width>960) {
                setWorkspaceDropdownIsOpen(false);
                setRecentDropdownIsOpen(true);
                setStarredDropdownIsOpen(false);
                setCreateDropdownIsOpen(false);
                setMoreDropdownIsOpen(false);
                setPlusDropdownIsOpen(false);
                setNotificationDropdownIsOpen(false);
                setProfilePicDropdownIsOpen(false);
            }
            else if (!RecentDropdownIsOpen && width<=960) {
                setWorkspaceDropdownIsOpen(false);
                setRecentDropdownIsOpen(true);
                setStarredDropdownIsOpen(false);
                setPlusDropdownIsOpen(false);
                setNotificationDropdownIsOpen(false);
                setProfilePicDropdownIsOpen(false);
            }
            else{
                setRecentDropdownIsOpen(false);
            }
        }

        const toggleDropdownStarred = () => {
            if (!StarredDropdownIsOpen) {
                setWorkspaceDropdownIsOpen(false);
                setRecentDropdownIsOpen(false);
                setStarredDropdownIsOpen(true);
                setCreateDropdownIsOpen(false);
                setPlusDropdownIsOpen(false);
                setNotificationDropdownIsOpen(false);
                setProfilePicDropdownIsOpen(false);
            }
            else{
                setStarredDropdownIsOpen(false);
            }
        }

        const toggleDropdownCreate = () => {
            if (!CreateDropdownIsOpen) {
                setWorkspaceDropdownIsOpen(false);
                setRecentDropdownIsOpen(false);
                setStarredDropdownIsOpen(false);
                setCreateDropdownIsOpen(true);
                setPlusDropdownIsOpen(true);
                setMoreDropdownIsOpen(false);
                setNotificationDropdownIsOpen(false);
                setProfilePicDropdownIsOpen(false);
            }
            else{
                setCreateDropdownIsOpen(false);
                setPlusDropdownIsOpen(false);
            }
        }

        

        const toggleDropdownNotification = () => {
            if (!NotificationDropdownIsOpen) {
                setWorkspaceDropdownIsOpen(false);
                setRecentDropdownIsOpen(false);
                setStarredDropdownIsOpen(false);
                setCreateDropdownIsOpen(false);
                setMoreDropdownIsOpen(false);
                setPlusDropdownIsOpen(false);
                setNotificationDropdownIsOpen(true);
                setProfilePicDropdownIsOpen(false);
            }
            else{
                setNotificationDropdownIsOpen(false);
            }
        }

        const toggleDropdownProfilePic = () => {
            if (!ProfilePicIsOpen) {
                setWorkspaceDropdownIsOpen(false);
                setRecentDropdownIsOpen(false);
                setStarredDropdownIsOpen(false);
                setCreateDropdownIsOpen(false);
                setMoreDropdownIsOpen(false);
                setPlusDropdownIsOpen(false);
                setNotificationDropdownIsOpen(false);
                setProfilePicDropdownIsOpen(true);
            }
            else{
                setProfilePicDropdownIsOpen(false);
            }
        }

        const toggleDropdownMore = () => {
            if (!MoreDropdownIsOpen) {
                setWorkspaceDropdownIsOpen(false);
                setRecentDropdownIsOpen(false);
                setStarredDropdownIsOpen(false);
                setCreateDropdownIsOpen(false);
                setMoreDropdownIsOpen(true);
                setPlusDropdownIsOpen(false);
                setNotificationDropdownIsOpen(false);
                setProfilePicDropdownIsOpen(false);
            }
            else{
                setMoreDropdownIsOpen(false);
            }
        }

        const toggleDropdownPlus = () => {
            if (!PlusDropdownIsOpen) {
                setWorkspaceDropdownIsOpen(false);
                setRecentDropdownIsOpen(false);
                setStarredDropdownIsOpen(false);
                setMoreDropdownIsOpen(false);
                setCreateDropdownIsOpen(true);
                setPlusDropdownIsOpen(true);
                setNotificationDropdownIsOpen(false);
                setProfilePicDropdownIsOpen(false);
            }
            else{
                setPlusDropdownIsOpen(false);
                setCreateDropdownIsOpen(false);
            }
        }

        const values = {
            WorkspaceDropdownIsOpen,
            RecentDropdownIsOpen,
            StarredDropdownIsOpen,
            CreateDropdownIsOpen,
            MoreDropdownIsOpen,
            PlusDropdownIsOpen,
            NotificationDropdownIsOpen,
            ProfilePicIsOpen,
            setWorkspaceDropdownIsOpen,
            setRecentDropdownIsOpen,
            setStarredDropdownIsOpen,
            setCreateDropdownIsOpen,
            setMoreDropdownIsOpen,
            setPlusDropdownIsOpen,
            setNotificationDropdownIsOpen,
            setProfilePicDropdownIsOpen,
            toggleDropdownWorkspace,
            toggleDropdownRecent,
            toggleDropdownStarred,
            toggleDropdownCreate,
            toggleDropdownMore,
            toggleDropdownPlus,
            toggleDropdownNotification,
            toggleDropdownProfilePic
        }



        const [width,setWidth] = useState(window.innerWidth);

        function handleResize(){
            setWidth(window.innerWidth);
        }

        useEffect(() => {
            window.addEventListener("resize", handleResize);
            
        }, [])

        return (
            <div className='bg-gray-800 w-full h-12 flex justify-between items-center border-b border-b-solid border-b-gray-500'>
                
                <DropdownContext.Provider value={values}>
                    <div className='flex items-center'>
                        <h1 className='text-xl font-bold text-gray-400 rounded hover:bg-gray-700 mx-4 p-2 cursor-pointer'
                            onClick={() => {navigate('/main/workspaces')}}>TaskIt</h1>

                        <div>
                        <WorkspaceDropdown
                            width = {window.innerWidth}
                        />
                        </div>

                        <div>
                        <RecentDropdown
                            width = {window.innerWidth}
                        />
                        </div>

                        <div>
                        <StarredDropdown
                            width = {window.innerWidth}
                        />
                        </div>

                        <div className={`${(width>1200) ? '' : 'hidden'}`}>
                        <CreateDropdown
                        />
                        </div>

                        <MoreDropdown
                        width = {window.innerWidth}
                        />

                        <PlusDropdown
                        width = {window.innerWidth}
                        />

                    </div>

                    <div className='flex items-center mx-2'>

                        <NotificationButton/>

                        <NavbarProfilePic/>
                    </div>
                </DropdownContext.Provider>
            </div>
        );
    };

    export default Navbar;
