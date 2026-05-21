import { useState, useEffect } from "react";
import "../../styles/analysis/TopCards.css"

const FastestCarCard = ({filteredRecords, cars}) => {

    const [fastestCar, setFastestCar] = useState("")
    
    useEffect(() => {
        console.log(filteredRecords)
        if(filteredRecords && filteredRecords.length > 0){
            cars.forEach(car => {
                if(car.carId == filteredRecords[0].carId){
                    setFastestCar(`${car.make} ${car.model} ${car.year}`)
                }
            })
        }
        else{
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