import { useState, useEffect } from "react";
import "../../styles/analysis/TopCards.css"

const MostConsistentCarCard = ({filteredRecords, cars}) => {
    const [mostConsistentCar, setMostConsistentCar] = useState("")

    useEffect(() => {
        if (filteredRecords && filteredRecords.length > 0) {
            const times = {}

            filteredRecords.forEach(record => {
                const totalMs = record.timeMin * 60000 + record.timeSec * 1000 + record.timeMs
                if (!times[record.carId]) {
                    times[record.carId] = []
                }
                times[record.carId].push(totalMs)
            })

            let bestCarId = null
            let bestStdDev = Infinity

            Object.keys(times).forEach(carId => {
                const carTimes = times[carId]
                if (carTimes.length < 2) return
                const mean = carTimes.reduce((sum, t) => sum + t, 0) / carTimes.length
                const variance = carTimes.reduce((sum, t) => sum + Math.pow(t - mean, 2), 0) / carTimes.length
                const stdDev = Math.sqrt(variance)
                if (stdDev < bestStdDev) {
                    bestStdDev = stdDev
                    bestCarId = parseInt(carId)
                }
            })

            if (bestCarId === null) {
                setMostConsistentCar("None")
            } else {
                const car = cars.find(c => c.carId === bestCarId)
                setMostConsistentCar(car ? `${car.make} ${car.model} ${car.year}` : "None")
            }
        } else {
            setMostConsistentCar("None")
        }
    }, [filteredRecords, cars])

    return (
        <div className="topcard-container">
            <h5>Most Consistent Car</h5>
            <p>{mostConsistentCar}</p>
        </div>
    );
}

export default MostConsistentCarCard;
