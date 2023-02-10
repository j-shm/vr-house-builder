# Guide on making objects for the game

## Objects

### Create model
Create a model in blender or whatever your preferred modelling program is. Models will have their colliders automatically generated when they are imported: to exclude meshes from being used to make the collider call the mesh EXCLUDE. 

#### Export Model

Export this model as a .glb and give it a name.

### Create Json File

Create a .json file in the same directory with the same name e.g window.glb window.json. Fill in infomation about the object here's the format:
```json
{
    "catalog": {
        "name":"",
        "description":""
    },
    "infomation": {
        "type":""
    }
}
```
```type``` can either be ```object``` or ```window```

## Windows

Window's should have any extra details defined as EXCLUDE. The main part of the window should be the only part that is not excluded. E.g a overhang should be excluded.