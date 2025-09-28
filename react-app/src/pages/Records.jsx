import AddRecordModal from "../components/AddRecordModal";
import { getBuildById, getRecordsByUserId, getCarById, getAllCars, createRecord, setRecordDeleted } from "../services/api";
import { useEffect, useState, useRef } from "react";

const Records = ({setOnDashboard}) => {
    const [records, setRecords] = useState([])
    const [cars, setCars] = useState([])
    const [filteredRecords, setFilteredRecords] = useState([])
    const [classFilter, setClassFilter] = useState('')
    const [eventFilter, setEventFilter] = useState('Goliath')
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
    }, [classFilter, records, eventFilter])


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

    function showCreateScreen (){
        let overlay = document.querySelector(".create-overlay")
        let content = document.querySelector(".create-content")
        let isVisible = overlay.style.display === "block";

        overlay.style.display = isVisible ? "none" : "block"
        content.style.display = isVisible ? "none" : "flex"
    }

    function handleFilterRecords() {
        let recordsPlaceholder = records
    
        if (classFilter !== '') {
            recordsPlaceholder = recordsPlaceholder.filter(record => record.classRank === classFilter)
        }
        
        recordsPlaceholder = recordsPlaceholder.filter(record => record.event === eventFilter)
    
        setFilteredRecords(recordsPlaceholder)
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
                            <h5>Class</h5>
                            <select className="form-select" onChange={(e) => {
                                setClassFilter(e.target.value)
                                handleFilterRecords()
                            }}>
                                <option value="" selected>All Classes</option>
                                <option value="X">X</option>
                                <option value="S2">S2</option>
                                <option value="S1">S1</option>
                                <option value="A">A</option>
                                <option value="B">B</option>
                                <option value="C">C</option>
                                <option value="D">D</option>
                                <option value="E">E</option>
                            </select>
                        </div>
                        <div className="record-selects-select">
                            <h5>Event</h5>
                            <select className="form-select" onChange={(e) => {
                                setEventFilter(e.target.value)
                                handleFilterRecords()
                            }}>
                                <option value="Goliath" selected>Goliath</option>
                                <option value="Colossus">Colossus</option>
                                <option value="Gauntlet">Gauntlet</option>
                                <option value="Titan">Titan</option>
                                <option value="Marathon">Marathon</option>
                                <option value="Vulcan Sprint">Vulcan Sprint</option>
                                
                            </select>
                        </div>
                    </div>
                    <button className="create-record" onClick={showCreateScreen}>ADD RECORD</button>
                </div>
                <div className="create-overlay"></div>

                <AddRecordModal cars={cars} setRecords={setRecords}/>

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
                            <th scope="col">Event</th>
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
                            <td>{record.event}</td>
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
