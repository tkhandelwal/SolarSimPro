// src/components/SolarSystemForm.jsx
import React, { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

const SolarSystemForm = () => {
    const { projectId } = useParams();
    const navigate = useNavigate();

    const [solarSystem, setSolarSystem] = useState({
        name: '',
        totalCapacityKWp: 0,
        numberOfModules: 0,
        moduleArea: 0,
        tilt: 30, // Default tilt
        azimuth: 180, // Default azimuth (south-facing)
        panelModelId: null,
        inverterModelId: null
    });

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');

    const handleInputChange = (e) => {
        const { name, value } = e.target;

        // Handle numeric values
        if (['totalCapacityKWp', 'numberOfModules', 'moduleArea', 'tilt', 'azimuth'].includes(name)) {
            setSolarSystem({
                ...solarSystem,
                [name]: parseFloat(value) || 0
            });
        } else {
            setSolarSystem({
                ...solarSystem,
                [name]: value
            });
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError('');

        try {
            const response = await fetch(`/api/projects/${projectId}/systems`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(solarSystem)
            });

            if (response.ok) {
                navigate(`/projects/${projectId}`);
            } else {
                const errorData = await response.text();
                console.error('Failed to create solar system:', errorData);
                setError('Failed to create solar system. Please check your input and try again.');
            }
        } catch (error) {
            console.error('Error creating solar system:', error);
            setError('An error occurred while creating the solar system.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="form-container">
            <h2 className="form-title">Add Solar System</h2>

            {error && <div className="error-message">{error}</div>}

            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="name">System Name*</label>
                    <input
                        type="text"
                        id="name"
                        name="name"
                        className="form-control"
                        value={solarSystem.name}
                        onChange={handleInputChange}
                        required
                    />
                </div>

                <div className="form-row">
                    <div className="form-group">
                        <label htmlFor="totalCapacityKWp">Total Capacity (kWp)*</label>
                        <input
                            type="number"
                            step="0.1"
                            id="totalCapacityKWp"
                            name="totalCapacityKWp"
                            className="form-control"
                            value={solarSystem.totalCapacityKWp}
                            onChange={handleInputChange}
                            required
                        />
                    </div>

                    <div className="form-group">
                        <label htmlFor="numberOfModules">Number of Modules*</label>
                        <input
                            type="number"
                            id="numberOfModules"
                            name="numberOfModules"
                            className="form-control"
                            value={solarSystem.numberOfModules}
                            onChange={handleInputChange}
                            required
                        />
                    </div>
                </div>

                <div className="form-group">
                    <label htmlFor="moduleArea">Module Area (m²)</label>
                    <input
                        type="number"
                        step="0.1"
                        id="moduleArea"
                        name="moduleArea"
                        className="form-control"
                        value={solarSystem.moduleArea}
                        onChange={handleInputChange}
                    />
                </div>

                <div className="form-row">
                    <div className="form-group">
                        <label htmlFor="tilt">Panel Tilt (degrees)</label>
                        <input
                            type="number"
                            min="0"
                            max="90"
                            id="tilt"
                            name="tilt"
                            className="form-control"
                            value={solarSystem.tilt}
                            onChange={handleInputChange}
                        />
                    </div>

                    <div className="form-group">
                        <label htmlFor="azimuth">Azimuth (degrees)</label>
                        <input
                            type="number"
                            min="0"
                            max="359"
                            id="azimuth"
                            name="azimuth"
                            className="form-control"
                            value={solarSystem.azimuth}
                            onChange={handleInputChange}
                        />
                        <small className="form-text">0° = North, 90° = East, 180° = South, 270° = West</small>
                    </div>
                </div>

                <div className="form-actions">
                    <button
                        type="button"
                        className="button secondary"
                        onClick={() => navigate(`/projects/${projectId}`)}
                    >
                        Cancel
                    </button>
                    <button
                        type="submit"
                        className="button"
                        disabled={loading}
                    >
                        {loading ? 'Creating...' : 'Create Solar System'}
                    </button>
                </div>
            </form>
        </div>
    );
};

export default SolarSystemForm;