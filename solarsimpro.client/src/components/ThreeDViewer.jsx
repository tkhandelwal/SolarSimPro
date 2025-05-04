// src/components/ThreeDViewer.jsx
import React, { useRef, useEffect } from 'react';
import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls';

const ThreeDViewer = ({ roofModel, panels }) => {
    const mountRef = useRef(null);

    useEffect(() => {
        // Set up Three.js scene
        const scene = new THREE.Scene();
        const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
        const renderer = new THREE.WebGLRenderer();

        renderer.setSize(window.innerWidth, window.innerHeight);
        mountRef.current.appendChild(renderer.domElement);

        // Add roof model
        if (roofModel) {
            // Logic to load and display 3D roof model
        }

        // Add solar panels
        if (panels) {
            panels.forEach(panel => {
                const panelGeometry = new THREE.BoxGeometry(panel.width, panel.length, 0.04);
                const panelMaterial = new THREE.MeshBasicMaterial({ color: 0x1a75ff });
                const panelMesh = new THREE.Mesh(panelGeometry, panelMaterial);

                panelMesh.position.set(panel.x, panel.y, panel.z);
                panelMesh.rotation.set(
                    THREE.MathUtils.degToRad(panel.tilt),
                    THREE.MathUtils.degToRad(panel.azimuth),
                    0
                );

                scene.add(panelMesh);
            });
        }

        // Set up orbit controls for navigation
        const controls = new OrbitControls(camera, renderer.domElement);
        camera.position.z = 5;

        // Animation loop
        const animate = () => {
            requestAnimationFrame(animate);
            controls.update();
            renderer.render(scene, camera);
        };

        animate();

        // Cleanup
        return () => {
            mountRef.current.removeChild(renderer.domElement);
        };
    }, [roofModel, panels]);

    return <div ref={mountRef} style={{ width: '100%', height: '500px' }}></div>;
};