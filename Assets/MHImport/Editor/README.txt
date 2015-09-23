Using MHImport:


Part 1 * If you're already familiar with MakeHuman, you can skip this part.

- Open MakeHuman. Play with the "macro" sliders to change gender, age, weight, height, etc.
- Go to the "Materials" tab and choose one of the materials for "skin". You can choose any, just don't leave it at "default" or "toon".
- Go to the "Geometries" tab. There should be several subtabs here, the first one open will be "Clothes". Choose clothes from the tab on the left, but too much layering will cause ugly artifacts to appear as lower layers point out of upper layers. It works well on one layer per section of the body. The fedora doesn't seem to work with hair.
- (Optional) Go to "Teeth". Choose any teeth. Then, go to "Eyebrows", "Eyelashes", and "Tongue" and do the same. You'll want to leave out this step if you're shooting for a low polygon count.
- (Optional) If you're on a tight performance budget, select one of the lower-polygon proxies from the "Topologies" tab.
- Go to "Pose/Animate". Go to the "Skeleton" subtab. Select the "basic" skeleton for more control over your character's movements or the "game" skeleton for better performance.
- (Optional, but a good idea) Click on "Files". Click on the "Save" subtab. Save your mesh.
- In the "Files" main tab, click on the "Export" subtab. First, export to the "BLender exchange (mhx)" format (mesh format is on the left). This step is not optional, as the textures will not be exported to the export directory if you only export to the fbx format.
- Then, click on the "Filmbox (fbx)" tab. Do not change any of the settings for now. Export the fbx file.
- Click on the three dots. Copy the directory which appears to the clipboard. It should show your .mhx and .fbx files as well as a "textures" directory.
- End of part one. Close MakeHuman or make a few more and use the batch import option!
* Note: If you want to use separate meshes in different programs, you'll probably want to export your humans into subdirectories. This is because every texture in the "textures" folder is transferred over to your Unity project.


Part 2

- Go to Unity.
- Click on "Window", then "MH Import".
- It is advisable that you choose a Destination path to avoid cluttering up the main directory. The utility  will generate the directory at runtime if it does not exist already.
- Select the source path. Choose "Select file" for individual files or "Select folder (batch import)" to import every file in a directory. Either way, every texture in the "textures" folder will be copied.
- After a minute or two (on an old laptop), your files should be imported and the textures and materials should be applied.
- (Optional) Makehuman meshes are fully compatable with Mecanim animations. There are many tutorials on how to do this, though I'd rather not do the authors a disservice by lifting their work. Here's a link to one instead: http://xenosmashgames.com/importing-makehuman-mecanim-unity-3d/

The "Importing MakeHuman" checkbox should remain checked for makehuman meshes. Other meshes are not officially supported, but should import fine with diffuse and normal mapped effects where appropriate if you use a "[MeshName].[extension]" and "[MeshName]normal.[extension]" for all your textures and place them all in a "[sourcefile directory]/textures" folder. Once again, not officially supported as of now, but might be an interesting thing to try out.

Good luck, and if this becomes popular enough I may expand it to include other functions like clothes correction to fix the layering artifacts.