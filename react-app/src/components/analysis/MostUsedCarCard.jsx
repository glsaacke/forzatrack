import { useState, useEffect } from "react";
import "../../styles/analysis/TopCards.css"

const MostUsedCarCard = ({filteredRecords, cars}) => {
    const [mostUsedCar, setMostUsedCar] = useState("")

    useEffect(() => {
        if (filteredRecords && filteredRecords.length > 0) {
            const counts = {}

            filteredRecords.forEach(record => {
                counts[record.carId] = (counts[record.carId] || 0) + 1
            })

            let bestCarId = null
            let bestCount = 0
            Object.keys(counts).forEach(carId => {
                if (counts[carId] > bestCount) {
                    bestCount = counts[carId]
                    bestCarId = parseInt(carId)
                }
            })

            const car = cars.find(c => c.carId === bestCarId)
            setMostUsedCar(car ? `${car.make} ${car.model} ${car.year}` : "None")
        } else {
            setMostUsedCar("None")
        }
    }, [filteredRecords, cars])

    return (
        <div className="topcard-container">
            <h5>Most Used Car</h5>
            <p>{mostUsedCar}</p>
        </div>
    );
}

export default MostUsedCarCard;
