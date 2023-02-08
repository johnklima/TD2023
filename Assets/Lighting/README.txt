As most of these have to be imported and baked into the main scene, I'll write a readme in case it is needed.
1. The SceneLighting is modifiable, as these were the settings that didnt take 4 hours for me to bake. Increase or decrease if needed in the settings. Recommended settings to increase: The Direct, Indirect, and Environmental Samples, and the light bounces. Also increase the Lightmap Resolution to at least 10 for the last bake (Default: 40 but takes like 6 days for me to render.).
2. The spore lights have to be parented by the spores in main scene.
3. Remember to plop the SceneLighting settings into the right place.
4. Insert the skybox in this folder into the right place.
5. I will include a folder with lightmaps and reflection-probes i baked, but feel free to bake yourself. They will have to be placed in the scenes/MainScene/ folder to work. Not tested.
6. Dont look at the LightProbes. If they cause performance issues, let me know immediately (I cant run the thingy because of errors in the FMOD import)
7. The "LightingImport" prefab should be placed at 0,0,0.
8. The mushrooms and plants should to be set to Recieve Global Illumination = LightProbes, else some plants turn black.