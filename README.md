# B581 Final Project                        Hardik Mahesh Kotangale                                           hkotanga

## 1. Unity Environment
- **Version:** Unity 2022.3.36f1 LTS  
- **Project Name:** Crystal-Cavern-Exploration

---

## 2. C# Files Included in this Problem Set

### **Task A: 3D Scene - Models and Surfaces**
- **CrystalClusterGenerator.cs**  
  Generates a procedurally defined crystal cluster, creating algorithmically calculated prismatic structures with variable heights and orientations. Includes vertex normals and material application.

- **TorusKnotGenerator.cs**  
  Generates a mathematically defined torus knot using trigonometric functions for vertex calculations. Includes vertex normals for shading and interaction logic for proximity detection with a dynamic light source.

---

### **Task B: Interaction - User and Object-Object Interaction**
- **DragObject.cs**  
  Implements user-controlled transformations using mouse input. Features include:
  - Translation on XY and XZ planes.
  - Rotation using the Rolling Ball Algorithm.
  - Dynamic scaling with constraints to avoid distortion.

- **CrystalInteraction.cs**  
  Handles proximity-based interactions between a glowing light source and crystal clusters. Alters crystal colors and glow intensity based on distance.

- **TorusInteraction.cs**  
  Manages proximity-based interaction for the torus knot, dynamically blending colors when close to a light source.

- **ColorSwatch.cs**  
  Provides a UI-based color selection system for the glowing sphere and interactive materials.
  Features include:
  - Color swatches for red, green, blue, yellow, and white.
  - Dynamic updates to the glowing sphere, cavern floor, and torus materials in real-time.
  - Debug logging for selected colors.
---

### **Task C: Animation**
- **GlowingSphereMover.cs**  
  Animates a glowing sphere along a predefined path with oscillating height. Includes functionality for automatic and manual control:
  - Automatic circular animation with sinusoidal height variation.
  - Manual movement using WASD keys for exploration.
  - Dynamic camera adjustment for default and ball-following views.

- **GlowingSphereLightManager.cs**  
  Updates shaders with the light source's dynamic position, affecting illumination and material response for all interactive objects in the scene.

---

### **Task D: Illumination - Lighting and Materials**
- **LightingShader.shader**  
  Implements a custom lighting equation with ambient, diffuse, and specular components. Computations are split between the vertex and fragment shaders:
  - **Vertex Shader:** Calculates light direction and normal transformations.
  - **Fragment Shader:** Computes ambient, diffuse, and specular contributions for realistic illumination.

- **RockyGroundShader.shader**  
  Simulates a bumpy cavern floor using a GPU-based noise function for displacement mapping.

- **CrystalGlowShader.shader**  
  Implements an emissive effect for the crystals, with a pulsating glow based on proximity to the light source.

- **TorusShader.shader**  
  Provides a custom material for the torus knot with dynamic color blending based on light interactions.

---

## 3. Key Implementations

### **3D Scene: Models and Surfaces**
- **Crystal Cluster:** Procedurally generated prisms with variable heights, random rotations, and dynamic material assignment.  
- **Torus Knot:** Algorithmically defined using trigonometric functions, showcasing advanced geometry modeling.  
- **Cavern Floor:** Non-flat surface generated using a noise function in the GPU for realistic terrain.

---

### **Animation**
- **Glowing Sphere Animation:** Moves along a circular path with adjustable height using sinusoidal functions.  
- **Crystal Pulsating Glow:** Sinusoidal animation of crystal glow intensity controlled by the fragment shader.  
- **Torus Knot Rotation:** Slow, constant rotation around its axis implemented in C#.

---

### **Interaction**
- **Crystal Rotation:** Interactive rotation using the Rolling Ball Algorithm, with adjustable modes for scaling and translation.  
 
- **Proximity Effects:** Color blending and glow intensity adjustments based on the distance between objects.

---

### **Illumination**
- **Lighting Model:** Combines ambient, diffuse, and specular components computed in custom shaders.  
- **Point Light Animation:** A glowing sphere serves as the light source, illuminating surrounding objects dynamically.  


---

### **Mapping**
- **Bump Mapping:** Applies a noise-based texture to the cavern floor for added realism.  

---
### **Help Screen**
- Pressing the **'H' key** on the keyboard toggles a HUD overlay that displays:
  - **Mouse Controls:** For rotating objects.
  - **Keyboard Controls:** For camera views and sphere animation modes.
  - **Light Controls:** Toggle light ON/OFF and adjust ambient, diffuse, and specular parameters.

### **Completion Panel**
- A **completion panel** is triggered when the glowing sphere interacts with the target object within a predefined radius. It displays a message.

---