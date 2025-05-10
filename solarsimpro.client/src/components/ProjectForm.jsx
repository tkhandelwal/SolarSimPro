import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';

const ProjectForm = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const isNew = !id || id === 'new';

    const [project, setProject] = useState({
        name: '',
        clientName: '',
        location: '',
        latitude: 0,
        longitude: 0,
        altitude: 0,
        timeZone: '',
        albedo: 0.2,
        type: 'Residential'
    });

    const [loading, setLoading] = useState(!isNew);
    const [error, setError] = useState('');

    useEffect(() => {
        if (!isNew) {
            const fetchProject = async () => {
                try {
                    setLoading(true);
                    const response = await fetch(`/api/projects/${id}`);
                    if (response.ok) {
                        const data = await response.json();
                        setProject(data);
                    } else {
                        setError('Failed to load project. Please try again later.');
                    }
                } catch (error) {
                    console.error('Error fetching project:', error);
                    setError('An error occurred while fetching the project.');
                } finally {
                    setLoading(false);
                }
            };

            fetchProject();
        }
    }, [id, isNew]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;

        // Handle numeric values
        if (['latitude', 'longitude', 'altitude', 'albedo'].includes(name)) {
            setProject({
                ...project,
                [name]: parseFloat(value) || 0
            });
        } else {
            setProject({
                ...project,
                [name]: value
            });
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');

        try {
            const url = isNew ? '/api/projects' : `/api/projects/${id}`;
            const method = isNew ? 'POST' : 'PUT';

            const response = await fetch(url, {
                method,
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(project)
            });

            if (response.ok) {
                const data = isNew ? await response.json() : project;
                navigate(`/projects/${isNew ? data.id : id}`);
            } else {
                const errorData = await response.text();
                console.error('Failed to save project:', errorData);
                setError('Failed to save project. Please check your input and try again.');
            }
        } catch (error) {
            console.error('Error saving project:', error);
            setError('An error occurred while saving the project.');
        }
    };

    if (loading) {
        return (
            <div className="loading-container">
                <div className="loading-spinner"></div>
                <p className="loading-text">Loading project...</p>
            </div>
        );
    }

    return (
        <div className="form-container">
            <h2 className="form-title">{isNew ? 'Create New Project' : 'Edit Project'}</h2>

            {error && <div className="error-message">{error}</div>}

            <form onSubmit={handleSubmit}>
                <div className="form-group">
                    <label htmlFor="name">Project Name*</label>
                    <input
                        type="text"
                        id="name"
                        name="name"
                        className="form-control"
                        value={project.name}
                        onChange={handleInputChange}
                        required
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="clientName">Client Name</label>
                    <input
                        type="text"
                        id="clientName"
                        name="clientName"
                        className="form-control"
                        value={project.clientName}
                        onChange={handleInputChange}
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="location">Location</label>
                    <input
                        type="text"
                        id="location"
                        name="location"
                        className="form-control"
                        value={project.location}
                        onChange={handleInputChange}
                    />
                </div>

                <div className="form-row">
                    <div className="form-group">
                        <label htmlFor="latitude">Latitude</label>
                        <input
                            type="number"
                            step="0.000001"
                            id="latitude"
                            name="latitude"
                            className="form-control"
                            value={project.latitude}
                            onChange={handleInputChange}
                        />
                    </div>

                    <div className="form-group">
                        <label htmlFor="longitude">Longitude</label>
                        <input
                            type="number"
                            step="0.000001"
                            id="longitude"
                            name="longitude"
                            className="form-control"
                            value={project.longitude}
                            onChange={handleInputChange}
                        />
                    </div>
                </div>

                <div className="form-row">
                    <div className="form-group">
                        <label htmlFor="altitude">Altitude (m)</label>
                        <input
                            type="number"
                            id="altitude"
                            name="altitude"
                            className="form-control"
                            value={project.altitude}
                            onChange={handleInputChange}
                        />
                    </div>

                    <div className="form-group">
                        <label htmlFor="albedo">Albedo</label>
                        <input
                            type="number"
                            step="0.01"
                            min="0"
                            max="1"
                            id="albedo"
                            name="albedo"
                            className="form-control"
                            value={project.albedo}
                            onChange={handleInputChange}
                        />
                    </div>
                </div>

                <div className="form-group">
                    <label htmlFor="timeZone">Time Zone</label>
                    <input
                        type="text"
                        id="timeZone"
                        name="timeZone"
                        className="form-control"
                        value={project.timeZone}
                        onChange={handleInputChange}
                    />
                </div>

                <div className="form-group">
                    <label htmlFor="type">Project Type</label>
                    <select
                        id="type"
                        name="type"
                        className="form-control"
                        value={project.type}
                        onChange={handleInputChange}
                    >
                        <option value="Residential">Residential</option>
                        <option value="Commercial">Commercial</option>
                        <option value="Industrial">Industrial</option>
                        <option value="Utility">Utility</option>
                    </select>
                </div>

                <div className="form-actions">
                    <button type="button" onClick={() => navigate('/projects')} className="button secondary">
                        Cancel
                    </button>
                    <button type="submit" className="button">
                        {isNew ? 'Create Project' : 'Save Changes'}
                    </button>
                </div>
            </form>
        </div>
    );
};

export default ProjectForm;