import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Login from './Pages/loginPage.jsx';
import Dashboard from './Pages/dashboard.jsx';
import Main from './Pages/Main.jsx'
import Preview from './Components/Preview/header.jsx';
import Boards from './Components/ContentFromSide/Boards.jsx';

import SignUpPage from './Pages/signUpPage.jsx';
import Board from './Components/Board.jsx';
import Test from './Components/Test.jsx';

function App() {



  return (
   <> 
   
 
      <BrowserRouter>
        <Routes>
          <Route path="/main" element={<Main/>}/>
          <Route path="/boards" element={<Boards/>}/>
          <Route path="/login" element={<Login/>}/>
       
          <Route path="/dashboard" element={<Dashboard/>}/>
          <Route path="/preview" element={<Preview/>}/>
          <Route path="/boards/:id" element={<Board/>} />
          <Route path="/:userId/:workspaceId?/:boardId?" element={<Test/>} />
        </Routes>
      </BrowserRouter>

    </>
    
  );
}

export default App;
