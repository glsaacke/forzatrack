import { useState } from 'react'
import Home from './pages/Home'
import NavBar from './components/NavBar'
import {Routes, Route} from 'react-router-dom'
import './styles/App.css'

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <div>
        <NavBar/>
        <main className='main-content'>
          <Routes>
            <Route path='/' element={<Home/>}/>
          </Routes>

        </main>
      </div>
    </>
  )
}

export default App
