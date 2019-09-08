using System;
using UnityEngine;

[RequireComponent (typeof (MeshRenderer))]
public class PixelTerrainGenerator : MonoBehaviour
{
	[Serializable]
	public struct NoiseGenerator
	{
		public enum NoiseType
		{
			PerlinNoise,
			WhiteNoise,
			VerticalWhiteToBlack
		}

		public NoiseType Type;
		public float Strength;
		public float Exponent;
		public float Frequency;

		public bool IsActive;

		public void Generate (GenerationLayer layer)
		{
			if (!IsActive)
			{
				return;
			}

			int width = layer.Values.GetLength (0);
			int height = layer.Values.GetLength (1);

			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					float noiseSample;

					if (Type == NoiseType.PerlinNoise)
					{
						noiseSample = Mathf.PerlinNoise (x * Frequency, y * Frequency);
					}
					else if (Type == NoiseType.VerticalWhiteToBlack)
					{
						noiseSample = (1.0f - ((float)y) / height);
					}
					else
					{
						noiseSample = UnityEngine.Random.value;
					}

					noiseSample = Mathf.Pow (noiseSample, Exponent) * Strength;

					layer.Values[x, y] += noiseSample;
				}
			}
		}
	}

	public class GenerationLayer
	{
		public float[,] Values;

		public int Width { get; }
		public int Height { get; }

		public GenerationLayer (int width, int height)
		{
			Values = new float[width, height];
			Width = width;
			Height = height;

			// Debug.Log ($"Generating Noise Layer, {Width} x {Height}");
		}
	}

	public class PhysicalTerrain
	{
		public bool[,] Values;

		public Texture2D ManagedTexture { get; set; }

		public int Width { get; }
		public int Height { get; }
		public Color TerrainColor { get; }

		public PhysicalTerrain (GenerationLayer layer, Color terrainColor)
		{
			Width = layer.Width;
			Height = layer.Height;

			// Debug.Log ($"Generating Physical Layer, {Width} x {Height}");

			Values = new bool[layer.Width, layer.Height];

			for (int x = 0; x < layer.Values.GetLength (0); x++)
			{
				for (int y = 0; y < layer.Values.GetLength (1); y++)
				{
					float sample = layer.Values[x, y];

					Values[x, y] = sample > 0.5f;
				}
			}

			TerrainColor = terrainColor;
			ManagedTexture = CreateManagedTexture ();
		}

		private Texture2D CreateManagedTexture ()
		{
			var texture = new Texture2D (Width, Height, TextureFormat.ARGB32, false);
			texture.filterMode = FilterMode.Point;
			texture.alphaIsTransparency = true;

			// Debug.Log ($"Generating Texture, {Width} x {Height}");

			int count = 0;
			int countOfFill = 0;
			for (int x = 0; x < Values.GetLength (0); x++)
			{
				for (int y = 0; y < Values.GetLength (1); y++)
				{
					bool sample = Values[x, y];

					texture.SetPixel(x, y, sample ? TerrainColor : Color.clear);
					count++;

					if (sample)
					{
						countOfFill++;
					}
				}
			}

			// Debug.Log ($"Finished Generating Texture, {Width} x {Height}, {count}/{countOfFill} pixels");

			texture.Apply ();

			return texture;
		}
	}

	public int PixelsPerUnit;
	public NoiseGenerator[] Generators;
	public Color TerrainColor;

	private void Start ()
	{
		var layer = new GenerationLayer (Mathf.Abs((int)(transform.localScale.x * PixelsPerUnit)),
			Mathf.Abs((int)(transform.localScale.y * PixelsPerUnit)));

		foreach (var generator in Generators)
		{
			generator.Generate (layer);
		}

		var physicalLayer = new PhysicalTerrain (layer, TerrainColor);

		var meshRenderer = GetComponent<MeshRenderer> ();

		meshRenderer.material.mainTexture = physicalLayer.ManagedTexture;
	}

	private void OnValidate()
	{
		Start ();
	}

}
