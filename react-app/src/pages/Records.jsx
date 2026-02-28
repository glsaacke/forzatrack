import AddRecordModal from "../components/AddRecordModal";
import AnalysisSection from "../components/AnalysisSection";
import { getRecordsByUserId, getAllCars, setRecordDeleted } from "../services/api";
import { useEffect, useState, useCallback } from "react";

const Records = ({ setOnDashboard }) => {
    const [records, setRecords] = useState([])
    const [cars, setCars] = useState([])
    const [filteredRecords, setFilteredRecords] = useState([])
    const [classFilter, setClassFilter] = useState('')
    const [eventFilter, setEventFilter] = useState('Goliath')
    const [selectedRecordId, setSelectedRecordId] = useState(null)
    const [recordsLoading, setRecordsLoading] = useState(false)
    const [showCreateModal, setShowCreateModal] = useState(false)
    const [showDeleteConfirm, setShowDeleteConfirm] = useState(false)

    useEffect(() => {
        setOnDashboard(true)

        async function loadData() {
            setRecordsLoading(true)
            try {
                const [recordResponse, carResponse] = await Promise.all([
                    getRecordsByUserId(sessionStorage.getItem("userId")),
                    getAllCars(),
                ])
                setRecords(recordResponse)
                setCars(carResponse)
            } catch {
                // Errors are surfaced by the apiFetch helper
            } finally {
                setRecordsLoading(false)
            }
        }

        loadData()
    }, [setOnDashboard])

    useEffect(() => {
        let result = records

        if (classFilter !== '') {
            result = result.filter(record => record.classRank === classFilter)
        }

        result = result.filter(record => record.event === eventFilter)

        setFilteredRecords(result)
    }, [classFilter, records, eventFilter])

    const handleDeleteRecord = useCallback(async () => {
        if (selectedRecordId === null) return

        try {
            await setRecordDeleted(selectedRecordId)
            setRecords(prev => prev.filter(record => record.recordId !== selectedRecordId))
        } catch {
            // Delete failed silently — record remains in list
        } finally {
            setSelectedRecordId(null)
            setShowDeleteConfirm(false)
        }
    }, [selectedRecordId])

    function formatDate(date) {
        const d = new Date(date)
        const month = String(d.getMonth() + 1)
        const day = String(d.getDate())
        const year = d.getFullYear()
        return `${month}/${day}/${year}`
    }

    function displayCar(id) {
        const car = cars.find(c => c.carId === id)
        return car ? `${car.make} ${car.model} ${car.year}` : ""
    }

    return (
        <div className="record-container">
            <h2>DASHBOARD</h2>
            <div className="record-content">
                <div className="record-functions">
                    <h3>CONFIGURE VIEW</h3>
                    <div className="record-selects">
                        <div className="record-selects-select">
                            <h5>Class</h5>
                            <select className="form-select" value={classFilter} onChange={(e) => setClassFilter(e.target.value)}>
                                <option value="">All Classes</option>
                                <option value="X">X</option>
                                <option value="S2">S2</option>
                                <option value="S1">S1</option>
                                <option value="A">A</option>
                                <option value="B">B</option>
                                <option value="C">C</option>
                                <option value="D">D</option>
                                <option value="E">E</option>
                            </select>
                        </div>
                        <div className="record-selects-select">
                            <h5>Event</h5>
                            <select className="form-select" value={eventFilter} onChange={(e) => setEventFilter(e.target.value)}>
                                <option value="Goliath">Goliath</option>
                                <option value="Colossus">Colossus</option>
                                <option value="Gauntlet">Gauntlet</option>
                                <option value="Titan">Titan</option>
                                <option value="Marathon">Marathon</option>
                                <option value="Vulcan Sprint">Vulcan Sprint</option>
                            </select>
                        </div>
                    </div>
                    <button className="create-record" onClick={() => setShowCreateModal(true)}>ADD RECORD</button>
                </div>
                <AnalysisSection filteredRecords={filteredRecords} cars={cars} />

                {showCreateModal && <div className="create-overlay" onClick={() => setShowCreateModal(false)}></div>}

                {showCreateModal && (
                    <AddRecordModal
                        cars={cars}
                        setRecords={setRecords}
                        onClose={() => setShowCreateModal(false)}
                    />
                )}

                {showDeleteConfirm && <div className="create-overlay" onClick={() => setShowDeleteConfirm(false)}></div>}

                {showDeleteConfirm && (
                    <div className="confirm-delete-content" style={{ display: "flex" }}>
                        <p>Are you sure you want to delete this record?</p>
                        <div className="confirm-delete-buttons">
                            <button className="record-delete btn btn-danger" onClick={handleDeleteRecord}>Delete</button>
                            <button className="record-delete btn btn-secondary" onClick={() => setShowDeleteConfirm(false)}>Cancel</button>
                        </div>
                    </div>
                )}

                {recordsLoading ? <div className="spinner"></div> :
                    <table className="table table-responsive record-table">
                        <thead className="thead-dark">
                            <tr>
                                <th scope="col">Ranking</th>
                                <th scope="col">Time</th>
                                <th scope="col">Class</th>
                                <th scope="col">Car</th>
                                <th scope="col">Event</th>
                                <th scope="col">Date</th>
                                <th scope="col">Delete</th>
                            </tr>
                        </thead>
                        <tbody>
                            {filteredRecords.filter(record => record.deleted === 0).map((record, index) => (
                                <tr key={record.recordId}>
                                    <td>{index + 1}</td>
                                    <td>{record.timeMin}:{record.timeSec}:{record.timeMs}</td>
                                    <td>{record.classRank}</td>
                                    <td>{displayCar(record.carId)}</td>
                                    <td>{record.event}</td>
                                    <td>{formatDate(record.addDate)}</td>
                                    <td>
                                        <button className='record-delete btn btn-danger' onClick={() => {
                                            setSelectedRecordId(record.recordId)
                                            setShowDeleteConfirm(true)
                                        }}>Delete</button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>}
            </div>
        </div>
    )
}

export default Records
