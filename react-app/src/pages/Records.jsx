import { getBuildById, getRecordsByUserId, getCarById } from "../services/api";
import { useEffect, useState } from "react";

const Records = () => {
    const [records, setRecords] = useState([])
    const [carDetails, setCarDetails] = useState({})

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
            const car = await getCar(record.buildId)
            newCar[record.buildId] = {
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

    return (
        <div>
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
                                    ? `${carDetails[record.buildId].year} ${carDetails[record.buildId].make} ${carDetails[record.buildId].model}`
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
