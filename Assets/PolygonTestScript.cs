using UnityEngine;
using System.Collections;

public class PolygonTestScript : MonoBehaviour {



	bool OnLeftHandSide(Vector3 start, Vector3 end, Vector3 p){

		return Mathf.Sign ((end.x - start.x) * (p.y - start.y) - (end.y - start.y) * (p.x - start.x)) == 1; 
	
	}

	int ProperModulus(int a, int b){
		if (a > 0) {
			return a % b;		
		} else {
			while (a < 0){
				a += b;
			}
			return a;
		}
	}

	// Use this for initialization
	void Start () {
	

		PolygonCollider2D coll = GetComponent<PolygonCollider2D> ();
		coll.SetPath (0, new Vector2[]{
			new Vector2(0.0f,0.0f),
			new Vector2(0.0f,1.0f),
			new Vector2(1.0f,1.0f),
			new Vector2(2.0f,-0.5f),
			new Vector2(3.0f,-0.5f),
			new Vector2(4.0f,0.0f),
			new Vector2(4.5f,-0.5f),
			new Vector2(3.0f,-1.0f),
			new Vector2(1.5f,-1.0f),
			new Vector2(1.0f,0.0f)
		});
		Debug.Log (coll.pathCount);


		MeshFilter mf = GetComponent<MeshFilter> ();
		Mesh mesh = new Mesh ();
		mesh.Clear ();

		Vector2[] verts = coll.GetPath (0);

		Vector3[] verts3d = new Vector3[verts.Length];
		for (int i = 0; i < verts.Length; i++) {
			verts3d[i] = new Vector3(verts[i].x, verts[i].y, 0);		
		}

		int[] meshTriangles = new int[3*(verts3d.Length-1)];


		//Baldwin algorithm:
		int pivot = 0;
		int offset = 0;
		bool forwards = true;

		for (int i = 0; i < verts3d.Length - 1; i++) {


			if(OnLeftHandSide(verts[pivot], verts[ProperModulus(pivot + (forwards ? 1 : -1) * (offset + 2),verts.Length)], verts[ProperModulus(pivot + (forwards ? 1 : -1) * (offset + 1),verts.Length)]) == (forwards ? true : false)){
				//MAKE A TRIANGLE FOR GOD'S SAKE
				//Wait... not yet!
				//Check if making a triangle here would overlap any vertices
				bool allGood = true;
				if(forwards){	
					for(int j = pivot + offset + 3; j < verts.Length; j++){
						if(OnLeftHandSide(verts[pivot], verts[pivot + offset + 2], verts[j])){
							allGood = false;
							break;
						}
					}
				} else {

					for(int j = ProperModulus(pivot - offset - 3,verts.Length); j != pivot; j = ProperModulus(j -1,verts.Length)){
						if(!OnLeftHandSide(verts[pivot], verts[ProperModulus(pivot - (offset + 2),verts.Length)], verts[j])){
							allGood = false;
							break;
						}
					}
				}
				if(!allGood){
					pivot = ProperModulus(pivot + (forwards ? 1 : -1) * (offset + 1),verts.Length);
					forwards = !forwards;
				}
				meshTriangles[i*3] = pivot;
				meshTriangles[i*3 + (forwards ? 1 : 2)] = ProperModulus(pivot + (forwards ? 1 : -1) * (offset + 1),verts.Length);
				meshTriangles[i*3 + (forwards ? 2 : 1)] = ProperModulus(pivot + (forwards ? 1 : -1) * (offset + 2),verts.Length);
				offset += 1;

			} else {

				pivot = ProperModulus(pivot + (forwards ? 1 : -1) * (offset + 1),verts.Length);
				forwards = !forwards;
				i--;

			}

			if(ProperModulus(pivot + (forwards ? 1 : -1) * (offset + 2),verts.Length) == pivot)
			{
				break;
			}


		}



		mesh.vertices = verts3d;
		mesh.triangles = meshTriangles;
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();

		mf.mesh = mesh;




	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
