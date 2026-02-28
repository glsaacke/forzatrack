import { useState, useEffect } from "react";
import "../../styles/analysis/TopCards.css"

const FastestCarCard = ({ filteredRecords, cars }) => {
    const [fastestCar, setFastestCar] = useState("")

    useEffect(() => {
        if (filteredRecords && filteredRecords.length > 0) {
            const car = cars.find(c => c.carId === filteredRecords[0].carId)
            setFastestCar(car ? `${car.make} ${car.model} ${car.year}` : "Unknown")
        } else {
            setFastestCar("None")
        }
    }, [filteredRecords, cars])

    return (
        <div className="topcard-container">
            <h5>Fastest Car (time)</h5>
            <p>{fastestCar}</p>
        </div>
    );
}

export default FastestCarCard;