using UnityEngine;
using System.Collections;

public class OverviewAreaRenderer : MonoBehaviour {

	public GameObject overviewCamera;
	public GameObject overviewGUICamera;
	public Transform alignmentBox;


	// Use this for initialization
	void Start () {


		GenerateMesh ();
	}
	
	// Update is called once per frame
	void Update () {
	
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

	bool OnLeftHandSide(Vector3 start, Vector3 end, Vector3 p){
		
		return Mathf.Sign ((end.x - start.x) * (p.y - start.y) - (end.y - start.y) * (p.x - start.x)) == 1; 
		
	}

	void GenerateMesh(){


		Transform[] areaVerts = GetComponentsInChildren<Transform> ();
		Vector2[] verts = new Vector2[areaVerts.Length - 1];
		for (int i = 1; i < areaVerts.Length; i++) {
			verts[i-1] = overviewCamera.camera.WorldToScreenPoint(areaVerts[i].position);
			areaVerts[i].gameObject.SetActive(false);
		}



		
		
		MeshFilter mf = GetComponent<MeshFilter> ();
		Mesh mesh = new Mesh ();
		mesh.Clear ();

		
		Vector3[] verts3d = new Vector3[verts.Length];
		for (int i = 0; i < verts.Length; i++) {
			verts3d[i] = new Vector3(verts[i].x, verts[i].y, 8.0f);		
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
		

		Debug.Log ("overview: " + overviewCamera.camera.pixelRect.ToString());
		Debug.Log ("overviewGUI: " + overviewGUICamera.camera.pixelRect.ToString ());

		Vector3[] meshVerts = new Vector3[verts3d.Length];
		//Vector3[] meshVerts2 = new Vector3[verts3d.Length];
		for (int i = 0; i < meshVerts.Length; i++) {
			meshVerts[i] = overviewGUICamera.camera.ScreenToWorldPoint(verts3d[i]);
			//meshVerts2[i] = overviewCamera.camera.ScreenToWorldPoint(new Vector3(verts3d[i].x,verts3d[i].x,270.0f));
			//meshVerts[i] -= areaVerts[0].position;
		}

		
		mesh.vertices = meshVerts;
		mesh.triangles = meshTriangles;
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
		//outputMesh.GetComponent<MeshFilter> ().mesh = mesh;
		mf.mesh = mesh;


		BoxCollider coll = GetComponent<BoxCollider> ();
		coll.center = mesh.bounds.center;
		coll.size = new Vector3(mesh.bounds.size.x, mesh.bounds.size.y, 1.0f);

		//Draw lines
//		LineRenderer lr = outputMesh.GetComponent<LineRenderer> ();
//		lr.SetVertexCount (meshVerts.Length + 1);
//		for(int i = 0; i < meshVerts.Length; i++){
//			lr.SetPosition(i, meshVerts[i]);
//		}
//		lr.SetPosition (meshVerts.Length, meshVerts [0]);


	}


}
