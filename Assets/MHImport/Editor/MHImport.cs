using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class MHImport : EditorWindow
{
	[MenuItem("Window/MH Import")]
	static void OpenWindow()
	{
		var window = ScriptableObject.CreateInstance<MHImport>();
		window.title = "MH Import";
		window.Show();
	}
	public string sourceFile = "";
	public string destinationFolder = "/";
	public string buttonText = "Import asset";
	public float scale = 0.12f;
	public int maxTex = 2048;
	public int lastTex = 2048;
	public bool isFolder = false;
	public bool useMats = true;
	public bool justLoaded = true;

	void OnGUI()
	{
		drawGUI();
	}
	public void drawGUI()
	{
		try {
			EditorGUILayout.LabelField("Destination path (relative):");
			destinationFolder = EditorGUILayout.TextField(destinationFolder);
			EditorGUILayout.LabelField("Source path (absolute):");
			sourceFile = EditorGUILayout.TextField(sourceFile);
			if (GUILayout.Button("Select file"))
			{
				sourceFile = EditorUtility.OpenFilePanel(
					"Select source file",
					sourceFile,
					"fbx");
				isFolder = false;
			} 
			if (GUILayout.Button("Select folder (batch import)"))
			{
				sourceFile = EditorUtility.OpenFolderPanel(
					"Select source folder",
					sourceFile,
					"");
				isFolder = true;
			} 
			scale = EditorGUILayout.FloatField("Scale: ",scale);
			maxTex = EditorGUILayout.IntField("Maximum texture size", maxTex);
			useMats = EditorGUILayout.Toggle("Importing MakeHuman",useMats);
			
			string normal = "normal";
			string diffuse = "diffuse";
			bool modified = false;
			string destPath = "";
			List<string> rootpaths = new List<string>();
			if (GUILayout.Button(buttonText))
			{
				string filepath = "";
				{
					if (!destinationFolder.StartsWith("/"))
						destinationFolder = "/" + destinationFolder;
					destPath = Application.dataPath + destinationFolder;
					
					bool source = false;
					try {
						Path.IsPathRooted(destPath);
						source = true;
						Path.IsPathRooted(sourceFile);
						
						string texDir = sourceFile + "/textures/";
						if (!isFolder)
							texDir = texDir.Replace(Path.GetFileName(sourceFile), "");
						string [] textures = Directory.GetFiles(texDir,
						                                        "*.png", SearchOption.AllDirectories);
						
						if (!Directory.Exists(destPath))
							Directory.CreateDirectory(destPath);
						if (!Directory.Exists(destPath + "/textures"))
							Directory.CreateDirectory(destPath + "/textures");
						foreach(string texture in textures)
						{
							filepath = destPath + "/textures/" + Path.GetFileName(texture);
							if (!System.IO.File.Exists(filepath))
							{
								modified = true;
								System.IO.File.Copy(texture, filepath, true);
							}
						}
						if (!isFolder)
						{
							string sourcePath = destPath + "/" + Path.GetFileName(sourceFile);
							string rootpath = "Assets" + sourcePath.Replace(Application.dataPath, "").Replace("\\","/");
							rootpaths.Add(rootpath);
							System.IO.File.Copy(sourceFile, sourcePath, true);
						}
						else
						{
							string [] roots = Directory.GetFiles(sourceFile,
							                                        "*.fbx", SearchOption.TopDirectoryOnly);
							foreach(string root in roots)
							{
								string sourcePath = destPath + "/" + Path.GetFileName(root);
								string rootpath = "Assets" + sourcePath.Replace(Application.dataPath, "").Replace("\\","/");
								rootpaths.Add(rootpath.Replace("\\","/"));
								System.IO.File.Copy(root, sourcePath, true);
							}
						}
						AssetDatabase.Refresh();

					}
					catch (System.Exception e){
						Debug.Log (e.Message);
						if (!source)
							buttonText = "Invalid destination path selected.";
						else
							buttonText = "Invalid source path selected.";
						return;
					}
				}    
				string[] texes = Directory.GetFiles(destPath + "/textures",
				                                    "*.png", SearchOption.AllDirectories);

				for (int i = 0; i < texes.Length; ++i)
				{
					string assetPath = "Assets" + texes[i].Replace(Application.dataPath, "").Replace("\\","/");
					texes[i] = assetPath;
					if (modified || maxTex != lastTex || justLoaded)
					{
						string texName = assetPath.Substring(assetPath.LastIndexOf("/")+1);
						TextureImporter textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
						
						if (texName.Contains(normal))
							textureImporter.convertToNormalmap = true;
						textureImporter.maxTextureSize = maxTex;
						AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
					}
				}
				lastTex = maxTex;
				EditorApplication.SaveAssets();
				AssetDatabase.Refresh();
				foreach(string rootpath in rootpaths)
				{
					{
						
						ModelImporter importer = AssetImporter.GetAtPath(rootpath) as ModelImporter;
						importer.globalScale = scale;
						importer.importMaterials = true;
						AssetDatabase.ImportAsset(rootpath, ImportAssetOptions.ForceUpdate);
					}
					Object [] objs = AssetDatabase.LoadAllAssetsAtPath(rootpath);
					if (useMats)
					{
						foreach(Object obj in objs)
						{
							if (obj.GetType() != typeof(SkinnedMeshRenderer))
								continue;
							SkinnedMeshRenderer mesh = (SkinnedMeshRenderer)obj;
							string meshName = mesh.name;
							Material sourceMat = mesh.sharedMaterial;
							if (meshName.LastIndexOf((string)"Mesh") >= 0)
								meshName = meshName.Substring(0,meshName.LastIndexOf((string)"Mesh"));
							if (Path.GetFileName(rootpath).LastIndexOf(meshName) >= 0)
								continue;

							string name = sourceMat.name;
							sourceMat.shader = Shader.Find("MHImport/CustomShaderNoBump");
							{
								if (name.LastIndexOf((string)"eye") >= 0 || name.LastIndexOf((string)"diffuse_black") >= 0)
									sourceMat.shader = Shader.Find("MHImport/CustomCutoutShader");
								string basic = "basic";
								string testName = name.Replace(basic, "");
								char[] chars = {'_','0','1','2','3','4','5','6','7','8','9'};
								int last = testName.LastIndexOfAny(chars);
								if (last > 1)
									testName = testName.Substring(0,last-1);
								for (int i = 0; i < texes.Length; ++i)
								{
									string texName = texes[i].Substring(texes[i].LastIndexOf((string)"/")+1);
									if (texName.Contains(normal))
									{
										if (texName.StartsWith(testName) ||
										    (name.StartsWith(diffuse) && texName.StartsWith(normal)))
										{
											sourceMat.shader = Shader.Find("MHImport/CustomShaderBump");
											Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(texes[i], typeof(Texture2D));
											sourceMat.SetTexture("_BumpMap",tex);
											
											break;
										}
									}
								}
							}
						}
					}
					else
					{
						foreach(Object obj in objs)
						{
							if (obj.GetType() != typeof(SkinnedMeshRenderer))
								continue;
							SkinnedMeshRenderer mesh = (SkinnedMeshRenderer)obj;
							string meshName = mesh.name;
							Material sourceMat = mesh.sharedMaterial;
							if (meshName.LastIndexOf((string)"Mesh") >= 0)
								meshName = meshName.Substring(0,meshName.LastIndexOf((string)"Mesh"));
							
							string testName = sourceMat.name;
							sourceMat.shader = Shader.Find("Diffuse");
							{
								for (int i = 0; i < texes.Length; ++i)
								{
									string texName = texes[i].Substring(texes[i].LastIndexOf((string)"/")+1);
									if (texName.Contains(normal))
									{
										if (texName.StartsWith(testName))
										{
											sourceMat.shader = Shader.Find("Bump Diffuse");
											Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(texes[i], typeof(Texture2D));
											sourceMat.SetTexture("_BumpMap",tex);
											
											break;
										}
									}
								}
							}
						}
					}
				}
				justLoaded = false;
				buttonText = "Asset imported.";

			}
		}
		catch(System.Exception e)
		{
			Debug.LogError (e.Message);
		}
	}
}
