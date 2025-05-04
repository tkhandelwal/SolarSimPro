// src/components/ThreeDVisualization.jsx
import React, { useEffect, useRef } from 'react';
import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader';

const ThreeDVisualization = ({ projectId, solarSystem, showShadows = true }) => {
    const containerRef = useRef(null);
    const sceneRef = useRef(null);
    const cameraRef = useRef(null);
    const rendererRef = useRef(null);
    const controlsRef = useRef(null);

    // Setup the 3D scene
    useEffect(() => {
        if (!containerRef.current) return;

        // Initialize scene
        const scene = new THREE.Scene();
        sceneRef.current = scene;
        scene.background = new THREE.Color(0x87ceeb); // Sky blue background

        // Initialize camera
        const camera = new THREE.PerspectiveCamera(
            75,
            containerRef.current.clientWidth / containerRef.current.clientHeight,
            0.1,
            1000
        );
        cameraRef.current = camera;
        camera.position.set(20, 20, 20);
        camera.lookAt(0, 0, 0);

        // Initialize renderer
        const renderer = new THREE.WebGLRenderer({ antialias: true });
        rendererRef.current = renderer;
        renderer.setSize(containerRef.current.clientWidth, containerRef.current.clientHeight);
        renderer.shadowMap.enabled = showShadows;
        renderer.shadowMap.type = THREE.PCFSoftShadowMap;
        containerRef.current.appendChild(renderer.domElement);

        // Add lighting
        const ambientLight = new THREE.AmbientLight(0xffffff, 0.5);
        scene.add(ambientLight);

        const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
        directionalLight.position.set(50, 50, 0);
        directionalLight.castShadow = showShadows;
        scene.add(directionalLight);

        if (showShadows) {
            directionalLight.shadow.mapSize.width = 2048;
            directionalLight.shadow.mapSize.height = 2048;
            directionalLight.shadow.camera.near = 0.5;
            directionalLight.shadow.camera.far = 500;
            directionalLight.shadow.camera.left = -100;
            directionalLight.shadow.camera.right = 100;
            directionalLight.shadow.camera.top = 100;
            directionalLight.shadow.camera.bottom = -100;
        }

        // Add orbit controls
        const controls = new OrbitControls(camera, renderer.domElement);
        controlsRef.current = controls;
        controls.enableDamping = true;
        controls.dampingFactor = 0.05;

        // Add grid helper
        const gridHelper = new THREE.GridHelper(100, 100);
        scene.add(gridHelper);

        // Animation loop
        const animate = () => {
            requestAnimationFrame(animate);
            controls.update();
            renderer.render(scene, camera);
        };

        animate();

        // Handle window resize
        const handleResize = () => {
            if (!containerRef.current) return;

            const width = containerRef.current.clientWidth;
            const height = containerRef.current.clientHeight;

            camera.aspect = width / height;
            camera.updateProjectionMatrix();

            renderer.setSize(width, height);
        };

        window.addEventListener('resize', handleResize);

        // Cleanup
        return () => {
            window.removeEventListener('resize', handleResize);
            containerRef.current?.removeChild(renderer.domElement);
            renderer.dispose();
        };
    }, [showShadows]);

    // Load terrain and buildings
    useEffect(() => {
        if (!sceneRef.current || !projectId) return;

        // Load terrain model
        fetch(`/api/projects/${projectId}/terrain-model`)
            .then(res => res.json())
            .then(data => {
                const loader = new GLTFLoader();
                loader.load(data.modelUrl, (gltf) => {
                    const terrain = gltf.scene;
                    terrain.scale.set(1, 1, 1);
                    terrain.position.set(0, 0, 0);
                    terrain.traverse(child => {
                        if (child.isMesh) {
                            child.castShadow = showShadows;
                            child.receiveShadow = showShadows;
                        }
                    });
                    sceneRef.current.add(terrain);
                });
            });

        // Load building models
        fetch(`/api/projects/${projectId}/building-model`)
            .then(res => res.json())
            .then(data => {
                const loader = new GLTFLoader();
                loader.load(data.modelUrl, (gltf) => {
                    const building = gltf.scene;
                    building.scale.set(1, 1, 1);
                    building.position.set(0, 0, 0);
                    building.traverse(child => {
                        if (child.isMesh) {
                            child.castShadow = showShadows;
                            child.receiveShadow = showShadows;
                        }
                    });
                    sceneRef.current.add(building);
                });
            });
    }, [projectId, showShadows]);

    // Add solar panels
    useEffect(() => {
        if (!sceneRef.current || !solarSystem) return;

        // Remove existing panels
        const existingPanels = sceneRef.current.children.filter(
            child => child.userData.isPanelObject
        );
        existingPanels.forEach(panel => sceneRef.current.remove(panel));

        // Load panel model
        const panelWidth = 1.7; // meters
        const panelHeight = 1.0; // meters
        const panelThickness = 0.04; // meters

        fetch(`/api/projects/${projectId}/panels`)
            .then(res => res.json())
            .then(panels => {
                // Create panel geometry
                const panelGeometry = new THREE.BoxGeometry(
                    panelWidth,
                    panelHeight,
                    panelThickness
                );

                const panelMaterial = new THREE.MeshPhongMaterial({
                    color: 0x2244aa,
                    shininess: 100
                });

                // Add each panel to the scene
                panels.forEach(panel => {
                    const panelMesh = new THREE.Mesh(panelGeometry, panelMaterial);
                    panelMesh.position.set(panel.x, panel.z + panelThickness / 2, panel.y);

                    // Apply panel orientation
                    panelMesh.rotation.x = THREE.MathUtils.degToRad(panel.tilt);
                    panelMesh.rotation.z = THREE.MathUtils.degToRad(panel.azimuth);

                    panelMesh.castShadow = showShadows;
                    panelMesh.receiveShadow = showShadows;
                    panelMesh.userData.isPanelObject = true;
                    panelMesh.userData.panelId = panel.id;

                    sceneRef.current.add(panelMesh);
                });
            });
    }, [projectId, solarSystem, showShadows]);

    // Calculate and visualize shadows for specific date/time
    const calculateShadows = (date, time) => {
        if (!sceneRef.current || !directionalLightRef.current) return;

        // Calculate sun position based on date, time, and project location
        fetch(`/api/projects/${projectId}/sun-position?date=${date}&time=${time}`)
            .then(res => res.json())
            .then(sunPosition => {
                // Update directional light position to match sun position
                directionalLightRef.current.position.set(
                    sunPosition.x,
                    sunPosition.y,
                    sunPosition.z
                );
            });
    };

    return (
        <div className="visualization-container">
            <div
                ref={containerRef}
                className="visualization-canvas"
                style={{ width: '100%', height: '600px' }}
            />

            {showShadows && (
                <div className="shadow-controls">
                    <h3>Shadow Analysis</h3>
                    <div className="time-controls">
                        <input
                            type="date"
                            onChange={(e) => calculateShadows(e.target.value, timeRef.current.value)}
                            ref={dateRef}
                        />
                        <input
                            type="time"
                            onChange={(e) => calculateShadows(dateRef.current.value, e.target.value)}
                            ref={timeRef}
                        />
                        <button
                            onClick={() => calculateShadows(dateRef.current.value, timeRef.current.value)}
                        >
                            Update Shadows
                        </button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default ThreeDVisualization;