import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Login from './Pages/loginPage.jsx';
import Dashboard from './Pages/dashboard.jsx';
import Main from './Pages/Main.jsx'
import Preview from './Components/Preview/header.jsx';
import SignUpPage from './Pages/signUpPage.jsx';


function App() {



  return (
   <> 
      <BrowserRouter>
        <Routes>
          <Route path="/main" element={<Main/>}/>
          <Route path="/:opened/:workspaceId?/:boardId?/:taskId?" element={<Main/>}/>
          <Route path="/login" element={<Login/>}/>
          <Route path="/signup" element={<SignUpPage/>}/>
          <Route path="/dashboard" element={<Dashboard/>}/>
          <Route path="/preview" element={<Preview/>}/>
        </Routes>
      </BrowserRouter>

    </>
    
  );
}

export default App;
