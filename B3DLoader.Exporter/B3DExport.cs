using B3DLoader.Data;
using B3DLoader.Extensions;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Scenes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace B3DLoader.Exporter;

using VERTEX = VertexBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>;
using MESH = MeshBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>;

public class B3DExport
{
	private SceneBuilder scene;
	private B3DModel model;

	private List<MaterialBuilder> materials;

	private string? outPath;

	public B3DExport( B3DModel mdl )
	{
		scene = new SceneBuilder();
		materials = new List<MaterialBuilder>();
		model = mdl;
	}

	public void ExportModel( string path )
	{
		outPath = path;

		if ( model == null || string.IsNullOrEmpty( outPath ) )
		{
			Log.Error( "Failed to export model, B3DModel or outPath is null!" );
			return;
		}

		// Add textures
		// TODO: Actually load the images if they exist relative to the b3d
		foreach ( var tex in model.Textures.TexData )
		{
			var mb = new MaterialBuilder();
			mb.Name = tex.Name;

			materials.Add( mb );
		}

		var rootNode = model.RootChunk.Children.Where( x => x.ChunkType == ChunkTypes.NODE ).First();
		var node = createNode( rootNode );

		// Rotate node so we're the correct orientation
		// TODO: Factor in original rotation?
		node?.WithLocalRotation( Quaternion.CreateFromYawPitchRoll( 180f.ToRadians(), -90f.ToRadians(), 0 ) );

		scene.AddNode( node );

		var gltfMdl = scene.ToGltf2();
		gltfMdl.SaveGLTF( outPath );
	}

	private NodeBuilder? createNode( B3DChunk nodeChunk )
	{
		if ( nodeChunk == null )
		{
			return null;
		}

		var nb = new NodeBuilder();

		var dataBlock = nodeChunk.DataBlock as B3DNodeData;
		if ( dataBlock == null )
		{
			Log.Error( "Data block was not node data" );
			return null;
		}

		var subData = dataBlock.Data;

		nb.Name = nodeChunk.DataBlock.Name;
		nb.WithLocalTranslation( subData.Position );
		nb.WithLocalRotation( subData.Rotation );
		nb.WithLocalScale( subData.Scale );

		if ( nodeChunk.Children.Count == 0 )
		{
			return nb;
		}

		// Parse child chunks
		var childNodes = nodeChunk.Children.Where( x => x.ChunkType == ChunkTypes.NODE ).ToList();
		foreach ( var child in childNodes )
		{
			nb.AddNode( createNode( child ) );
		}

		// Nodes may only have either 1 node *OR* bone.
		var mdl = nodeChunk.Children.Where( x => x.ChunkType == ChunkTypes.MESH ).FirstOrDefault();
		if ( mdl != null )
		{
			var mesh = createMesh( mdl );
			nb.Name = mesh.Name;

			scene.AddRigidMesh( mesh, nb );
		}

		return nb;
	}
	
	// TODO: handle multiple materials on 1 mesh, dunno how we'll do this
	private MESH createMesh( B3DChunk mesh )
	{
		MESH mb = new MESH();

		var meshBlock = mesh.DataBlock as B3DMeshData;
		if ( meshBlock == null )
		{
			Log.Error( "Mesh block was equal to null!" );
			return mb;
		}

		var triBlock = meshBlock.Triangles[0];
		var brush = model.Brushes.BrushData[triBlock.BrushId];

		mb.Name = brush.Name;

		var prim = mb.UsePrimitive( materials[brush.TextureIds.Last()] );

		var vertList = meshBlock.Vertices.Verts;

		for ( var i = 0; i < meshBlock.Triangles.Count; i++ )
		{
			var tris = meshBlock.Triangles[i];

			foreach ( var tri in tris.Triangles )
			{
				var v1 = vertList[tri.Point1];
				var v2 = vertList[tri.Point2];
				var v3 = vertList[tri.Point3];

				bool hasNormals = meshBlock.Vertices.Flags.HasFlag( B3DVertData.VertFlags.HasNormals );

				Vector3 n = Vector3.Zero;
				if ( !hasNormals )
				{
					var ab = v2.Position - v1.Position;
					var ac = v3.Position - v1.Position;
					n = Vector3.Normalize( Vector3.Cross( ab, ac ) );
				}

				var n1 = hasNormals ? v1.Normal : n;
				var n2 = hasNormals ? v2.Normal : n;
				var n3 = hasNormals ? v3.Normal : n;

				var vn1 = new VertexPositionNormal( v1.Position, n1 );
				var vn2 = new VertexPositionNormal( v2.Position, n2 );
				var vn3 = new VertexPositionNormal( v3.Position, n3 );

				// TODO: Factor in advanced texcoords here
				var uv1 = v1.TexCoords[0];
				var uv2 = v2.TexCoords[0];
				var uv3 = v3.TexCoords[0];

				var vUV1 = new VertexTexture1( uv1 );
				var vUV2 = new VertexTexture1( uv2 );
				var vUV3 = new VertexTexture1( uv3 );

				var vert1 = new VERTEX( vn1, vUV1, (1, 1) );
				var vert2 = new VERTEX( vn2, vUV2, (1, 1) );
				var vert3 = new VERTEX( vn3, vUV3, (1, 1) );

				prim.AddTriangle( vert1, vert2, vert3 );
			}
		}

		return mb;
	}
}
