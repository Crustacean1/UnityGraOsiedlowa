using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public MeshRenderer Border;
    public MeshRenderer Center;
    public bool IsActive;

    // Start is called before the first frame update
    void Start()
    {
	    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    //Border.enabled = false;
	    if(Physics.Raycast(ray, out var hit, Mathf.Infinity)){
		    if(hit.collider.gameObject == Center.gameObject){
			    //Border.enabled = true;
		    }
	    }
    }
}
