using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    [SerializeField]
    private float size = 1f;
    public Vector3 GetNearestPoint(Vector3 pos) {
        pos -= transform.position;
        int xCount = Mathf.RoundToInt(pos.x/size);
        int zCount = Mathf.RoundToInt(pos.z/size);
        Vector3 result = new Vector3((float)xCount*size,0,(float)zCount*size);
        result += transform.position;
        return result;
    }

    //this will soon be obselete with new method
    public bool GetValidityPos(Vector3 pos, GameObject invis = null) {
        if(pos.x == -0.1f) {
            return false;
        }

        if(invis == null) {
            Collider[] groundHitColliders = Physics.OverlapSphere(pos, 0.25f,(1<<31));
            if(groundHitColliders.Length != 0) {
                return false;
            }
            Collider[] hitColliders = Physics.OverlapSphere(pos, 0.25f,(1<<6));
            if(hitColliders.Length == 0) {
                return true;
            }

        } else {
            //invis get collider then check the colliders collosion instead so its more accurate:)
        }
        return false;
    }
    /*
        Checks # 
        ###
        #o#
        ###
    */
    public Vector3 GetNearestValidPoint(Vector3 pos,int max = 10000) {
        Vector3 newPos = GetNearestPoint(pos);
        if(GetValidityPos(newPos)) {
            return newPos;
        }
        /* THIS DOESNT FIND THE EASIEST*/
        for(int x = -1; x < 2; x++) {
            for(int z =-1; z < 2; z++) {
                newPos = GetNearestPoint(new Vector3(pos.x+x,pos.y,pos.z+z));
                if(GetValidityPos(newPos)) {
                    return newPos;
                }
            }
        }

        return new Vector3(-0.1f,-0.1f,-0.1f);
    }
}
