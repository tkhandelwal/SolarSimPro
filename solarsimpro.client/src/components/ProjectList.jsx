// src/components/ProjectList.jsx
import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

const ProjectList = () => {
    const [projects, setProjects] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchProjects();
    }, []);

    const fetchProjects = async () => {
        try {
            const response = await fetch('/api/projects');
            if (response.ok) {
                const data = await response.json();
                setProjects(data);
            } else {
                console.error('Failed to fetch projects');
            }
        } catch (error) {
            console.error('Error fetching projects:', error);
        } finally {
            setLoading(false);
        }
    };

    if (loading) {
        return <div>Loading projects...</div>;
    }

    return (
        <div className="project-list">
            <h2>Solar Projects</h2>

            <Link to="/projects/new" className="button create-button">
                Create New Project
            </Link>

            {projects.length === 0 ? (
                <p>No projects found. Create your first solar project!</p>
            ) : (
                <div className="project-grid">
                    {projects.map(project => (
                        <div key={project.id} className="project-card">
                            <h3>{project.name}</h3>
                            <p>{project.location}</p>
                            <p>Type: {project.type}</p>
                            <p>Systems: {project.systems.length}</p>
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