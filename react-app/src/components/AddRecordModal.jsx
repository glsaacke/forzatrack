import { getBuildById, getRecordsByUserId, createRecord, } from "../services/api";
import { useEffect, useState, useRef } from "react";
import "../styles/AddRecordModal.css"

const AddRecordModal = ({cars, setRecords}) => {

    const inputTimeMin = useRef('')
    const inputTimeSec = useRef('')
    const inputTimeMs = useRef('')
    const selectedCar = useRef('')
    const selectedClass = useRef('')
    const selectedEvent = useRef('')
    const selectedCpuDiff = useRef('')
    const [createRecordError, setCreateRecordError] = useState(false)
    const [createRecordErrorMessage, setCreateRecordErrorMessage] = useState('')
    const [isLoading, setIsLoading] = useState(false)


    async function handleCreateSubmit(e){
        e.preventDefault()
        setIsLoading(true)

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

        setIsLoading(false)
    }

    function hideCreateScreen(){
        let overlay = document.querySelector(".create-overlay")
        let content = document.querySelector(".create-content")

        overlay.style.display = "none";
        content.style.display = "none";
    }


    return ( 
        <div className="create-content">
            <h2>ADD RECORD</h2>
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
                <button className='create-form-submit' disabled={isLoading}>
                    {isLoading ? <div className="spinner"></div> : "GO"}
                </button>
            </form>
            <button className='create-form-cancel' onClick={hideCreateScreen}>CANCEL</button>
        </div> );
}
 
export default AddRecordModal;