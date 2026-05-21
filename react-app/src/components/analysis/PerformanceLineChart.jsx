import { useState, useMemo } from "react";
import {
    LineChart,
    Line,
    XAxis,
    YAxis,
    Tooltip,
    ResponsiveContainer,
    Dot,
} from "recharts";
import "../../styles/analysis/Charts.css";

const formatTime = (ms) => {
    const min = Math.floor(ms / 60000);
    const sec = Math.floor((ms % 60000) / 1000);
    const millis = ms % 1000;
    return `${min}:${String(sec).padStart(2, "0")}.${String(millis).padStart(3, "0")}`;
};

const formatDate = (dateStr) =>
    new Date(dateStr).toLocaleDateString("en-US", { month: "numeric", day: "numeric", year: "numeric" });

const CustomTooltip = ({ active, payload }) => {
    if (active && payload && payload.length) {
        return (
            <div className="chart-tooltip">
                <p>{formatDate(payload[0].payload.date)}</p>
                <p>{formatTime(payload[0].value)}</p>
            </div>
        );
    }
    return null;
};

const PerformanceLineChart = ({ filteredRecords, cars }) => {
    const carsInRecords = useMemo(() => {
        if (!filteredRecords || filteredRecords.length === 0) return [];
        const seen = new Set();
        const result = [];
        filteredRecords.forEach((r) => {
            if (!seen.has(r.carId)) {
                seen.add(r.carId);
                const car = cars.find((c) => c.carId === r.carId);
                if (car) result.push(car);
            }
        });
        return result;
    }, [filteredRecords, cars]);

    const [selectedCarId, setSelectedCarId] = useState(null);

    const effectiveCarId =
        selectedCarId !== null && carsInRecords.some((c) => c.carId === selectedCarId)
            ? selectedCarId
            : carsInRecords[0]?.carId ?? null;

    const data = useMemo(() => {
        if (!filteredRecords || effectiveCarId === null) return [];
        return filteredRecords
            .filter((r) => r.carId === effectiveCarId)
            .map((r) => ({
                date: r.addDate,
                time: r.timeMin * 60000 + r.timeSec * 1000 + r.timeMs,
            }))
            .sort((a, b) => new Date(a.date) - new Date(b.date));
    }, [filteredRecords, effectiveCarId]);

    const selectedCarLabel = useMemo(() => {
        const car = carsInRecords.find((c) => c.carId === effectiveCarId);
        return car ? `${car.make} ${car.model} '${String(car.year).slice(-2)}` : "";
    }, [carsInRecords, effectiveCarId]);

    if (carsInRecords.length === 0) {
        return (
            <div className="chart-container">
                <p className="chart-title">Performance Over Time</p>
                <p className="chart-empty">No data</p>
            </div>
        );
    }

    const times = data.map((d) => d.time);
    const minTime = Math.min(...times);
    const maxTime = Math.max(...times);
    const padding = (maxTime - minTime) * 0.1 || 5000;

    return (
        <div className="chart-container">
            <p className="chart-title">Performance Over Time</p>
            <ResponsiveContainer width="100%" height={220}>
                <LineChart data={data} margin={{ top: 10, right: 10, left: 60, bottom: 20 }}>
                    <XAxis
                        dataKey="date"
                        tickFormatter={formatDate}
                        tick={{ fontSize: 11, fill: "#333" }}
                        label={{ value: "Date", position: "insideBottom", offset: -10, fontSize: 12 }}
                    />
                    <YAxis
                        domain={[minTime - padding, maxTime + padding]}
                        tickFormatter={formatTime}
                        tick={{ fontSize: 11, fill: "#333" }}
                        width={70}
                        label={{ value: "Time", angle: -90, position: "insideLeft", offset: -50, fontSize: 12 }}
                    />
                    <Tooltip content={<CustomTooltip />} />
                    <Line
                        type="monotone"
                        dataKey="time"
                        stroke="#111111"
                        strokeWidth={2}
                        dot={<Dot r={5} fill="#F0AC2D" stroke="#F0AC2D" />}
                        activeDot={{ r: 7, fill: "#F0AC2D", stroke: "#F0AC2D" }}
                    />
                </LineChart>
            </ResponsiveContainer>
            <div className="chart-car-select">
                <label htmlFor="perf-car-select">Car:</label>
                <select
                    id="perf-car-select"
                    value={effectiveCarId ?? ""}
                    onChange={(e) => setSelectedCarId(parseInt(e.target.value))}
                >
                    {carsInRecords.map((car) => (
                        <option key={car.carId} value={car.carId}>
                            {car.make} {car.model} {car.year}
                        </option>
                    ))}
                </select>
            </div>
        </div>
    );
};

export default PerformanceLineChart;
