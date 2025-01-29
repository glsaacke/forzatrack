import { useState } from 'react'
import Home from './pages/Home'
import About from './pages/About'
import NavBar from './components/NavBar'
import {Routes, Route} from 'react-router-dom'
import './styles/App.css'
import Login from './pages/Login'

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
          </Routes>

        </main>
      </div>
    </>
  )
}

export default App
