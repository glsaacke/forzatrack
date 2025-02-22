import { getBuildById, getRecordsByUserId, getCarById, getAllCars, createRecord, setRecordDeleted } from "../services/api";
import { useEffect, useState, useRef } from "react";

const Records = () => {
    const [records, setRecords] = useState([])
    const [cars, setCars] = useState([])
    const [filteredRecords, setFilteredRecords] = useState([])
    const [classFilter, setClassFilter] = useState('')
    const [eventFilter, setEventFilter] = useState('Goliath')
    const [deleteRecordId, setDeleteRecordId] = useState(null)
    const [selectedRecordId, setSelectedRecordId] = useState(null)
    const [createRecordError, setCreateRecordError] = useState(false)
    const [createRecordErrorMessage, setCreateRecordErrorMessage] = useState('')
    const inputTimeMin = useRef('')
    const inputTimeSec = useRef('')
    const inputTimeMs = useRef('')
    const selectedCar = useRef('')
    const selectedClass = useRef('')
    const selectedEvent = useRef('')
    const selectedCpuDiff = useRef('')

    useEffect(() => {
        async function GetRecords() {
            const recordResponse = await getRecordsByUserId(sessionStorage.getItem("userId"))
            setRecords(recordResponse)
        }

        async function GetCars(){
            const carResponse = await getAllCars()
            setCars(carResponse)
        }

        GetRecords()
        GetCars()
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

    function hideCreateScreen(){
        let overlay = document.querySelector(".create-overlay")
        let content = document.querySelector(".create-content")

        overlay.style.display = "none";
        content.style.display = "none";
    }

    async function handleCreateSubmit(e){
        e.preventDefault()
        let newRecord = {
            userId: sessionStorage.getItem("userId"),
            carId: selectedCar.current.value,
            event: selectedEvent.current.value,
            classRank: selectedClass.current.value,
            timeMin: inputTimeMin.current.value,
            timeSec: inputTimeSec.current.value,
            timeMs: inputTimeMs.current.value,
            cpuDiff: selectedCpuDiff.current.value,
            deleted: 0
        }
        // console.log(`New record created: ${newRecord}`)
        let response = await createRecord(newRecord)
        console.log(response.message)

        if(response.success){
            hideCreateScreen()
            console.log('record created successfully')

            inputTimeMin.current.value = '';
            inputTimeSec.current.value = '';
            inputTimeMs.current.value = '';
            selectedCar.current.value = '';
            selectedClass.current.value = '';
            selectedEvent.current.value = '';
            selectedCpuDiff.current.value = '';

            setCreateRecordError(false)

            setRecords(await getRecordsByUserId(sessionStorage.getItem("userId")));
        }
        else{
            setCreateRecordErrorMessage(response.message)
            setCreateRecordError(true)
        }
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
        <div className="record-content">
            <div className="record-functions">
                <div className="record-selects">
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
                    <select className="form-select" onChange={(e) => {
                        setEventFilter(e.target.value)
                        handleFilterRecords()
                    }}>
                        <option value="Goliath" selected>Goliath</option>
                        <option value="Colossus">Colossus</option>
                        <option value="Gauntlet">Gauntlet</option>
                        <option value="Titan">Titan</option>
                        <option value="Marathon">Marathon</option>
                    </select>
                </div>
                <button className="create-record" onClick={showCreateScreen}>ADD RECORD</button>
            </div>
            <div className="create-overlay"></div>
            <div className="create-content">
                <h1>ADD RECORD</h1>
                <form onSubmit={handleCreateSubmit}>
                    <div className="create-time-div">
                        <label>TIME (mm:ss.mmm)</label>
                        <input type="text" required ref={inputTimeMin} />
                        <p>:</p>
                        <input type="text" required ref={inputTimeSec} />
                        <p>.</p>
                        <input type="text" required ref={inputTimeMs} />
                    </div>
                    
                    <select className="create-car-select" required ref={selectedCar}>
                        <option value='' selected>SELECT CAR</option>
                        {cars.map((car) => (
                            <option value={car.carId}>{car.make} {car.model} {car.year}</option>
                        ))}
                    </select>

                    <div className="create-select-div">
                        <select required ref={selectedClass}>
                            <option selected value=''>SELECT CLASS</option>
                            <option value="X">X</option>
                            <option value="S2">S2</option>
                            <option value="S1">S1</option>
                            <option value="A">A</option>
                            <option value="B">B</option>
                            <option value="C">C</option>
                            <option value="D">D</option>
                            <option value="E">E</option>
                        </select>

                        <select required ref={selectedEvent}>
                            <option selected value=''>SELECT EVENT</option>
                            <option value="Goliath">Goliath</option>
                            <option value="Colossus">Colossus</option>
                            <option value="Gauntlet">Gauntlet</option>
                            <option value="Titan">Titan</option>
                            <option value="Marathon">Marathon</option>
                        </select>

                        <select required ref={selectedCpuDiff}>
                            <option selected value=''>SELECT CPU LEVEL</option>
                            <option value="Unbeatable">Unbeatable</option>
                            <option value="Pro">Pro</option>
                            <option value="Expert">Expert</option>
                            <option value="Highly Skilled">Highly Skilled</option>
                            <option value="Above Average">Above Average</option>
                            <option value="Average">Average</option>
                            <option value="Novice">Novice</option>
                            <option value="New Racer">New Racer</option>
                            <option value="Tourist">Tourist</option>
                            <option value="None">None</option>
                        </select>
                    </div>
                    {createRecordError && <p className='login-error-message'>{createRecordErrorMessage}</p>}
                    <button className='create-form-submit' type="submit">GO</button>
                </form>
                <button className='create-form-back' onClick={hideCreateScreen}>BACK</button>
            </div>

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

            <table className="table record-table">
                <thead className="thead-dark">
                    <tr>
                        <th scope="col">Ranking</th>
                        <th scope="col">Time</th>
                        <th scope="col">Class</th>
                        <th scope="col">Car</th>
                        <th scope="col">Event</th>
                        <th scope="col">CPU</th>
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
                        <td>{record.cpuDiff}</td>
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
            </table>
        </div>
    )
}

export default Records
