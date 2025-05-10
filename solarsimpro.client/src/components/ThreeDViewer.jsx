import React, { useRef, useEffect } from 'react';
import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls';

const ThreeDViewer = ({ roofModel, panels }) => {
    const mountRef = useRef(null);

    useEffect(() => {
        if (!mountRef.current) return;

        // Set up Three.js scene
        const scene = new THREE.Scene();
        scene.background = new THREE.Color(0xf0f0f0);

        // Add ambient light
        const ambientLight = new THREE.AmbientLight(0xffffff, 0.5);
        scene.add(ambientLight);

        // Add directional light
        const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
        directionalLight.position.set(10, 20, 15);
        directionalLight.castShadow = true;
        scene.add(directionalLight);

        // Add camera
        const width = mountRef.current.clientWidth;
        const height = mountRef.current.clientHeight;
        const camera = new THREE.PerspectiveCamera(75, width / height, 0.1, 1000);
        camera.position.set(5, 5, 10);

        // Set up renderer
        const renderer = new THREE.WebGLRenderer({ antialias: true });
        renderer.setSize(width, height);
        renderer.shadowMap.enabled = true;

        // Store current mount node for cleanup
        const currentMount = mountRef.current;
        currentMount.appendChild(renderer.domElement);

        // Add orbit controls
        const controls = new OrbitControls(camera, renderer.domElement);
        controls.enableDamping = true;
        controls.dampingFactor = 0.05;

        // Add grid helper
        const gridHelper = new THREE.GridHelper(20, 20);
        scene.add(gridHelper);

        // Add a simple ground plane
        const groundGeometry = new THREE.PlaneGeometry(20, 20);
        const groundMaterial = new THREE.MeshStandardMaterial({
            color: 0x7ec850,
            roughness: 0.8,
            metalness: 0.2
        });
        const ground = new THREE.Mesh(groundGeometry, groundMaterial);
        ground.rotation.x = -Math.PI / 2;
        ground.receiveShadow = true;
        scene.add(ground);

        // Add a simple building if no roof model
        if (!roofModel) {
            const buildingGeometry = new THREE.BoxGeometry(8, 4, 6);
            const buildingMaterial = new THREE.MeshStandardMaterial({ color: 0xcccccc });
            const building = new THREE.Mesh(buildingGeometry, buildingMaterial);
            building.position.y = 2;
            building.castShadow = true;
            building.receiveShadow = true;
            scene.add(building);

            // Add a simple roof
            const roofGeometry = new THREE.ConeGeometry(6, 2, 4);
            const roofMaterial = new THREE.MeshStandardMaterial({ color: 0xa52a2a });
            const roof = new THREE.Mesh(roofGeometry, roofMaterial);
            roof.position.y = 5;
            roof.rotation.y = Math.PI / 4;
            roof.castShadow = true;
            scene.add(roof);
        }

        // Add solar panels if available
        if (panels && panels.length > 0) {
            panels.forEach(panel => {
                const panelGeometry = new THREE.BoxGeometry(1, 0.05, 1.6);
                const panelMaterial = new THREE.MeshStandardMaterial({
                    color: 0x1a75ff,
                    metalness: 0.8,
                    roughness: 0.2
                });
                const panelMesh = new THREE.Mesh(panelGeometry, panelMaterial);

                // Position and rotate based on panel data
                panelMesh.position.set(
                    panel.x || 0,
                    panel.y || 4.1,
                    panel.z || 0
                );

                // If we have tilt and azimuth, apply them
                if (panel.tilt !== undefined && panel.azimuth !== undefined) {
                    const tiltRad = THREE.MathUtils.degToRad(panel.tilt);
                    const azimuthRad = THREE.MathUtils.degToRad(panel.azimuth);
                    panelMesh.rotation.set(tiltRad, azimuthRad, 0);
                } else {
                    // Default to a 20-degree tilt for demo
                    panelMesh.rotation.x = THREE.MathUtils.degToRad(20);
                }

                panelMesh.castShadow = true;
                panelMesh.receiveShadow = true;
                scene.add(panelMesh);
            });
        } else if (roofModel === null && panels === null) {
            // Add demo panels on the roof
            for (let i = -2; i <= 2; i++) {
                for (let j = -1; j <= 1; j++) {
                    const panelGeometry = new THREE.BoxGeometry(1, 0.05, 1.6);
                    const panelMaterial = new THREE.MeshStandardMaterial({
                        color: 0x1a75ff,
                        metalness: 0.8,
                        roughness: 0.2
                    });
                    const panelMesh = new THREE.Mesh(panelGeometry, panelMaterial);

                    panelMesh.position.set(i * 1.2, 4.1, j * 1.8);
                    panelMesh.rotation.x = THREE.MathUtils.degToRad(20);

                    panelMesh.castShadow = true;
                    panelMesh.receiveShadow = true;
                    scene.add(panelMesh);
                }
            }
        }

        // Animation loop
        const animate = () => {
            requestAnimationFrame(animate);
            controls.update();
            renderer.render(scene, camera);
        };

        animate();

        // Handle window resize
        const handleResize = () => {
            const width = currentMount.clientWidth;
            const height = currentMount.clientHeight;

            camera.aspect = width / height;
            camera.updateProjectionMatrix();

            renderer.setSize(width, height);
        };

        window.addEventListener('resize', handleResize);

        // Cleanup
        return () => {
            window.removeEventListener('resize', handleResize);
            if (currentMount) {
                currentMount.removeChild(renderer.domElement);
            }
            renderer.dispose();
        };
    }, [roofModel, panels]);

    return (
        <div
            ref={mountRef}
            className="visualization-canvas"
            style={{ width: '100%', height: '500px' }}
        ></div>
    );
};

export default ThreeDViewer;