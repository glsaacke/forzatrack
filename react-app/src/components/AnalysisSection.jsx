import "../styles/AnalysisSection.css"
import FastestCarCard from "./analysis/FastestCarCard";

const AnalysisSection = ({filteredRecords, cars}) => {
    return ( 
        <div className="analysis-container">
            <h3>ANALYSIS</h3>
            <div className="analysis-content">
                <div className="top-cards">
                    <FastestCarCard filteredRecords={filteredRecords} cars={cars}/>
                </div>
            </div>
        </div>
     );
}
 
export default AnalysisSection;