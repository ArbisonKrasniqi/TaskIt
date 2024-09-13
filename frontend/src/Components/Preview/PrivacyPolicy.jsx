
import { useNavigate } from 'react-router-dom';
const PrivacyPolicy = ()=>{
    const navigate = useNavigate();


    return(

        <div className="min-h-screen h-full flex flex-col justify-center" style={{backgroundImage: 'linear-gradient(115deg, #1a202c, #2d3748)'}}>
        
                <h1 className="font-semibold font-sans text-gray-400 ml-20 mt-10 text-3xl">
                    Privacy Policy</h1>
        
                <div className="font-semibold font-sans text-gray-400 ml-20 mt-10 mb-30">
                    <h2 className="text-2xl">Introduction</h2><br/>
                    <p>Welcome to <i>TaskIt</i>. We value your privacy and are committed to
                         protecting your personal information. This Privacy Policy explains how we collect,
                          use, and safeguard your data when you use our application, which is designed to help
                         users manage projects, collaborate with others, and organize workspaces.</p>
                <br/><br/>
                <h2 className="text-2xl">Information We Collect</h2><br/>
                <ol className="list-decimal text-xl pl-5">
                    <li className="text-xl">Account Information
                        <ul className="list-disc pl-5 text-lg">
                            <li><b>Email Address:</b> Required for creating an account and logging into the platform.</li>
                            <li><b>Username:</b> Used for identification within the application.</li>
                            <li><b>Password:</b> Encrypted and securely stored to protect your account.</li>
                        </ul>
                    </li>
                    <br/>
                    <li className="text-xl">Workspace Information
                    <ul className="list-disc pl-5 text-lg">
                        <li><b>Workspace Name and Description: </b>When you create a workspace, we store the name and description you provide.</li>
                        <li><b>Members: </b>We store the list of members you invite to join your workspace, including their roles and permissions.</li>
                        </ul>
                    </li>
                    <li className="text-xl">User-Generated Content
                    <ul className="list-disc pl-5 text-lg">
                        <li><b>Projects, Boards, and Tasks:</b> Any data you create within the application, such as project boards, tasks, and lists, is stored securely to allow you to manage your projects effectively.</li>
                        <li><b>Comments and Attachments:</b> Any comments or files you upload to boards or tasks are stored to facilitate collaboration between users.</li>    
                    </ul>
                </li> 
                </ol>    
                <br/><br/>

                <h2 className="text-2xl">How We Use Your Information</h2><br/>
                <ol className="list-decimal text-xl pl-5">
                    <li className="text-xl">To Provide and Improve Our Services
                        <ul className="list-disc pl-5 text-lg">
                            <li>Your information is used to create and maintain your account, enable collaboration features, and ensure the proper functioning of the application.</li>
                        </ul>
                    </li>
                    <br/>
                    <li className="text-xl">To Enhance User Experience
                    <ul className="list-disc pl-5 text-lg">
                        <li>We may analyze usage data to understand how our users interact with the application, allowing us to improve features and user experience.</li>
                       </ul>
                    </li>
                    <li className="text-xl">To Ensure Security
                    <ul className="list-disc pl-5 text-lg">
                        <li>We use account information and activity logs to detect and prevent fraudulent activities and to ensure the security of your data.</li>
                  </ul>
                </li> 
                </ol>   
                <br/><br/>

                <h2 className="text-2xl">Sharing Your Information</h2><br/>
                <ol className="list-decimal text-xl pl-5">
                    <li className="text-xl">With Other Users
                        <ul className="list-disc pl-5 text-lg">
                            <li><b>Workspace Members: </b>When you invite others to join your workspace, they will be able to see your username and any content shared within that workspace.</li>
                            <li><b>Collaborators: </b>Other users with whom you collaborate on projects will have access to relevant data within those projects.</li>
                        </ul>
                    </li>
                </ol>   
                <br/><br/>

                <h2 className="text-2xl">Data Security</h2><br/>
                <ol className="list-decimal text-xl pl-5">
                    <li className="text-xl">Encryption
                        <ul className="list-disc pl-5 text-lg">
                            <li>We use industry-standard encryption methods to protect your data both in transit and at rest.</li>
                        </ul>
                    </li>
                    <br/>
                    <li className="text-xl">Access Controls
                    <ul className="list-disc pl-5 text-lg">
                        <li>Access to your data is restricted to authorized personnel who need it to perform their job functions.</li>
                       </ul>
                    </li>
                    <li className="text-xl">Regular Audits
                    <ul className="list-disc pl-5 text-lg">
                        <li>We conduct regular security audits to ensure the integrity and security of your data.</li>
                  </ul>
                </li> 
                </ol>
                <br/><br/>

                
                <h2 className="text-2xl">Your Rights</h2><br/>
                <ol className="list-decimal text-xl pl-5">
                    <li className="text-xl">Access and Update Your Information
                        <ul className="list-disc pl-5 text-lg">
                            <li>You can access and update your account information at any time through your account settings.</li>
                        </ul>
                    </li>
                    <br/>
                    <li className="text-xl">Delete Your Account
                    <ul className="list-disc pl-5 text-lg">
                        <li>If you wish to delete your account, you can do so through the application settings. Upon deletion, all your data will be permanently removed from our servers.</li>
                       </ul>
                    </li>
                    <li className="text-xl">Data Portability
                    <ul className="list-disc pl-5 text-lg">
                        <li>You have the right to request a copy of your data in a machine-readable format.</li>
                  </ul>
                </li> 
                </ol>
                <br/><br/>

                <h2 className="text-2xl">Changes to This Policy</h2><br/>
                <p className="pl-5 text-lg">We may update this Privacy Policy from time to time. Any changes will be communicated to you through email or within the application.</p>
                <br/><br/>

                <h2 className="text-2xl">Contact Us</h2><br/>
                <p className="pl-5 text-lg">If you have any questions or concerns about this Privacy Policy, please <button onClick={() => navigate(`/ContactUs`)} className="text-blue-600 underline">Contact Us</button>.</p>
               
                <br/><br/><br/>
                
                </div>
            
            
            </div>


    );
}
export default PrivacyPolicy;