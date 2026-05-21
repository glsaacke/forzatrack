import { useState, useEffect } from "react";
import "../../styles/analysis/TopCards.css"

const FastestAvgCarCard = ({filteredRecords, cars}) => {
    const [fastestAvgCar, setFastestAvgCar] = useState("")

    useEffect(() => {
        if (filteredRecords && filteredRecords.length > 0) {
            const avgTimes = {}
            const counts = {}

            filteredRecords.forEach(record => {
                const totalMs = record.timeMin * 60000 + record.timeSec * 1000 + record.timeMs
                if (avgTimes[record.carId] === undefined) {
                    avgTimes[record.carId] = 0
                    counts[record.carId] = 0
                }
                avgTimes[record.carId] += totalMs
                counts[record.carId]++
            })

            let bestCarId = null
            let bestAvg = Infinity
            Object.keys(avgTimes).forEach(carId => {
                const avg = avgTimes[carId] / counts[carId]
                if (avg < bestAvg) {
                    bestAvg = avg
                    bestCarId = parseInt(carId)
                }
            })

            const car = cars.find(c => c.carId === bestCarId)
            setFastestAvgCar(car ? `${car.make} ${car.model} ${car.year}` : "None")
        } else {
            setFastestAvgCar("None")
        }
    }, [filteredRecords, cars])

    return (
        <div className="topcard-container">
            <h5>Fastest Car (average time)</h5>
            <p>{fastestAvgCar}</p>
        </div>
    );
}

export default FastestAvgCarCard;
