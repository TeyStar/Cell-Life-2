# Tank Simulation Project

## Overview

This project simulates a tank environment where different cells interact with each other. Each cell belongs to a team and has various body parts that define its behavior and abilities. The simulation includes features such as cell movement, feeding, reproduction, and evolution.

## Table of Contents

1. [Installation](#installation)
2. [Usage](#usage)
3. [Classes and Components](#classes-and-components)
    - [Tank](#tank)
    - [Cell](#cell)
    - [BodyPart](#bodypart)
    - [DNABlueprint](#dnablueprint)
    - [WaterFlow](#waterflow)
4. [Body Parts](#body-parts)
5. [Controls](#controls)
6. [Customization](#customization)
7. [License](#license)

## Installation

1. Clone the repository to your local machine.
    
2. Open the project in Unity:
    - Open Unity Hub.
    - Click on "Add" and select the cloned project folder.
    - Open the project.

3. Ensure you have the required dependencies:
    - Unity 2020.3 or later.
    - .NET Framework 4.7.1.

## Usage

1. Open the `SampleScene` scene in the `Assets/Scenes` folder.
2. Press the play button in the Unity editor to start the simulation.
3. Observe the behavior of the cells as they move, feed, and interact with each other.

## Classes and Components

### Tank

The `Tank` class is the main controller of the simulation. It initializes the environment, spawns cells, and manages the update loop.

#### Key Methods:
- `Start()`: Initializes the simulation, sets the target frame rate, and spawns cells.
- `BuildDNA()`: Builds the DNA blueprints for each team.
- `SetTeam(GameObject gameObject)`: Assigns a team to a cell.
- `Update()`: Handles user input for controlling the simulation speed and camera.
- `UpdateCoroutine()`: Main update loop for the simulation.
- `LagFix()`: Removes excess cells to prevent lag.
- `CheckTeams()`: Checks the living status of each team and respawns cells if necessary.

### Cell

The `Cell` class represents an individual cell in the simulation. Each cell has various properties and behaviors defined by its DNA and body parts.

#### Key Properties:
- `bodyParts`: Array of body parts attached to the cell.
- `isAlive`: Indicates if the cell is alive.
- `team`: The team the cell belongs to.
- `isFed`: Indicates if the cell is fed.
- `lifeSpan`: The lifespan of the cell.
- `deathSpan`: The death span of the cell.

#### Key Methods:
- `Wiggle()`: Causes the cell to wiggle.
- `BodyPartBehavior(int i)`: Executes the behavior of a specific body part.
- `SetTeam(string s)`: Sets the team color of the cell.
- `Kill()`: Kills the cell.

### BodyPart

The `BodyPart` class represents a body part attached to a cell. Each cell can have different body parts that define its abilities and behaviors. 
Here are the descriptions of each body part:

- **Nothing**: No special ability.
- **Flagella**: Provides consistent movement by generating force in a specific direction.
- **Cilia**: Provides rapid small bursts of movement in a specific direction.
- **Jet**: Provides massive slow bursts movement in a specific direction.
- **Glue**: Allows the cell to attach to other cells to either drag with them, be dragged by them, or keep predatory cells at "arms" length away.
- **Eye**: Allows the cell to detect other cells from far away but has a small field of view.  It can spot prey when hungry or look for a mate, then turn its body so that its movement parts will push it towards them.
- **Stalk**: Allows the cell to see all predators around it so that it can turn and run.  It also detects and controls its balance, preventing being spun out of control.
- **Photosynthesizer**: Allows the cell to generate energy and stay young if at the top of the tank via Photosynthesis.
- **Chemosynthesizer**: Allows the cell to generate energy and stay young if at the bottom of the tank via Chemosynthesis.  Although, it is slower than Photosynthesis.
- **Jaw**: Allows the cell to kill and consume other living cells.  After eating a cell, they will leave its remains behind.  They cannot eat dead cells.
- **Filter**: Allows the cell to filter and consume dead cells.
- **Proboscis**: Allows the cell to extract nutrients from other cells if the Proboscis is undisturbed long enough while feeding.  It is also careful enough not to ingest poison or anything infectious.
- **Spike**: Provides a defensive mechanism by killing other cells on contact.  They break off when used.  Cells born with spikes are often smaller than their parents.
- **Shell**: Provides protection by knocking away cells and sending them into a violent spin.  Cells born with shells are often larger than their parents.
- **Poison**: Allows the cell to release poison blobs that can harm and slow other cells.  If a cell with poison is eaten, it will poison the other cell, reducing its reproduction capabilities.
- **Electric**: Allows the cell to release electric shock that reflect around until hitting another cell.  If it touches a mate, it will magnetically pull both into each other.  If it detects something else, it will electricute the cell making that cell's body parts sometimes not work.  This stacks and can make a cell unable to act.
- **Egg**: Allows the cell to carry more eggs to produce more offspring.  If eaten, the predator cell will be infected and carry the prey's eggs in place of its own.

#### Key Properties:
- `bodyPart`: The type of body part.
- `cell`: The cell this body part is attached to.

#### Key Methods:
- `BodyPartBehavior()`: Executes the behavior of the body part.
- `Photosynthesize()`: Executes the photosynthesis behavior.
- `Chemosynthesize()`: Executes the chemosynthesis behavior.

### DNABlueprint

The `DNABlueprint` class represents the DNA blueprint of a cell. It defines the body parts and abilities of a cell.

#### Key Properties:
- `bodyParts`: Array of body parts defined by the DNA.
- `team`: The team the DNA belongs to.
- `generation`: The generation of the DNA.

#### Key Methods:
- `BuildBlueprint()`: Builds the DNA blueprint.
- `Evolve()`: Evolves the DNA by mutating body parts.
- `MixBlueprints(DNABlueprint dad, DNABlueprint mom)`: Mixes two DNA blueprints to create a new one.

### WaterFlow

The `WaterFlow` class represents the water flow in the tank. It affects the movement of cells within its collider.

#### Key Properties:
- `cellsInCollider`: List of cells within the water flow collider.
- `flowDirection`: The direction of the water flow.

#### Key Methods:
- `FlowTheWater(float flowPower)`: Applies the water flow to the cells within the collider.

## Controls

- **Up Arrow**: Decrease game speed.
- **Down Arrow**: Increase game speed.
- **R**: Restart the simulation.
- **W**: Move the camera up.
- **S**: Move the camera down.
- **A**: Zoom in the camera.
- **D**: Zoom out the camera.
- **Q**: Decrease water flow speed.
- **E**: Increase water flow speed.

## Customization

You can customize the simulation by modifying the following parameters in the `Tank` class:

- `cellCount`: Number of cells to spawn.
- `gameSpeed`: Speed of the simulation.
- `maxLifeSpan`: Maximum lifespan of a cell.
- `maxDeathSpan`: Maximum death span of a cell.
