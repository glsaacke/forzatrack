import { createCar } from "../services/api";
import { useState, useRef } from "react";
import "../styles/AddRecordModal.css";

const AddCarModal = ({selectedGame, onCreated}) => {

    const inputMake = useRef('')
    const inputModel = useRef('')
    const inputYear = useRef('')
    const [modalGame, setModalGame] = useState(selectedGame ?? 'FH6')
    const [createCarError, setCreateCarError] = useState(false)
    const [createCarErrorMessage, setCreateCarErrorMessage] = useState('')
    const [isLoading, setIsLoading] = useState(false)

    async function handleCreateSubmit(e){
        e.preventDefault()
        setIsLoading(true)

        let newCar = {
            make: inputMake.current.value.trim(),
            model: inputModel.current.value.trim(),
            year: parseInt(inputYear.current.value, 10),
            game: modalGame,
            deleted: 0
        }
        let response = await createCar(newCar)

        if(response.success){
            hideAddCarScreen()

            inputMake.current.value = '';
            inputModel.current.value = '';
            inputYear.current.value = '';

            setCreateCarError(false)

            await onCreated()
        }
        else{
            setCreateCarErrorMessage(response.message)
            setCreateCarError(true)
        }

        setIsLoading(false)
    }

    function hideAddCarScreen(){
        let overlay = document.querySelector(".create-overlay")
        let content = document.querySelector(".add-car-content")

        overlay.style.display = "none";
        content.style.display = "none";
    }

    return (
        <div className="create-content add-car-content">
            <h2>ADD CAR</h2>
            <form onSubmit={handleCreateSubmit}>
                <input type="text" required placeholder="Make" ref={inputMake} className="create-car-input" />
                <input type="text" required placeholder="Model" ref={inputModel} className="create-car-input" />
                <input type="number" required placeholder="Year" min="1886" max="2100" ref={inputYear} className="create-car-input" />
                <div className="create-select-div">
                    <select value={modalGame} onChange={(e) => setModalGame(e.target.value)}>
                        <option value="FH6">FH6</option>
                        <option value="FH5">FH5</option>
                    </select>
                </div>
                {createCarError && <p className='login-error-message'>{createCarErrorMessage}</p>}
                <button className='create-form-submit' disabled={isLoading}>
                    {isLoading ? <div className="spinner"></div> : "GO"}
                </button>
            </form>
            <button className='create-form-cancel' onClick={hideAddCarScreen}>CANCEL</button>
        </div> );
}

export default AddCarModal;
