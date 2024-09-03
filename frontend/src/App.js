import { BrowserRouter, Route, Routes } from 'react-router-dom';
import Login from './Pages/loginPage.jsx';
import Dashboard from './Pages/dashboard.jsx';
import Main from './Pages/Main.jsx'
import Preview from './Components/Preview/header.jsx';
import Boards from './Components/ContentFromSide/Boards.jsx';
import TaskModal from './Components/TaskModal/TaskModal.jsx';
import SignUpPage from './Pages/SignUpPage.jsx';

import React, { Suspense, lazy } from 'react';
import LoadingModal from './Components/Modal/LoadingModal.jsx';

const App = () => {
  return (
   <> 
      <BrowserRouter>
      <Suspense fallback={<LoadingModal />}>
        <Routes>
          <Route path="/main/:opened/:workspaceId?/:boardId?/:taskId?" element={<Main/>}/>
          <Route path="/login" element={<Login/>}/>
          <Route path="/signup" element={<SignUpPage/>}/>
          <Route path="/dashboard/*" element={<Dashboard/>}/>
          <Route path="/preview" element={<Preview/>}/>
          <Route path="/board/:id" element={<Boards/>} />
          <Route path="/task" element={<TaskModal/>}/>
        </Routes>
        </Suspense>
      </BrowserRouter>

    </>
    
  );
}

export default App;
