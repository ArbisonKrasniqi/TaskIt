import React, {useState} from 'react';
import WorkspaceDropdown from '../Dropdowns/WorkspaceDropdown';
import RecentDropdown from '../Dropdowns/RecentDropDown';
import StarredDropdown from '../Dropdowns/StarredDropdown';
import CreateDropdown from '../Dropdowns/CreateDropdown';


const Navbar = () => {
    //Created a state to track which dropdown is open
    const [openDropdown, setOpenDropdown] = useState(null);

    const handleDropdownToggle = (dropdownName) => {
        //Close the dropdown if it is already open
        if (openDropdown === dropdownName) {
            setOpenDropdown(null);
        }
        //Else open the selected dropdown 
        else {
            setOpenDropdown(dropdownName); 
        }
    };

    return (
        <div className='bg-gray-300 w-full h-20 flex justify-start items-center'>
            <h1 className='text-2xl font-bold text-blue-500 mx-4'>TaskIt</h1>
            {/*Created a list that contains the navbar components*/}
            {/*Each component contains 2 parameters 'isOpen' & 'toggleDropdown' that are sent in their own pages*/}
            <ul className='flex items-center'>
                <li className='px-2'>
                    <WorkspaceDropdown 
                        isOpen={openDropdown === 'workspace'}
                        toggleDropdown={() => handleDropdownToggle('workspace')}
                    />
                </li>
                <li className='px-2'>
                    <RecentDropdown 
                        isOpen={openDropdown === 'recent'}
                        toggleDropdown={() => handleDropdownToggle('recent')}
                    />
                </li>
                <li>
                    <StarredDropdown
                        isOpen={openDropdown === 'starred'}
                        toggleDropdown={() => handleDropdownToggle('starred')}
                    />
                </li>
                <li className='px-2'>
                    <CreateDropdown 
                        isOpen={openDropdown === 'create'}
                        toggleDropdown={() => handleDropdownToggle('create')}
                    />
                </li>                
            </ul>
        </div>
    );
}

export default Navbar;