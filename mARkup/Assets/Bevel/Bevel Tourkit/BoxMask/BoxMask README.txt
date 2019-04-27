Features
	Dynamically crop any number of objects to view only part of a model. 
	Dynamically cull and load any objects completely outside the crop boundary.
	Crop with transparency
Dependencies
	None
Documentation
	Example
	Instructions


To use: 
1. Place the BoxMaskMaterialAligner prefab in the scene and scale/move into desired place.
2. Put whatever objects you want it to workwith under a single game object and 
3. Point to it "Box Mask Material Aligner"'s "Target Game Object" slot
4. Make sure all objects you want cropped are using materials with one of the included shaders. Either:
	Hyperreal Standard Box Mask Opaque or
	Hyperreal Standard Box Mask Transparent
5. If raycasting into a cropped area, be sure to use Bevel Input's "Cropped RayCast".