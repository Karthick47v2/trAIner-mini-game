using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour{
    public GameObject[] obsPrefabs;
    public GameObject[] spawnPoints;
    private List<GameObject> useableObjs;
    

    private void Start(){
        useableObjs = new List<GameObject>();
        ObjPool();
        ObjPool();
    }

    private void ObjPool(){
        foreach(GameObject go in obsPrefabs){
            GameObject goI = Instantiate(go,new Vector3(spawnPoints[0].transform.position.x, spawnPoints[0].transform.position.y, 
                    spawnPoints[0].transform.position.z),Quaternion.identity);
            // goI.SetActive(false);
            useableObjs.Add(goI);
            go.transform.localScale = new Vector3(2f,2f,1f);
            goI = Instantiate(go,new Vector3(spawnPoints[0].transform.position.x, spawnPoints[0].transform.position.y, 
                    spawnPoints[0].transform.position.z), Quaternion.identity);
            // goI.SetActive(false);
            useableObjs.Add(goI);
            goI = Instantiate(go,new Vector3(spawnPoints[1].transform.position.x, spawnPoints[1].transform.position.y, 
                    spawnPoints[1].transform.position.z), Quaternion.Euler(new Vector3(180, 0, 0)));
            // goI.SetActive(false);
            useableObjs.Add(goI);
            go.transform.localScale = new Vector3(1f,1f,1f);
            goI = Instantiate(go,new Vector3(spawnPoints[1].transform.position.x, spawnPoints[1].transform.position.y, 
                    spawnPoints[1].transform.position.z), Quaternion.Euler(new Vector3(180, 0, 0)));
            // goI.SetActive(false);
            useableObjs.Add(goI);
        }
    }

    private void FixedUpdate(){

    }
}
