import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import './App.css';
import ProjectList from './components/ProjectList';
import ProjectForm from './components/ProjectForm';
import ProjectDashboard from './components/ProjectDashboard';
import SolarSystemForm from './components/SolarSystemForm'; // Add this import

function App() {
    return (
        <Router>
            <div className="app-container">
                <header className="app-header">
                    <div className="logo">
                        <span>SolarSimPro</span>
                    </div>
                    <nav className="main-nav">
                        <Link to="/">Home</Link>
                        <Link to="/projects">Projects</Link>
                    </nav>
                </header>

                <main className="container">
                    <Routes>
                        <Route path="/" element={
                            <div className="home-page">
                                <h1>Welcome to SolarSimPro</h1>
                                <p>Advanced solar design and simulation software for professionals</p>
                                <div className="button-group">
                                    <Link to="/projects" className="button">View Projects</Link>
                                    <Link to="/projects/new" className="button secondary">Create New Project</Link>
                                </div>
                            </div>
                        } />
                        <Route path="/projects" element={<ProjectList />} />
                        <Route path="/projects/new" element={<ProjectForm />} />
                        <Route path="/projects/:id" element={<ProjectDashboard />} />
                        <Route path="/projects/edit/:id" element={<ProjectForm />} />
                        <Route path="/projects/:projectId/systems/new" element={<SolarSystemForm />} />

                    </Routes>
                </main>

                <footer>
                    <div className="copyright">
                        © 2025 SolarSimPro - Solar Design and Simulation
                    </div>
                </footer>
            </div>
        </Router>
    );
}

export default App;