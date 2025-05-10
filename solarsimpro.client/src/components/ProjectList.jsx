import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

const ProjectList = () => {
    const [projects, setProjects] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        fetchProjects();
    }, []);

    const fetchProjects = async () => {
        try {
            setLoading(true);
            const response = await fetch('/api/projects');
            if (response.ok) {
                const data = await response.json();
                setProjects(data);
            } else {
                setError('Failed to fetch projects. Please try again later.');
            }
        } catch (error) {
            console.error('Error fetching projects:', error);
            setError('An error occurred while fetching projects.');
        } finally {
            setLoading(false);
        }
    };

    if (loading) {
        return (
            <div className="loading-container">
                <div className="loading-spinner"></div>
                <p className="loading-text">Loading projects...</p>
            </div>
        );
    }

    return (
        <div className="section">
            <div className="section-header">
                <h2 className="section-title">Solar Projects</h2>
                <Link to="/projects/new" className="button">
                    Create New Project
                </Link>
            </div>

            {error && <div className="error-message">{error}</div>}

            {projects.length === 0 ? (
                <div className="empty-state">
                    <h3>No projects found</h3>
                    <p>Create your first solar project to get started.</p>
                    <Link to="/projects/new" className="button">Create New Project</Link>
                </div>
            ) : (
                <div className="project-grid">
                    {projects.map(project => (
                        <div key={project.id} className="project-card">
                            <div className="card-content">
                                <h3>{project.name}</h3>
                                <p>{project.location || 'No location specified'}</p>

                                <div className="project-meta">
                                    <div className="meta-item">
                                        <p className="meta-label">Type</p>
                                        <p className="meta-value">{project.type}</p>
                                    </div>
                                    <div className="meta-item">
                                        <p className="meta-label">Systems</p>
                                        <p className="meta-value">{project.systems?.length || 0}</p>
                                    </div>
                                </div>
                            </div>
                            <div className="card-actions">
                                <Link to={`/projects/${project.id}`} className="button">
                                    View Details
                                </Link>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
};

export default ProjectList;