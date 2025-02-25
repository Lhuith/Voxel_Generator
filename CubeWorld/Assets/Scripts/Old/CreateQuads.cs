﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateQuads : MonoBehaviour
{
    public Material material;

    enum Cubeside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK };
	public enum BlockType{GRASS, DIRT, STONE};

	//Block Type and its corrosponding index on the texture Atlas
	public Dictionary<BlockType, Vector2> BlockDictionary = new Dictionary<BlockType, Vector2>(){
		{BlockType.GRASS, new Vector2(15, 1)},
		{BlockType.DIRT, new Vector2(15, 1)},
		{BlockType.STONE, new Vector2(15, 1)},
	}; 

	public BlockType bType;

	Vector2[,] blockUVs = {
		/* GRASS TOP */ {new Vector2(0.125f, 0.375f), new Vector2(0.1875f, 0.375f), 
						 new Vector2(0.125f, 0.4375f), new Vector2(0.1875f, 0.4375f)},

		/* GRASS SIDE*/ {new Vector2(0.1875f, 0.9375f), new Vector2(0.25f, 0.9375f), 
						 new Vector2(0.1875f, 1.0f), new Vector2(0.25f, 1.0f)},

		/* GRASS SIDE*/	{new Vector2(0.125f, 0.9375f), new Vector2(0.1875f, 0.9375f), 
						 new Vector2(0.125f, 1.0f), new Vector2(0.1875f, 1.0f)},

		/* GRASS SIDE*/	{new Vector2(0.0f, 0.875f), new Vector2(0.0625f, 0.875f), 
						new Vector2(0.0f, 0.9375f), new Vector2(0.0625f, 0.9375f)}
	};


    void CreateQuad(Cubeside side)
    {
        Mesh mesh = new Mesh();
        mesh.name = "ScriptedMesh";

        Vector3[] vertices = new Vector3[4];
        Vector3[] normals = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangles = new int[6];

        //all possible UVS
        Vector2 uv00;
        Vector2 uv10;
        Vector2 uv01;
        Vector2 uv11;

		if(bType == BlockType.GRASS && side == Cubeside.TOP){

			uv00 = blockUVs[0,0];
			uv10 = blockUVs[0,1];
			uv01 = blockUVs[0,2];
			uv11 = blockUVs[0,3];
		}
		else if(bType == BlockType.GRASS && side == Cubeside.BOTTOM){
			uv00 = blockUVs[(int)(BlockType.DIRT+1),0];
			uv10 = blockUVs[(int)(BlockType.DIRT+1),1];
			uv01 = blockUVs[(int)(BlockType.DIRT+1),2];
			uv11 = blockUVs[(int)(BlockType.DIRT+1),3];
		}
		else
		{
			uv00 = blockUVs[(int)(bType+1),0];
			uv10 = blockUVs[(int)(bType+1),1];
			uv01 = blockUVs[(int)(bType+1),2];
			uv11 = blockUVs[(int)(bType+1),3];
		}
        //all possible vertices

        Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
        Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
        Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
        Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);

        Vector2[] uvsref = new Vector2[] { (uv11), (uv01), (uv00), (uv10)};

		//Vector2[] uvsref = new Vector2[] { (uv11/16.00f), (uv01/16.00f), (uv00/16.00f), (uv10/16.00f)};
		//
		//for(int i = 0; i < uvsref.Length; i++){
		//	uvsref[i].y += 15f/16f;
		//}


        int[] trianglesref = new int[] { 3, 1, 0, 3, 2, 1 };

        switch (side)
        {
            case Cubeside.BOTTOM:
                vertices = new Vector3[] { p0, p1, p2, p3 };
                normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                uvs = uvsref;
                triangles = trianglesref;
                break;
            case Cubeside.TOP:
                vertices = new Vector3[] { p7, p6, p5, p4 };
                normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                uvs = uvsref;
                triangles = trianglesref;
			break;

			 case Cubeside.LEFT:
                vertices = new Vector3[] { p7, p4, p0, p3 };
                normals = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                uvs = uvsref;
                triangles = trianglesref;
                break;
            case Cubeside.RIGHT:
                vertices = new Vector3[] { p5, p6, p2, p1 };
                normals = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                uvs = uvsref;
                triangles = trianglesref;
			break;

			 case Cubeside.FRONT:
                vertices = new Vector3[] { p4, p5, p1, p0 };
                normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                uvs = uvsref;
                triangles = trianglesref;
                break;
            case Cubeside.BACK:
                vertices = new Vector3[] { p6, p7, p3, p2 };
                normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                uvs = uvsref;
                triangles = trianglesref;
			break;

        }



        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

        GameObject quad = new GameObject("Quad");
        quad.transform.parent = this.gameObject.transform;
        MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
        MeshRenderer renderer = quad.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = material;
    }
    // Use this for initialization

	void CombineQuads(){

		//1. Combine all children meshes
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];

		int i = 0;

		while(i < meshFilters.Length){
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			i++;
		}

		//2. Create a new mesh on the parent Object
		MeshFilter mf = (MeshFilter) this.gameObject.AddComponent(typeof(MeshFilter));
		mf.mesh = new Mesh();
		
		//3. Add combined meshes on children as the parents mesh
		mf.mesh.CombineMeshes(combine);

		//4. Create renderer for Parent
		MeshRenderer renderer = this.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		renderer.material = material;

		foreach(Transform quad in this.transform){
			Destroy(quad.gameObject);
		}
	}

	void CreateCube(){
		CreateQuad(Cubeside.FRONT);
		CreateQuad(Cubeside.BACK);
		CreateQuad(Cubeside.TOP);
		CreateQuad(Cubeside.BOTTOM);
		CreateQuad(Cubeside.LEFT);
		CreateQuad(Cubeside.RIGHT);
		CombineQuads();
	}
    void Start()
    {
        CreateCube();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
