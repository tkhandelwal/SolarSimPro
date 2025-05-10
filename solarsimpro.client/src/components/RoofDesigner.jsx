// src/components/RoofDesigner.jsx
import React, { useState, useEffect, useRef } from 'react';
import { Stage, Layer, Rect, Line, Circle, Group } from 'react-konva';

const RoofDesigner = ({ projectId, initialRoofGeometry, onSave }) => {
    const [roofGeometry, setRoofGeometry] = useState(initialRoofGeometry || {
        width: 10,
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
                tilt: 30, // degrees
                azimuth: -89.6 // degrees
            }
        ]
    });

    const [selectedSectionId, setSelectedSectionId] = useState(null);
    const [selectedPointIndex, setSelectedPointIndex] = useState(null);
    const [scale, setScale] = useState(20); // pixels per meter
    const [isDragging, setIsDragging] = useState(false);
    const stageRef = useRef(null);

    // Load roof geometry from server if available
    useEffect(() => {
        const fetchRoofData = async () => {
            try {
                const response = await fetch(`/api/projects/${projectId}/roof-geometry`);
                if (response.ok) {
                    const data = await response.json();
                    // Only update if we have data and no initialRoofGeometry was provided
                    if (data && !initialRoofGeometry) {
                        setRoofGeometry(data);
                    }
                }
            } catch (error) {
                console.error('Error fetching roof geometry:', error);
            }
        };

        if (projectId) {
            fetchRoofData();
        }
    }, [projectId, initialRoofGeometry]);

    // Find the selected section if any
    const selectedSection = selectedSectionId !== null
        ? roofGeometry.sections.find(section => section.id === selectedSectionId)
        : null;

    // Handle section selection
    const handleSelectSection = (id) => {
        setSelectedSectionId(id);
        setSelectedPointIndex(null);
    };

    // Handle point selection
    const handleSelectPoint = (sectionId, pointIndex) => {
        setSelectedSectionId(sectionId);
        setSelectedPointIndex(pointIndex);
    };

    // Handle point drag
    const handlePointDrag = (sectionId, pointIndex, newPosition) => {
        setRoofGeometry({
            ...roofGeometry,
            sections: roofGeometry.sections.map(section => {
                if (section.id === sectionId) {
                    const newPoints = [...section.points];
                    newPoints[pointIndex] = newPosition;
                    return { ...section, points: newPoints };
                }
                return section;
            })
        });
    };

    // Add a new section
    const handleAddSection = () => {
        const newId = Math.max(...roofGeometry.sections.map(s => s.id), 0) + 1;

        setRoofGeometry({
            ...roofGeometry,
            sections: [
                ...roofGeometry.sections,
                {
                    id: newId,
                    points: [
                        { x: 2, y: 2 },
                        { x: 8, y: 2 },
                        { x: 8, y: 8 },
                        { x: 2, y: 8 }
                    ],
                    tilt: 30,
                    azimuth: 180
                }
            ]
        });

        setSelectedSectionId(newId);
    };

    // Remove the selected section
    const handleRemoveSection = () => {
        if (selectedSectionId === null) return;

        setRoofGeometry({
            ...roofGeometry,
            sections: roofGeometry.sections.filter(section => section.id !== selectedSectionId)
        });

        setSelectedSectionId(null);
        setSelectedPointIndex(null);
    };

    // Update section properties
    const handleUpdateSectionProperties = (e) => {
        if (selectedSectionId === null) return;

        const { name, value } = e.target;
        const numValue = parseFloat(value);

        setRoofGeometry({
            ...roofGeometry,
            sections: roofGeometry.sections.map(section => {
                if (section.id === selectedSectionId) {
                    return { ...section, [name]: isNaN(numValue) ? 0 : numValue };
                }
                return section;
            })
        });
    };

    // Handle save
    const handleSave = async () => {
        // Save to the server using the projectId
        if (projectId) {
            try {
                const response = await fetch(`/api/projects/${projectId}/roof-geometry`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(roofGeometry)
                });

                if (response.ok) {
                    console.log('Roof geometry saved successfully');
                } else {
                    console.error('Failed to save roof geometry');
                }
            } catch (error) {
                console.error('Error saving roof geometry:', error);
            }
        }

        // Also call the provided onSave callback if it exists
        if (onSave) {
            onSave(roofGeometry);
        }
    };

    // Handle zoom
    const handleWheel = (e) => {
        e.evt.preventDefault();

        const scaleBy = 1.1;
        const newScale = e.evt.deltaY < 0 ? scale * scaleBy : scale / scaleBy;

        setScale(Math.max(5, Math.min(50, newScale)));
    };

    // Add a point to the selected section
    const handleAddPoint = () => {
        if (selectedSectionId === null) return;

        const section = roofGeometry.sections.find(s => s.id === selectedSectionId);
        if (!section) return;

        // Find midpoint of last and first point to add a new point
        const lastPoint = section.points[section.points.length - 1];
        const firstPoint = section.points[0];
        const newPoint = {
            x: (lastPoint.x + firstPoint.x) / 2,
            y: (lastPoint.y + firstPoint.y) / 2
        };

        setRoofGeometry({
            ...roofGeometry,
            sections: roofGeometry.sections.map(s => {
                if (s.id === selectedSectionId) {
                    return {
                        ...s,
                        points: [...s.points, newPoint]
                    };
                }
                return s;
            })
        });
    };

    // Remove the selected point
    const handleRemovePoint = () => {
        if (selectedSectionId === null || selectedPointIndex === null) return;

        const section = roofGeometry.sections.find(s => s.id === selectedSectionId);
        if (!section || section.points.length <= 3) {
            // Don't allow removing if we have a triangle (minimum shape)
            return;
        }

        setRoofGeometry({
            ...roofGeometry,
            sections: roofGeometry.sections.map(s => {
                if (s.id === selectedSectionId) {
                    const newPoints = [...s.points];
                    newPoints.splice(selectedPointIndex, 1);
                    return { ...s, points: newPoints };
                }
                return s;
            })
        });

        setSelectedPointIndex(null);
    };

    return (
        <div className="roof-designer">
            <div className="design-controls">
                <h3>Roof Designer - Project {projectId}</h3>
                <button onClick={handleAddSection}>Add Section</button>
                <button
                    onClick={handleRemoveSection}
                    disabled={selectedSectionId === null}
                >
                    Remove Section
                </button>
                <button onClick={handleSave}>Save Roof</button>

                {selectedSection && (
                    <div className="section-properties">
                        <h4>Section Properties</h4>
                        <div className="property-group">
                            <label>
                                Tilt (degrees):
                                <input
                                    type="number"
                                    name="tilt"
                                    value={selectedSection.tilt}
                                    onChange={handleUpdateSectionProperties}
                                    min="0"
                                    max="90"
                                />
                            </label>
                        </div>
                        <div className="property-group">
                            <label>
                                Azimuth (degrees):
                                <input
                                    type="number"
                                    name="azimuth"
                                    value={selectedSection.azimuth}
                                    onChange={handleUpdateSectionProperties}
                                    min="-180"
                                    max="180"
                                />
                            </label>
                        </div>
                        <button
                            onClick={handleAddPoint}
                            disabled={selectedSectionId === null}
                        >
                            Add Point
                        </button>
                        <button
                            onClick={handleRemovePoint}
                            disabled={selectedPointIndex === null}
                        >
                            Remove Point
                        </button>
                    </div>
                )}
            </div>

            <div className="roof-canvas">
                <Stage
                    width={800}
                    height={600}
                    ref={stageRef}
                    onWheel={handleWheel}
                    draggable
                    onDragStart={() => setIsDragging(true)}
                    onDragEnd={() => setIsDragging(false)}
                >
                    <Layer>
                        {/* Grid lines for reference */}
                        {Array.from({ length: 21 }).map((_, i) => (
                            <Line
                                key={`grid-h-${i}`}
                                points={[0, i * scale, 20 * scale, i * scale]}
                                stroke="#ddd"
                                strokeWidth={1}
                            />
                        ))}
                        {Array.from({ length: 21 }).map((_, i) => (
                            <Line
                                key={`grid-v-${i}`}
                                points={[i * scale, 0, i * scale, 20 * scale]}
                                stroke="#ddd"
                                strokeWidth={1}
                            />
                        ))}

                        {/* Render roof sections */}
                        {roofGeometry.sections.map((section) => {
                            // Convert points to flat array for Konva
                            const flatPoints = section.points.flatMap(p => [p.x * scale, p.y * scale]);

                            const isSelected = section.id === selectedSectionId;

                            return (
                                <Group key={section.id}>
                                    {/* Render the section polygon */}
                                    <Line
                                        points={flatPoints}
                                        closed
                                        fill={isSelected ? "#b3d9ff" : "#e6f2ff"}
                                        stroke={isSelected ? "#0066cc" : "#99ccff"}
                                        strokeWidth={2}
                                        onClick={() => handleSelectSection(section.id)}
                                    />

                                    {/* Render control points when section is selected */}
                                    {isSelected && section.points.map((point, index) => (
                                        <Circle
                                            key={index}
                                            x={point.x * scale}
                                            y={point.y * scale}
                                            radius={6}
                                            fill={selectedPointIndex === index ? "#ff6600" : "#0066cc"}
                                            stroke="#ffffff"
                                            strokeWidth={2}
                                            draggable
                                            onClick={(e) => {
                                                e.cancelBubble = true;
                                                handleSelectPoint(section.id, index);
                                            }}
                                            onDragMove={(e) => {
                                                if (isDragging) return;

                                                handlePointDrag(section.id, index, {
                                                    x: e.target.x() / scale,
                                                    y: e.target.y() / scale,
                                                });
                                            }}
                                        />
                                    ))}
                                </Group>
                            );
                        })}
                    </Layer>
                </Stage>
            </div>

            <div className="roof-info">
                <h4>Roof Information</h4>
                <p>Project ID: {projectId}</p>
                <p>Width: {roofGeometry.width} meters</p>
                <p>Length: {roofGeometry.length} meters</p>
                <p>Sections: {roofGeometry.sections.length}</p>
                <p>Scale: {scale} pixels/meter</p>
            </div>
        </div>
    );
};

export default RoofDesigner;