For little 360 renders and photosphere balls that, when clicked, open up and surround the player. 

To use: 
1. Place a photosphere manager prefab. 
2. Set "Balls Layer" to "photospheres" (if it doesn't exist, make it)
3. If necessary, set photosphere base material and selected sphere material. 
4. Place a photosphere prefab nested under Photosphere Manager
5. Import equirectangular image file. Under advanced settings, enable read/write.
6. set image file to the "Photosphere Image" in your photosphere object
7. Locate photosphere in scene according to where photo/rendering was taken.
8. Set manager and all photospheres to the "photosphere layer"
9. Make sure the bevel input script is in the scene and working.