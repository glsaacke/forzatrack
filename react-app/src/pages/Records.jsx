import AddRecordModal from "../components/AddRecordModal";
import AddCarModal from "../components/AddCarModal";
import AddCourseModal from "../components/AddCourseModal";
import AnalysisSection from "../components/AnalysisSection";
import { getBuildById, getRecordsByUserId, getCarById, getAllCars, createRecord, setRecordDeleted, getCoursesByGame } from "../services/api";
import { useEffect, useState, useRef } from "react";
import "../styles/Dashboard.css";
import { CLASS_OPTIONS } from "../constants/gameClasses";

const Records = ({setOnDashboard}) => {
    const [records, setRecords] = useState([])
    const [cars, setCars] = useState([])
    const [courses, setCourses] = useState([])
    const [filteredRecords, setFilteredRecords] = useState([])
    const [classFilter, setClassFilter] = useState('')
    const [courseFilter, setCourseFilter] = useState(null)
    const [selectedGame, setSelectedGame] = useState('FH6')
    const [deleteRecordId, setDeleteRecordId] = useState(null)
    const [selectedRecordId, setSelectedRecordId] = useState(null)
    const [recordsLoading, setRecordsLoading] = useState(false)
    

    useEffect(() => {
        async function GetRecords() {
            const recordResponse = await getRecordsByUserId(sessionStorage.getItem("userId"))
            setRecords(recordResponse)
        }

        async function GetCars(){
            const carResponse = await getAllCars()
            setCars(carResponse)
        }
        
        setOnDashboard(true)
        setRecordsLoading(true)
        GetRecords()
        GetCars()
        setRecordsLoading(false)
    }, [])

    useEffect(() => {
        handleFilterRecords()
    }, [classFilter, records, courseFilter, selectedGame, courses])

    useEffect(() => {
        async function fetchCourses() {
            const data = await getCoursesByGame(selectedGame)
            setCourses(Array.isArray(data) ? data : [])
            setCourseFilter(null)
            setClassFilter('')
        }
        fetchCourses()
    }, [selectedGame])


    useEffect(() => {
        let response = null

        if (deleteRecordId !== null) {
            response = setRecordDeleted(deleteRecordId)
            setDeleteRecordId(null)
        }

    }, [deleteRecordId])

    function formatDate(date){
        const d = new Date(date);
        const month = String(d.getMonth() + 1)
        const day = String(d.getDate())
        const year = d.getFullYear()
        
        return `${month}/${day}/${year}`
    };

    function displayCar(id){
        let currentCar = {}
        cars.forEach(function(car){
            if (car.carId === id){
                currentCar = car
            }
        })
        return `${currentCar.make} ${currentCar.model} ${currentCar.year}`
    }

    function displayCourse(courseId) {
        return courses.find(c => c.courseId === courseId)?.name ?? 'Unknown'
    }

    function showCreateScreen (){
        let overlay = document.querySelector(".create-overlay")
        let content = document.querySelector(".add-record-content")
        let isVisible = overlay.style.display === "block";

        overlay.style.display = isVisible ? "none" : "block"
        content.style.display = isVisible ? "none" : "flex"
    }

    function showAddCarScreen (){
        let overlay = document.querySelector(".create-overlay")
        let content = document.querySelector(".add-car-content")

        overlay.style.display = "block"
        content.style.display = "flex"
    }

    function showAddCourseScreen (){
        let overlay = document.querySelector(".create-overlay")
        let content = document.querySelector(".add-course-content")

        overlay.style.display = "block"
        content.style.display = "flex"
    }

    async function refreshCars(){
        setCars(await getAllCars())
    }

    async function refreshCourses(){
        const data = await getCoursesByGame(selectedGame)
        setCourses(Array.isArray(data) ? data : [])
    }

    function handleFilterRecords() {
        const gameCourseIds = new Set(
            courses.filter(c => c.game === selectedGame).map(c => c.courseId)
        )

        let filtered = records.filter(r => gameCourseIds.has(r.courseId))

        if (classFilter !== '') {
            filtered = filtered.filter(r => r.classRank === classFilter)
        }

        if (courseFilter !== null) {
            filtered = filtered.filter(r => r.courseId === courseFilter)
        }

        setFilteredRecords(filtered)
    }

    function showConfirmDeleteScreen(){
        let overlay = document.querySelector(".create-overlay")
        let content = document.querySelector(".confirm-delete-content")
        let isVisible = overlay.style.display === "block";

        overlay.style.display = isVisible ? "none" : "block"
        content.style.display = isVisible ? "none" : "flex"
    }

    function hideConfirmDeleteScreen(){
        let overlay = document.querySelector(".create-overlay")
        let content = document.querySelector(".confirm-delete-content")

        overlay.style.display = "none";
        content.style.display = "none";
    }

    function removeDeletedRecord(recordId){
        setRecords(records.filter(record => record.recordId !== recordId))
    }

    return (
        <div className="record-container">
            <h2>DASHBOARD</h2>
            <div className="record-content">
                <div className="record-functions">
                    <h3>CONFIGURE VIEW</h3>
                    <div className="record-selects">
                        <div className="record-selects-select">
                            <h5>Game</h5>
                            <select className="form-select" value={selectedGame} onChange={(e) => setSelectedGame(e.target.value)}>
                                <option value="FH6">FH6</option>
                                <option value="FH5">FH5</option>
                            </select>
                        </div>
                        <div className="record-selects-select">
                            <h5>Course</h5>
                            <select className="form-select" value={courseFilter ?? ''} onChange={(e) => {
                                const val = e.target.value
                                setCourseFilter(val === '' ? null : parseInt(val, 10))
                            }}>
                                <option value="">All Courses</option>
                                {courses.map((course) => (
                                    <option key={course.courseId} value={course.courseId}>{course.name}</option>
                                ))}
                            </select>
                        </div>
                        <div className="record-selects-select">
                            <h5>Class</h5>
                            <select className="form-select" value={classFilter} onChange={(e) => setClassFilter(e.target.value)}>
                                <option value="">All Classes</option>
                                {CLASS_OPTIONS[selectedGame].map(cls => (
                                    <option key={cls} value={cls}>{cls}</option>
                                ))}
                            </select>
                        </div>
                    </div>
                    <div className="record-buttons">
                        <button className="create-record" onClick={showAddCarScreen}>ADD CAR</button>
                        <button className="create-record" onClick={showAddCourseScreen}>ADD COURSE</button>
                        <button className="create-record" onClick={showCreateScreen}>ADD RECORD</button>
                    </div>
                </div>
                <AnalysisSection filteredRecords={filteredRecords} cars={cars} selectedGame={selectedGame}/>
                <div className="create-overlay"></div>

                <AddRecordModal cars={cars} selectedGame={selectedGame} setRecords={setRecords}/>
                <AddCarModal selectedGame={selectedGame} onCreated={refreshCars}/>
                <AddCourseModal selectedGame={selectedGame} onCreated={refreshCourses}/>

                <div className="confirm-delete-content">
                    <p>Are you sure you want to delete this record?</p>
                    <div className="confirm-delete-buttons">
                        <button className="record-delete btn btn-danger" onClick={() => {
                            setDeleteRecordId(selectedRecordId)
                            removeDeletedRecord(selectedRecordId)
                            hideConfirmDeleteScreen()
                            }}>Delete</button>
                        <button className="record-delete btn btn-secondary" onClick={hideConfirmDeleteScreen}>Cancel</button>
                    </div>
                </div>

                {recordsLoading ? <div className="spinner"></div> : 
                <table className="table table-responsive record-table">
                    <thead className="thead-dark">
                        <tr>
                            <th scope="col">Ranking</th>
                            <th scope="col">Time</th>
                            <th scope="col">Class</th>
                            <th scope="col">Car</th>
                            <th scope="col">Course</th>
                            <th scope="col">Date</th>
                            <th scope="col">Delete</th>
                        </tr>
                    </thead>
                    <tbody>
                    {filteredRecords.filter(record => record.deleted === 0).map((record, index) => (
                        <tr key={record.recordId}>
                            <td>{index + 1}</td>
                            <td>{record.timeMin}:{record.timeSec}:{record.timeMs}</td>
                            <td>{record.classRank}</td>
                            <td>{displayCar(record.carId)}</td>
                            <td>{displayCourse(record.courseId)}</td>
                            <td>{formatDate(record.addDate)}</td>
                            <td>
                                <button className='record-delete btn btn-danger' onClick={() => {
                                    setSelectedRecordId(record.recordId)
                                    showConfirmDeleteScreen()
                                }}>Delete</button>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>}
            </div>
        </div>
    )
}

export default Records
