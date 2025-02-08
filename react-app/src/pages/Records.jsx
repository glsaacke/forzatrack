import { getBuildById, getRecordsByUserId, getCarById } from "../services/api";
import { useEffect, useState, useRef } from "react";

const Records = () => {
    const [records, setRecords] = useState([])
    const [carDetails, setCarDetails] = useState({})
    const [newRecord, setNewRecord] = useState({})
    const timeMin = useRef('')
    const timeSec = useRef('')
    const timeMs = useRef('')
    const selectedCar = useRef('')
    const selectedClass = useRef('')

    useEffect(() => {
        async function GetRecords() {
            const recordResponse = await getRecordsByUserId(sessionStorage.getItem("userId"))
            setRecords(recordResponse)
            fetchCarDetails(recordResponse)
        }

        GetRecords()
    }, [])

    async function fetchCarDetails(records) {
        const newCar = {}
        for (const record of records) {
            const car = await getCar(record.carId)
            newCar[record.carId] = {
                make: car.make,
                model: car.model,
                year: car.year
            }
        }
        setCarDetails(newCar)
    }

    async function getCar(buildId) {
        const build = await getBuildById(buildId)
        const car = await getCarById(build.carId)
        return car
    }

    const formatDate = (date) => {
        const d = new Date(date);
        const month = String(d.getMonth() + 1)
        const day = String(d.getDate())
        const year = d.getFullYear()
        
        return `${month}/${day}/${year}`
    };

    function showCreateScreen (){
        let overlay = document.querySelector(".create-overlay");
        let content = document.querySelector(".create-content");

        // Toggle display property
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

    function handleCreateSubmit(){

    }

    return (
        <div className="record-content">
            <div className="record-functions">
                <div className="record-selects">
                    <select className="form-select" aria-label="Default select example">
                        <option value="" selected>All Classes</option>
                        <option value="1">S2</option>
                        <option value="2">S1</option>
                        <option value="3">A</option>
                        <option value="3">B</option>
                        <option value="3">C</option>
                        <option value="3">D</option>
                        <option value="3">E</option>
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
                    <label>TIME</label>
                    <input type="text" required ref={timeMin} />
                    <input type="text" required ref={timeSec} />
                    <input type="text" required ref={timeMs} />

                    <label>CAR</label>
                    <input type="text" required ref={selectedCar} />

                    <label>CLASS</label>
                    <input type="text" required ref={selectedClass} />

                    <button type="submit">GO</button>
                </form>
                <button onClick={hideCreateScreen}>BACK</button>
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
                            <td>
                                {carDetails[record.buildId] 
                                    ? `${carDetails[record.carId].year} ${carDetails[record.carId].make} ${carDetails[record.carId].model}`
                                    : "Loading..."}
                            </td>
                            <td>{formatDate(record.date)}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    )
}

export default Records
