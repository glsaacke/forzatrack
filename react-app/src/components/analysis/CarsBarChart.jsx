import { useMemo } from "react";
import {
    BarChart,
    Bar,
    XAxis,
    YAxis,
    Tooltip,
    ResponsiveContainer,
    Cell,
} from "recharts";
import "../../styles/analysis/Charts.css";

const formatTime = (ms) => {
    const min = Math.floor(ms / 60000);
    const sec = Math.floor((ms % 60000) / 1000);
    const millis = ms % 1000;
    return `${min}:${String(sec).padStart(2, "0")}.${String(millis).padStart(3, "0")}`;
};

const CustomTooltip = ({ active, payload }) => {
    if (active && payload && payload.length) {
        return (
            <div className="chart-tooltip">
                <p>{payload[0].payload.name}</p>
                <p>{formatTime(payload[0].value)}</p>
            </div>
        );
    }
    return null;
};

const CarsBarChart = ({ filteredRecords, cars }) => {
    const data = useMemo(() => {
        if (!filteredRecords || filteredRecords.length === 0) return [];

        const carFastest = {};
        filteredRecords.forEach((record) => {
            const totalMs =
                record.timeMin * 60000 + record.timeSec * 1000 + record.timeMs;
            if (carFastest[record.carId] === undefined || totalMs < carFastest[record.carId]) {
                carFastest[record.carId] = totalMs;
            }
        });

        return Object.keys(carFastest)
            .map((carId) => {
                const car = cars.find((c) => c.carId === parseInt(carId));
                const label = car
                    ? `${car.make} ${car.model} '${String(car.year).slice(-2)}`
                    : `Car ${carId}`;
                return { name: label, time: carFastest[carId] };
            })
            .sort((a, b) => a.time - b.time);
    }, [filteredRecords, cars]);

    if (data.length === 0) {
        return (
            <div className="chart-container">
                <p className="chart-title">Cars by time</p>
                <p className="chart-empty">No data</p>
            </div>
        );
    }

    const minTime = data[0].time;
    const maxTime = data[data.length - 1].time;
    const padding = (maxTime - minTime) * 0.1 || 5000;

    return (
        <div className="chart-container">
            <p className="chart-title">Cars by time</p>
            <ResponsiveContainer width="100%" height={220}>
                <BarChart data={data} margin={{ top: 10, right: 10, left: 60, bottom: 60 }}>
                    <XAxis
                        dataKey="name"
                        tick={{ fontSize: 11, fill: "#333" }}
                        angle={-45}
                        textAnchor="end"
                        interval={0}
                    />
                    <YAxis
                        domain={[minTime - padding, maxTime + padding]}
                        tickFormatter={formatTime}
                        tick={{ fontSize: 11, fill: "#333" }}
                        width={70}
                    />
                    <Tooltip content={<CustomTooltip />} />
                    <Bar dataKey="time" radius={[4, 4, 0, 0]}>
                        {data.map((_, index) => (
                            <Cell key={index} fill="#FF48A5" />
                        ))}
                    </Bar>
                </BarChart>
            </ResponsiveContainer>
        </div>
    );
};

export default CarsBarChart;
