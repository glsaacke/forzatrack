import { useState } from 'react'
import Home from './pages/Home'
import About from './pages/About'
import NavBar from './components/NavBar'
import {Routes, Route, Navigate} from 'react-router-dom'
import './styles/App.css'
import Login from './pages/Login'
import Signup from './pages/Signup'
import NotFound from './pages/NotFound'
import PrivateRoute from './components/PrivateRoute'
import DashLayout from './components/DashLayout'
import Builds from './pages/Builds'
import Records from './pages/Records'

function App() {
  const [onDashboard, setOnDashboard] = useState(false)

  return (
    <>
      <div>
        <NavBar onDashboard={onDashboard}/>
        <main className='main-content'>
          <Routes>
            <Route path='/' element={<Home setOnDashboard={setOnDashboard}/>}/>
            <Route path='/about' element={<About/>}/>
            <Route path='/login' element={<Login/>}/>
            <Route path='/signup' element={<Signup/>}/>
            <Route path='/dashboard' element={<PrivateRoute/>}>
              <Route index element={<Navigate to="records" />} />
              <Route path='records' element={<Records setOnDashboard={setOnDashboard}/>}/>
              {/* <Route path='builds' element={<Builds/>}/> */}
            </Route>
            <Route path='*' element={<NotFound/>}/>
          </Routes>

        </main>
      </div>
    </>
  )
}

export default App
