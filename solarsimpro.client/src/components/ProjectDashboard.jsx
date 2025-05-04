// src/components/ProjectDashboard.jsx
import React, { useState, useEffect } from 'react';
import { MapContainer } from './MapContainer';
import { PanelDesigner } from './PanelDesigner';
import { SimulationResults } from './SimulationResults';

const ProjectDashboard = ({ projectId }) => {
    const [project, setProject] = useState(null);
    const [activeTab, setActiveTab] = useState('location');

    useEffect(() => {
        // Fetch project data from API
        fetchProject(projectId).then(data => setProject(data));
    }, [projectId]);

    return (
        <div className="dashboard">
            <nav className="dashboard-tabs">
                <button onClick={() => setActiveTab('location')}>Location</button>
                <button onClick={() => setActiveTab('panels')}>Panel Design</button>
                <button onClick={() => setActiveTab('simulation')}>Simulation</button>
                <button onClick={() => setActiveTab('financial')}>Financial Analysis</button>
                <button onClick={() => setActiveTab('report')}>Report</button>
            </nav>

            <div className="dashboard-content">
                {activeTab === 'location' && <MapContainer project={project} />}
                {activeTab === 'panels' && <PanelDesigner project={project} />}
                {activeTab === 'simulation' && <SimulationResults project={project} />}
                {/* Additional tabs */}
            </div>
        </div>
    );
};