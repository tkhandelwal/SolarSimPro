// src/components/RoofDesigner.jsx
import React, { useRef, useEffect, useState } from 'react';
import { Stage, Layer, Rect, Line, Group, Text } from 'react-konva';
import useImage from 'use-image';

const RoofDesigner = ({ projectId, initialRoofGeometry, onSave }) => {
    const [roofGeometry, setRoofGeometry] = useState(initialRoofGeometry || {
        width: 10, // meters
        length: 15, // meters
        sections: [
            {
                id: 1,
                points: [
                    { x: 0, y: 0 },
                    { x: 10, y: 0 },
                    { x: 10, y: 15 },
                    { x: 0, y: 15 }
                ],
                tilt: 3, // degrees
                azimuth: -89.6 // degrees
            }
        ]
    });

    const [panels, setPanels] = useState([]);
    const [selectedPanelModel, setSelectedPanelModel] = useState(null);
    const [panelModels, setPanelModels] = useState([]);
    const [mapImage, setMapImage] = useState(null);
    const [scale, setScale] = useState(20); // pixels per meter
    const [tool, setTool] = useState('select'); // select, draw, placePanels

    // Load panel models from API
    useEffect(() => {
        fetch('/api/panels')
            .then(res => res.json())
            .then(data => {
                setPanelModels(data);
                setSelectedPanelModel(data[0]); // Select first panel by default
            });

        // Load existing panels if any
        if (projectId) {
            fetch(`/api/projects/${projectId}/panels`)
                .then(res => res.json())
                .then(data => setPanels(data));
        }

        // Load satellite imagery for the location
        if (projectId) {
            fetch(`/api/projects/${projectId}/map-image`)
                .then(res => res.json())
                .then(data => setMapImage(data.imageUrl));
        }
    }, [projectId]);

    // Function to generate optimal panel layout
    const generateOptimalLayout = () => {
        if (!selectedPanelModel) return;

        fetch(`/api/projects/${projectId}/generate-layout`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                roofGeometry,
                panelModelId: selectedPanelModel.id
            })
        })
            .then(res => res.json())
            .then(data => setPanels(data));
    };

    // Function to manually place a panel
    const addPanel = (x, y, sectionId) => {
        if (!selectedPanelModel) return;

        const section = roofGeometry.sections.find(s => s.id === sectionId);
        const newPanel = {
            id: Date.now(), // Temporary ID
            x,
            y,
            width: selectedPanelModel.width,
            height: selectedPanelModel.height,
            tilt: section.tilt,
            azimuth: section.azimuth,
            panelModelId: selectedPanelModel.id
        };

        setPanels([...panels, newPanel]);
    };

    // Calculate system capacity
    const calculateCapacity = () => {
        if (!selectedPanelModel) return 0;
        return (panels.length * selectedPanelModel.nominalPowerWp / 1000).toFixed(2);
    };

    // Save the layout
    const saveLayout = () => {
        fetch(`/api/projects/${projectId}/panels`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(panels)
        })
            .then(res => res.json())
            .then(data => {
                onSave && onSave(data);
                alert('Panel layout saved successfully!');
            });
    };

    return (
        <div className="roof-designer">
            <div className="toolbar">
                <button onClick={() => setTool('select')}>Select</button>
                <button onClick={() => setTool('draw')}>Draw Roof</button>
                <button onClick={() => setTool('placePanels')}>Place Panels</button>
                <button onClick={generateOptimalLayout}>Generate Optimal Layout</button>
                <select
                    value={selectedPanelModel?.id || ''}
                    onChange={(e) => {
                        const selected = panelModels.find(p => p.id === e.target.value);
                        setSelectedPanelModel(selected);
                    }}
                >
                    {panelModels.map(panel => (
                        <option key={panel.id} value={panel.id}>
                            {panel.manufacturer} {panel.modelName} ({panel.nominalPowerWp}W)
                        </option>
                    ))}
                </select>
                <div className="system-info">
                    Panels: {panels.length} | System Size: {calculateCapacity()} kWp
                </div>
                <button onClick={saveLayout} className="save-btn">Save Layout</button>
            </div>

            <div className="canvas-container">
                <Stage width={800} height={600}>
                    {/* Background map layer */}
                    {mapImage && (
                        <Layer>
                            <Image src={mapImage} />
                        </Layer>
                    )}

                    {/* Roof sections layer */}
                    <Layer>
                        {roofGeometry.sections.map(section => (
                            <Group key={section.id}>
                                <Line
                                    points={section.points.flatMap(p => [p.x * scale, p.y * scale])}
                                    closed
                                    fill="#cccccc"
                                    stroke="#333333"
                                    strokeWidth={2}
                                />
                                <Text
                                    text={`Tilt: ${section.tilt}°, Azimuth: ${section.azimuth}°`}
                                    x={section.points[0].x * scale}
                                    y={section.points[0].y * scale}
                                    fontSize={14}
                                />
                            </Group>
                        ))}
                    </Layer>

                    {/* Panels layer */}
                    <Layer>
                        {panels.map(panel => (
                            <Rect
                                key={panel.id}
                                x={panel.x * scale}
                                y={panel.y * scale}
                                width={panel.width * scale}
                                height={panel.height * scale}
                                fill="#3388ff"
                                stroke="#0044cc"
                                strokeWidth={1}
                                draggable={tool === 'select'}
                                onDragEnd={(e) => {
                                    // Update panel position when dragged
                                    const updatedPanels = panels.map(p =>
                                        p.id === panel.id
                                            ? { ...p, x: e.target.x() / scale, y: e.target.y() / scale }
                                            : p
                                    );
                                    setPanels(updatedPanels);
                                }}
                            />
                        ))}
                    </Layer>
                </Stage>
            </div>
        </div>
    );
};

export default RoofDesigner;