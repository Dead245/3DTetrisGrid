# Grid-Based Item Placement System

This is a **Unity 6** grid-based item placement system designed for precise item snapping within a defined trigger volume.
The system dynamically tracks occupied cells and ensures correct placement based on item shape and rotation, item types and grid sizes are saved as scriptable objects for quick iterating/swapping.

| Feature | Description |
| ----------- | ----------- |
| **Player Input Map** | Utilizes Unity's newer player input mapping system for easier controls. |
| **Item Rotation** | Items can be rotated with the scroll wheel (left/right), with Shift as the default modifier to change the direction to up/down. |
| **Grid Snapping** | Outside of the grids, items can be moved around normally and can be rotated in small increments at a time. But, once in a grid, the positioning snaps the the nearest cells that can fit the item, and the rotation snaps to 90 degree increments.
| **Scriptable Object-Based Grids & Items** | Both grids and items use ScriptableObjects to determine size and shape. |
| **ScriptableObject-Based Shapes** | Items define their shape via a cellOffsets[] array. |
| **Multi-Grid Support** | Multiple grids can exist in the scene, each with its own trigger volume. |
| **Customizable Cell Size** | Uses 0.1 unit cell scale by default, editable via a master script. |
| **Dynamic Grid Tracking** | Only stores occupied cells instead of maintaining a full grid in memory. |
| **Raycast-Based Positioning** | Ensures accurate placement using raycasting. |
| **Trigger Volume Detection** | Grids determine when a held item is in side their grid via a trigger volume. |
| **Toggleable Grids** | Grids can be toggled on/off for performance, keeping attached items in place. |
| **Setup Warnings** | Several console warnings have been added incase of inproper setup or other issues with the scripts | 
| **Gizmo Debugging** | Item shapes are represented by Gizmos for easier debugging. |

The basic scene has a player controller that can pick up and interact with 2 different items, and can toggle on the 3 different grids and place the items in them.

## You can take this project and edit it however you wish.
