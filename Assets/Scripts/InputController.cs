using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    private GameObject firstPosition;
    [SerializeField] private SceneManager sceneManager;

	void Update () {
	    if (Input.GetButtonDown("Fire1")) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit) 
                firstPosition = hit.transform.gameObject;
            else
                firstPosition = null;
        }
        
	    if (Input.GetButtonUp("Fire1")) {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit && firstPosition)
                sceneManager.SwapDiamonds(firstPosition, hit.transform.gameObject);
        }

        if (Input.GetButtonDown("Cancel"))
            Application.Quit();
	}
}
