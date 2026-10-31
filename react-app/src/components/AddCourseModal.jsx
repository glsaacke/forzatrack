import { createCourse } from "../services/api";
import { useState, useRef } from "react";
import "../styles/AddRecordModal.css";

const AddCourseModal = ({selectedGame, onCreated}) => {

    const inputName = useRef('')
    const [modalGame, setModalGame] = useState(selectedGame ?? 'FH6')
    const [createCourseError, setCreateCourseError] = useState(false)
    const [createCourseErrorMessage, setCreateCourseErrorMessage] = useState('')
    const [isLoading, setIsLoading] = useState(false)

    async function handleCreateSubmit(e){
        e.preventDefault()
        setIsLoading(true)

        let newCourse = {
            name: inputName.current.value.trim(),
            game: modalGame
        }
        let response = await createCourse(newCourse)

        if(response.success){
            hideAddCourseScreen()

            inputName.current.value = '';

            setCreateCourseError(false)

            await onCreated()
        }
        else{
            setCreateCourseErrorMessage(response.message)
            setCreateCourseError(true)
        }

        setIsLoading(false)
    }

    function hideAddCourseScreen(){
        let overlay = document.querySelector(".create-overlay")
        let content = document.querySelector(".add-course-content")

        overlay.style.display = "none";
        content.style.display = "none";
    }

    return (
        <div className="create-content add-course-content">
            <h2>ADD COURSE</h2>
            <form onSubmit={handleCreateSubmit}>
                <input type="text" required placeholder="Course name" ref={inputName} className="create-car-input" />
                <div className="create-select-div">
                    <select value={modalGame} onChange={(e) => setModalGame(e.target.value)}>
                        <option value="FH6">FH6</option>
                        <option value="FH5">FH5</option>
                    </select>
                </div>
                {createCourseError && <p className='login-error-message'>{createCourseErrorMessage}</p>}
                <button className='create-form-submit' disabled={isLoading}>
                    {isLoading ? <div className="spinner"></div> : "GO"}
                </button>
            </form>
            <button className='create-form-cancel' onClick={hideAddCourseScreen}>CANCEL</button>
        </div> );
}

export default AddCourseModal;
