using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hackSystem : MonoBehaviour {
    //taken from sightcone, may not need, but good to have until then
    public float visDist;
    public Material sightConeMat;
    public Vector3[] vertices3D;
    public MeshFilter filter;
    Vector2[] uv;
    float min = 0;
    float max = 1;
    public Transform debugPoint;
    public int numVerts;
    public float visArc;
    public float reloadTimerMax = .5f;
    public float reloadTimer = 0;
    public float maxHealth;
    public float health;
    public float power;
    public Transform laserPrefab;
    public int healthbarLength;
    public int healthbarHeight;
    Texture2D healthbarTex;
    Texture2D healthbarBGText;

    public bool playerOwned;
    public int hackLevel;
    public int hackSkill;
    Vector2[] accesPoints;
    
    bool flipControl(int hackSkill, int hackLevel, bool playerOwned){
        if(playerOwned != true){
            if(hackSkill > hackLevel){
                playerOwned = true;
            }
            return false;
        }
        return false;
    }
    /*
    Vector2 connectToNetwork(Vector2 accesPoints[], int hackSkill, GameObject person){
        if(hackSkill > 3){
            foreach(int i in accesPoints){
                if(i.hackLevel < person.hackSkill){
                    flipControl(i);
                }
            }
        }
        else exit(1);
    }
    */

}
