import { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import ThreeDViewer from './ThreeDViewer';

const ProjectDashboard = () => {
    const { id } = useParams();
    const [project, setProject] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [activeTab, setActiveTab] = useState('overview');

    useEffect(() => {
        if (!id) {
            setError('Invalid project ID');
            setLoading(false);
            return;
        }

        const fetchProject = async () => {
            try {
                const response = await fetch(`/api/projects/${id}`);
                if (response.ok) {
                    const data = await response.json();
                    setProject(data);
                } else {
                    setError('Failed to fetch project details. Please try again later.');
                }
            } catch (error) {
                console.error('Error fetching project:', error);
                setError('An error occurred while fetching the project details.');
            } finally {
                setLoading(false);
            }
        };

        fetchProject();
    }, [id]);

    if (loading) {
        return (
            <div className="loading-container">
                <div className="loading-spinner"></div>
                <p className="loading-text">Loading project...</p>
            </div>
        );
    }

    if (error) {
        return (
            <div className="section">
                <div className="error-message">{error}</div>
                <Link to="/projects" className="button">Back to Projects</Link>
            </div>
        );
    }

    if (!project) {
        return (
            <div className="section">
                <div className="error-message">Project not found</div>
                <Link to="/projects" className="button">Back to Projects</Link>
            </div>
        );
    }

    return (
        <div className="section">
            <div className="dashboard">
                <div className="dashboard-header">
                    <h2 className="dashboard-title">{project.name}</h2>
                    <div className="dashboard-actions">
                        <Link to={`/projects/edit/${id}`} className="button">Edit Project</Link>
                    </div>
                </div>

                <div className="dashboard-tabs">
                    <button
                        className={activeTab === 'overview' ? 'active' : ''}
                        onClick={() => setActiveTab('overview')}
                    >
                        Overview
                    </button>
                    <button
                        className={activeTab === 'systems' ? 'active' : ''}
                        onClick={() => setActiveTab('systems')}
                    >
                        Solar Systems
                    </button>
                    <button
                        className={activeTab === 'simulation' ? 'active' : ''}
                        onClick={() => setActiveTab('simulation')}
                    >
                        Simulation
                    </button>
                    <button
                        className={activeTab === 'visualization' ? 'active' : ''}
                        onClick={() => setActiveTab('visualization')}
                    >
                        3D Visualization
                    </button>
                    <button
                        className={activeTab === 'reports' ? 'active' : ''}
                        onClick={() => setActiveTab('reports')}
                    >
                        Reports
                    </button>
                </div>

                <div className="dashboard-content">
                    {activeTab === 'overview' && (
                        <div>
                            <h3>Project Overview</h3>
                            <div className="info-grid">
                                <div className="info-card">
                                    <p className="info-title">Location</p>
                                    <p className="info-value">{project.location || 'Not specified'}</p>
                                </div>
                                <div className="info-card">
                                    <p className="info-title">Type</p>
                                    <p className="info-value">{project.type}</p>
                                </div>
                                <div className="info-card">
                                    <p className="info-title">Client</p>
                                    <p className="info-value">{project.clientName || 'Not specified'}</p>
                                </div>
                                <div className="info-card">
                                    <p className="info-title">Created</p>
                                    <p className="info-value">{new Date(project.createdAt).toLocaleDateString()}</p>
                                </div>
                            </div>

                            <div className="card mt-4">
                                <div className="card-header">
                                    <h4 className="card-title">Project Details</h4>
                                </div>
                                <div className="card-body">
                                    <div className="info-item">
                                        <div className="label">Coordinates</div>
                                        <div className="value">{project.latitude}°, {project.longitude}°</div>
                                    </div>
                                    <div className="info-item">
                                        <div className="label">Altitude</div>
                                        <div className="value">{project.altitude} m</div>
                                    </div>
                                    <div className="info-item">
                                        <div className="label">Time Zone</div>
                                        <div className="value">{project.timeZone || 'Not specified'}</div>
                                    </div>
                                    <div className="info-item">
                                        <div className="label">Albedo</div>
                                        <div className="value">{project.albedo}</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    )}

                    // In the Solar Systems tab section of ProjectDashboard.jsx
                    {activeTab === 'systems' && (
                        <div>
                            <div className="section-header">
                                <h3 className="section-title">Solar Systems</h3>
                                <Link to={`/projects/${id}/systems/new`} className="button">
                                    Add Solar System
                                </Link>
                            </div>

                            {project.systems && project.systems.length > 0 ? (
                                <div className="systems-list">
                                    {project.systems.map(system => (
                                        <div key={system.id} className="system-card">
                                            <div className="card-header">
                                                <h4>{system.name}</h4>
                                            </div>
                                            <div className="card-body">
                                                <p>Capacity: {system.totalCapacityKWp.toFixed(2)} kWp</p>
                                                <p>Modules: {system.numberOfModules}</p>
                                                <p>Orientation: {system.tilt}° tilt, {system.azimuth}° azimuth</p>
                                            </div>
                                            <div className="card-actions">
                                                <button className="button small">Details</button>
                                                <button className="button small secondary">Edit</button>
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            ) : (
                                <div className="empty-state">
                                    <p>No solar systems</p>
                                    <p>Add your first system to start designing</p>
                                    <Link to={`/projects/${id}/systems/new`} className="button">
                                        Add Solar System
                                    </Link>
                                </div>
                            )}
                        </div>
                    )}

                    {activeTab === 'simulation' && (
                        <div>
                            <div className="section-header">
                                <h3>Simulation</h3>
                            </div>

                            <div className="empty-state">
                                <h3>No simulations</h3>
                                <p>Select a solar system and run a simulation to see results</p>
                                <button className="button" disabled={!project.systems || project.systems.length === 0}>
                                    Run Simulation
                                </button>
                            </div>
                        </div>
                    )}

                    {activeTab === 'visualization' && (
                        <div>
                            <div className="section-header">
                                <h3>3D Visualization</h3>
                            </div>

                            <div className="visualization-container">
                                <ThreeDViewer
                                    roofModel={null}
                                    panels={project.systems && project.systems.length > 0 ? [] : null}
                                />
                            </div>
                        </div>
                    )}

                    {activeTab === 'reports' && (
                        <div>
                            <div className="section-header">
                                <h3>Reports</h3>
                            </div>

                            <div className="empty-state">
                                <h3>No reports available</h3>
                                <p>Run a simulation first to generate reports</p>
                                <button className="button" disabled>Generate Report</button>
                            </div>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default ProjectDashboard;