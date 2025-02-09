import { getBuildById, getRecordsByUserId, getCarById, getAllCars, createRecord } from "../services/api";
import { useEffect, useState, useRef } from "react";

const Records = () => {
    const [records, setRecords] = useState([])
    const [cars, setCars] = useState([])
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
        let overlay = document.querySelector(".create-overlay");
        let content = document.querySelector(".create-content");
        let isVisible = overlay.style.display === "block";

        overlay.style.display = isVisible ? "none" : "block";
        content.style.display = isVisible ? "none" : "flex";
    }

    function hideCreateScreen(){
        let overlay = document.querySelector(".create-overlay");
        let content = document.querySelector(".create-content");

        overlay.style.display = "none";
        content.style.display = "none";
    }

    async function handleCreateSubmit(){
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
        console.log(newRecord)
        createRecord(newRecord)
    }

    return (
        <div className="record-content">
            <div className="record-functions">
                <div className="record-selects">
                    <select className="form-select" aria-label="Default select example">
                        <option value="" selected>All Classes</option>
                        <option value="S2">S2</option>
                        <option value="S1">S1</option>
                        <option value="A">A</option>
                        <option value="B">B</option>
                        <option value="C">C</option>
                        <option value="D">D</option>
                        <option value="E">E</option>
                    </select>
                    <select>
                        <option value="" selected>Goliath</option>
                        <option value="1">Colossus</option>
                        <option value="2">Two</option>
                        <option value="3">Three</option>
                    </select>
                </div>
                <button className="create-record" onClick={showCreateScreen}>ADD RECORD</button>
            </div>
            <div className="create-overlay"></div>
            <div className="create-content">
                <h1>ADD RECCORD</h1>
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
                            <option selected>SELECT CLASS</option>
                            <option value="S2">S2</option>
                            <option value="S1">S1</option>
                            <option value="A">A</option>
                            <option value="B">B</option>
                            <option value="C">C</option>
                            <option value="D">D</option>
                            <option value="E">E</option>
                        </select>

                        <select required ref={selectedEvent}>
                            <option selected>SELECT EVENT</option>
                            <option value="">Goliath</option>
                            <option value="1">Colossus</option>
                            <option value="2">Two</option>
                            <option value="3">Three</option>
                        </select>

                        <select required ref={selectedCpuDiff}>
                            <option selected>SELECT CPU LEVEL</option>
                            <option value="">Unbeatable</option>
                            <option value="">Pro</option>
                            <option value="1">Expert</option>
                            <option value="1">Highly Skilled</option>
                            <option value="1">Above Average</option>
                            <option value="2">Average</option>
                            <option value="2">Novice</option>
                            <option value="2">New Racer</option>
                            <option value="3">Tourist</option>
                        </select>
                    </div>
                    
                    <button className='create-form-submit' type="submit">GO</button>
                </form>
                <button className='create-form-back' onClick={hideCreateScreen}>BACK</button>
            </div>
            <table className="table record-table">
                <thead className="thead-dark">
                    <tr>
                        <th scope="col">Ranking</th>
                        <th scope="col">Time</th>
                        <th scope="col">Class</th>
                        <th scope="col">Car</th>
                        <th scope="col">Date</th>
                    </tr>
                </thead>
                <tbody>
                    {records.map((record) => (
                        <tr key={record.recordId}>
                            <td>{record.recordId}</td>
                            <td>{record.timeMin}:{record.timeSec}:{record.timeMs}</td>
                            <td>{record.classRank}</td>
                            <td>{displayCar(record.carId)}</td>
                            <td>{formatDate(record.date)}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    )
}

export default Records
