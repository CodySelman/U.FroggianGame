using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpritesheetSlicer : ScriptableWizard
{
    public bool mipmapEnabled = false;
    public FilterMode filterMode = FilterMode.Point;
    public Vector2 spritePivot = Vector2.zero;
    public TextureImporterCompression textureImporterCompression =  TextureImporterCompression.Uncompressed;
    public SpriteMeshType spriteMeshType = SpriteMeshType.Tight;
    public uint spriteExtrude = 0;
    public int spriteHeight = 32;
    public int spriteWidth = 32;

	[MenuItem("Tools/Slice Spritesheets %&s")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<SpritesheetSlicer>("Slice Spritesheets", "Slice");
    }

    void OnWizardCreate()
    {
        Slice();
    }

	void Slice()
	{
		var textures = Selection.GetFiltered<Texture2D>(SelectionMode.Assets);
        if (textures.Length == 0) {
            Debug.LogError("No spritesheets selected. This tool works by selecting a spritesheet in the editor.");
        } else {
            foreach (var texture in textures)
            {
                ProcessTexture(texture);
            }
        }
	}

	void ProcessTexture(Texture2D texture)
	{
		string path = AssetDatabase.GetAssetPath(texture);
		var importer = AssetImporter.GetAtPath(path) as TextureImporter;

		//importer.isReadable = true;
		importer.textureType = TextureImporterType.Sprite;
		importer.spriteImportMode = SpriteImportMode.Multiple;
		importer.mipmapEnabled = mipmapEnabled;
		importer.filterMode = filterMode;
		importer.spritePivot = spritePivot;
		importer.textureCompression = textureImporterCompression;
        // need this class because spriteExtrude and spriteMeshType aren't exposed on TextureImporter
		var textureSettings = new TextureImporterSettings();
        importer.ReadTextureSettings(textureSettings);
		textureSettings.spriteMeshType = spriteMeshType;
		textureSettings.spriteExtrude = spriteExtrude;
		importer.SetTextureSettings(textureSettings);

        int colCount = texture.width / spriteWidth;
        int rowCount = texture.height / spriteHeight;
 
        List<SpriteMetaData> metas = new List<SpriteMetaData>();
 
        for (int r = 0; r < rowCount; ++r)
        {
            for (int c = 0; c < colCount; ++c)
            {
                SpriteMetaData meta = new SpriteMetaData();
                meta.rect = new Rect(c * spriteWidth, r * spriteHeight, spriteWidth, spriteHeight);
                meta.name = c + "-" + r;
                metas.Add(meta);
            }
        }
 
        importer.spritesheet = metas.ToArray();

		AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        AssetDatabase.Refresh();
	}
}