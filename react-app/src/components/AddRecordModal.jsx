import { getRecordsByUserId, createRecord, getCoursesByGame } from "../services/api";
import { useEffect, useState, useRef } from "react";
import "../styles/AddRecordModal.css";
import { CLASS_OPTIONS } from "../constants/gameClasses";

const AddRecordModal = ({cars, selectedGame: initialGame, setRecords}) => {

    const inputTimeMin = useRef('')
    const inputTimeSec = useRef('')
    const inputTimeMs = useRef('')
    const selectedClass = useRef('')
    const selectedCourse = useRef('')
    const selectedCpuDiff = useRef('')
    const [carInputValue, setCarInputValue] = useState('')
    const [selectedCarId, setSelectedCarId] = useState('')
    const [modalGame, setModalGame] = useState(initialGame ?? 'FH6')
    const [modalCourses, setModalCourses] = useState([])
    const [createRecordError, setCreateRecordError] = useState(false)
    const [createRecordErrorMessage, setCreateRecordErrorMessage] = useState('')
    const [isLoading, setIsLoading] = useState(false)

    useEffect(() => {
        async function fetchCourses() {
            const data = await getCoursesByGame(modalGame)
            setModalCourses(Array.isArray(data) ? data : [])
            if (selectedClass.current) selectedClass.current.value = ''
            if (selectedCourse.current) selectedCourse.current.value = ''
        }
        fetchCourses()
    }, [modalGame])

    function handleCarInput(e) {
        const inputText = e.target.value;
        setCarInputValue(inputText);
        const match = cars.find(
            car => `${car.make} ${car.model} ${car.year}` === inputText
        );
        setSelectedCarId(match ? match.carId : '');
    }

    async function handleCreateSubmit(e){
        e.preventDefault()
        setIsLoading(true)

        if (!selectedCarId) {
            setCreateRecordErrorMessage('Please select a valid car from the list.');
            setCreateRecordError(true);
            setIsLoading(false);
            return;
        }

        let newRecord = {
            userId: sessionStorage.getItem("userId"),
            carId: selectedCarId,
            courseId: selectedCourse.current.value,
            classRank: selectedClass.current.value,
            timeMin: inputTimeMin.current.value,
            timeSec: inputTimeSec.current.value,
            timeMs: inputTimeMs.current.value,
            cpuDiff: selectedCpuDiff.current.value,
            deleted: 0
        }
        let response = await createRecord(newRecord)
        console.log(response.message)

        if(response.success){
            hideCreateScreen()
            console.log('record created successfully')

            inputTimeMin.current.value = '';
            inputTimeSec.current.value = '';
            inputTimeMs.current.value = '';
            setCarInputValue('');
            setSelectedCarId('');
            selectedClass.current.value = '';
            selectedCourse.current.value = '';
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
        let content = document.querySelector(".add-record-content")

        overlay.style.display = "none";
        content.style.display = "none";
    }


    return ( 
        <div className="create-content add-record-content">
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
                
                <input
                    type="text"
                    list="car-options"
                    value={carInputValue}
                    onChange={handleCarInput}
                    placeholder="Search for a car..."
                    required
                    className="create-car-input"
                />
                <datalist id="car-options">
                    {cars.map(car => (
                        <option key={car.carId} value={`${car.make} ${car.model} ${car.year}`} />
                    ))}
                </datalist>

                <div className="create-select-div">
                    <select value={modalGame} onChange={(e) => setModalGame(e.target.value)}>
                        <option value="FH6">FH6</option>
                        <option value="FH5">FH5</option>
                    </select>

                    <select required ref={selectedCourse}>
                        <option value=''>SELECT COURSE</option>
                        {modalCourses.map((course) => (
                            <option key={course.courseId} value={course.courseId}>
                                {course.name}
                            </option>
                        ))}
                    </select>

                    <select required ref={selectedClass}>
                        <option value=''>SELECT CLASS</option>
                        {CLASS_OPTIONS[modalGame]?.map(cls => (
                            <option key={cls} value={cls}>{cls}</option>
                        ))}
                    </select>

                    <select required ref={selectedCpuDiff}>
                        <option value=''>SELECT CPU LEVEL</option>
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