using System;
using System.Collections.Generic;
using System.Linq;
using Jammy.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

// TODO: replace these with the processor input and output types.
using TiledLib;
using TInput = System.String;
using TOutput = System.String;

namespace JammyContentExtension
{
	// Each tile has a texture, source rect, and sprite effects.
	[ContentSerializerRuntimeType ("Jammy.TileMap.Tile, Jammy")]
	public class JTileContent
	{
		public ExternalReference<Texture2DContent> Texture;
		public Rectangle SourceRectangle;
		public SpriteEffects SpriteEffects;
	}

	// For each layer, we store the size of the layer and the tiles.
	[ContentSerializerRuntimeType ("Jammy.TileMap.Layer, Jammy")]
	public class JTileLayerContent
	{
		public int Width;
		public int Height;
		public JTileContent[] Tiles;
	}

	// We only support polygons on the object layer
	[ContentSerializerRuntimeType ("Jammy.TileMap.ObjectLayer, Jammy")]
	public class JObjectLayerContent
	{
		public int Width;
		public int Height;
		public List<Polygon> Polygons = new List<Polygon>();
	}

	// For the map itself, we just store the size, tile size, and a list of layers.
	[ContentSerializerRuntimeType ("Jammy.TileMap.Map, Jammy")]
	public class JMapContent
	{
		public int TileWidth;
		public int TileHeight;
		public List<JTileLayerContent> Layers = new List<JTileLayerContent> ();
		public List<JObjectLayerContent> ObjectLayers = new List<JObjectLayerContent>();
	}

	[ContentProcessor (DisplayName = "Jammy TMX Map Processor")]
	public class MapProcessor : ContentProcessor<TiledLib.MapContent, JMapContent>
	{
		public override JMapContent Process (TiledLib.MapContent input, ContentProcessorContext context)
		{
			// build the textures
			TiledHelpers.BuildTileSetTextures (input, context);

			// generate source rectangles
			TiledHelpers.GenerateTileSourceRectangles (input);

			// now build our output, first by just copying over some data
			JMapContent output = new JMapContent
			{
				TileWidth = input.TileWidth,
				TileHeight = input.TileHeight
			};

			// iterate all the layers of the input
			foreach (LayerContent layer in input.Layers)
			{
				if (layer is TiledLib.TileLayerContent)
				{
					output.Layers.Add (ParseTileLayer (input, layer as TiledLib.TileLayerContent));
				}
				else if (layer is MapObjectLayerContent)
				{
					output.ObjectLayers.Add (ParseObjectLayer (input, layer as MapObjectLayerContent));
				}
			}

			// return the output object. because we have ContentSerializerRuntimeType attributes on our
			// objects, we don't need a ContentTypeWriter and can just use the automatic serialization.
			return output;
		}

		private static JObjectLayerContent ParseObjectLayer(MapContent input, MapObjectLayerContent l)
		{
			JObjectLayerContent outLayer = new JObjectLayerContent
			{
				Width = l.Width,
				Height = l.Height
			};

			foreach (MapObjectContent o in l.Objects)
			{
				if (o.ObjectType == MapObjectType.Polygon)
				{
					var transformed = o.Points.Select (v => 
						new Vector2 (v.X + o.Bounds.X, v.Y + o.Bounds.Y)
					).ToArray();

					var polygon = new Polygon(transformed);
					outLayer.Polygons.Add (polygon);
				}
			}
			return outLayer;
		}

		private static JTileLayerContent ParseTileLayer (MapContent input, TileLayerContent l)
		{
			JTileLayerContent outLayer = new JTileLayerContent
			{
				Width = l.Width,
				Height = l.Height,
			};

			// we need to build up our tile list now
			outLayer.Tiles = new JTileContent[l.Data.Length];
			for (int i = 0; i < l.Data.Length; i++)
			{
				// get the ID of the tile
				uint tileID = l.Data[i];

				// use that to get the actual index as well as the SpriteEffects
				int tileIndex;
				SpriteEffects spriteEffects;
				TiledHelpers.DecodeTileID (tileID, out tileIndex, out spriteEffects);

				// figure out which tile set has this tile index in it and grab
				// the texture reference and source rectangle.
				ExternalReference<Texture2DContent> textureContent = null;
				Rectangle sourceRect = new Rectangle();

				// iterate all the tile sets
				foreach (var tileSet in input.TileSets)
				{
					// if our tile index is in this set
					if (tileIndex - tileSet.FirstId < tileSet.Tiles.Count)
					{
						// store the texture content and source rectangle
						textureContent = tileSet.Texture;
						sourceRect = tileSet.Tiles[(int) (tileIndex - tileSet.FirstId)].Source;

						// and break out of the foreach loop
						break;
					}
				}

				// now insert the tile into our output
				outLayer.Tiles[i] = new JTileContent
				{
					Texture = textureContent,
					SourceRectangle = sourceRect,
					SpriteEffects = spriteEffects
				};
			}
			return outLayer;
		}
	}
}