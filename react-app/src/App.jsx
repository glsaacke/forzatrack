import { useState } from 'react'
import Home from './pages/Home'
import About from './pages/About'
import NavBar from './components/NavBar'
import {Routes, Route} from 'react-router-dom'
import './styles/App.css'
import Login from './pages/Login'
import Signup from './pages/Signup'
import NotFound from './pages/NotFound'

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <div>
        <NavBar/>
        <main className='main-content'>
          <Routes>
            <Route path='/' element={<Home/>}/>
            <Route path='/about' element={<About/>}/>
            <Route path='/login' element={<Login/>}/>
            <Route path='/signup' element={<Signup/>}/>
            <Route path='*' element={<NotFound/>}/>
          </Routes>

        </main>
      </div>
    </>
  )
}

export default App
