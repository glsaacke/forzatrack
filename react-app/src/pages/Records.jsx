import { getBuildById, getRecordsByUserId, getCarById } from "../services/api";
import { useEffect, useState } from "react";

const Records = () => {
    const [records, setRecords] = useState([]);
    const [carDetails, setCarDetails] = useState({}); // Store the makes and models by buildId

    useEffect(() => {
        async function GetRecords() {
            const recordResponse = await getRecordsByUserId(sessionStorage.getItem("userId"));
            setRecords(recordResponse);
            fetchCarDetails(recordResponse);  // Fetch car details once records are loaded
        }

        GetRecords();
    }, []);

    // Fetch car makes and store them in the state
    async function fetchCarDetails(records) {
        const newCar = {};
        for (const record of records) {
            const car = await getCar(record.buildId);  // Fetch car data for each record
            newCar[record.buildId] = {
                make: car.make,
                model: car.model,
                year: car.year
            };  // Store make, model, and year for each buildId
        }
        setCarDetails(newCar);  // Set carDetails state with the new object containing make, model, and year
    }

    async function getCar(buildId) {
        const build = await getBuildById(buildId);
        const car = await getCarById(build.carId);
        return car;
    }

    return (
        <div>
            <table className="table">
                <thead className="thead-dark">
                    <tr>
                        <th scope="col">Ranking</th>
                        <th scope="col">Time</th>
                        <th scope="col">Class</th>
                        <th scope="col">Car</th> {/* Updated header to "Make & Model" */}
                    </tr>
                </thead>
                <tbody>
                    {records.map((record) => (
                        <tr key={record.recordId}>
                            <td>{record.recordId}</td>
                            <td>{record.timeMin}:{record.timeSec}:{record.timeMs}</td>
                            <td>{record.classRank}</td>
                            {/* Concatenate make and model into one column */}
                            <td>
                                {carDetails[record.buildId] 
                                    ? `${carDetails[record.buildId].year} ${carDetails[record.buildId].make} ${carDetails[record.buildId].model}`
                                    : "Loading..."}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default Records;
