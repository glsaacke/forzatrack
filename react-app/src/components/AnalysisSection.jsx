import "../styles/AnalysisSection.css"
import FastestCarCard from "./analysis/FastestCarCard";
import FastestAvgCarCard from "./analysis/FastestAvgCarCard";
import MostConsistentCarCard from "./analysis/MostConsistentCarCard";
import MostUsedCarCard from "./analysis/MostUsedCarCard";

const AnalysisSection = ({filteredRecords, cars}) => {
    return ( 
        <div className="analysis-container">
            <h3>ANALYSIS</h3>
            <div className="analysis-content">
                <div className="top-cards">
                    <FastestCarCard filteredRecords={filteredRecords} cars={cars}/>
                    <FastestAvgCarCard filteredRecords={filteredRecords} cars={cars}/>
                    <MostConsistentCarCard filteredRecords={filteredRecords} cars={cars}/>
                    <MostUsedCarCard filteredRecords={filteredRecords} cars={cars}/>
                </div>
            </div>
        </div>
     );
}
 
export default AnalysisSection;