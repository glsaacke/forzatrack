import { getRecordsByUserId, createRecord } from "../services/api";
import { useState, useRef } from "react";
import "../styles/AddRecordModal.css"

const AddRecordModal = ({ cars, setRecords, onClose }) => {
    const inputTimeMin = useRef(null)
    const inputTimeSec = useRef(null)
    const inputTimeMs = useRef(null)
    const selectedCar = useRef(null)
    const selectedClass = useRef(null)
    const selectedEvent = useRef(null)
    const selectedCpuDiff = useRef(null)
    const [createRecordError, setCreateRecordError] = useState(false)
    const [createRecordErrorMessage, setCreateRecordErrorMessage] = useState('')
    const [isLoading, setIsLoading] = useState(false)

    function resetForm() {
        if (inputTimeMin.current) inputTimeMin.current.value = '';
        if (inputTimeSec.current) inputTimeSec.current.value = '';
        if (inputTimeMs.current) inputTimeMs.current.value = '';
        if (selectedCar.current) selectedCar.current.value = '';
        if (selectedClass.current) selectedClass.current.value = '';
        if (selectedEvent.current) selectedEvent.current.value = '';
        if (selectedCpuDiff.current) selectedCpuDiff.current.value = '';
    }

    async function handleCreateSubmit(e) {
        e.preventDefault()
        setIsLoading(true)
        setCreateRecordError(false)

        const newRecord = {
            userId: sessionStorage.getItem("userId"),
            carId: selectedCar.current.value,
            event: selectedEvent.current.value,
            classRank: selectedClass.current.value,
            timeMin: inputTimeMin.current.value,
            timeSec: inputTimeSec.current.value,
            timeMs: inputTimeMs.current.value,
            cpuDiff: selectedCpuDiff.current.value,
        }

        try {
            const response = await createRecord(newRecord)

            if (response.success) {
                resetForm()
                setCreateRecordError(false)
                setRecords(await getRecordsByUserId(sessionStorage.getItem("userId")))
                onClose()
            } else {
                setCreateRecordErrorMessage(response.message)
                setCreateRecordError(true)
            }
        } catch {
            setCreateRecordErrorMessage("An unexpected error occurred. Please try again.")
            setCreateRecordError(true)
        } finally {
            setIsLoading(false)
        }
    }

    return (
        <div className="create-content" style={{ display: "flex" }}>
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

                <select className="create-car-select" required ref={selectedCar} defaultValue="">
                    <option value="" disabled>SELECT CAR</option>
                    {cars.map((car) => (
                        <option key={car.carId} value={car.carId}>{car.make} {car.model} {car.year}</option>
                    ))}
                </select>

                <div className="create-select-div">
                    <select required ref={selectedClass} defaultValue="">
                        <option disabled value=''>SELECT CLASS</option>
                        <option value="X">X</option>
                        <option value="S2">S2</option>
                        <option value="S1">S1</option>
                        <option value="A">A</option>
                        <option value="B">B</option>
                        <option value="C">C</option>
                        <option value="D">D</option>
                        <option value="E">E</option>
                    </select>

                    <select required ref={selectedEvent} defaultValue="">
                        <option disabled value=''>SELECT EVENT</option>
                        <option value="Goliath">Goliath</option>
                        <option value="Colossus">Colossus</option>
                        <option value="Gauntlet">Gauntlet</option>
                        <option value="Titan">Titan</option>
                        <option value="Marathon">Marathon</option>
                        <option value="Vulcan Sprint">Vulcan Sprint</option>
                    </select>

                    <select required ref={selectedCpuDiff} defaultValue="">
                        <option disabled value=''>SELECT CPU LEVEL</option>
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
            <button className='create-form-cancel' onClick={onClose}>CANCEL</button>
        </div>
    );
}

export default AddRecordModal;