using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour {

    [SerializeField] private List<GameObject> FirstRow;
    [SerializeField] private GameObject DiamondPrefab;
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private AudioClip swapSound;

    bool checkingFields = false;

    private void Start() {
        FillField();
    }

    private void Update() {
        if (!AreMovingExists() && !checkingFields) {
            //print("UPDATE SCENE");
            UpdateScene();
            CheckFields();
        }
    }

    private void ClearWaste() {
        foreach (var temp in GameObject.FindGameObjectsWithTag("Diamond"))
            if ((temp.transform.position.y > 8 || temp.GetComponent<Collider2D>().enabled == false) && !temp.GetComponent<DiamondController>().IsSwapping)
                Destroy(temp.transform.gameObject);
    }

    public void SwapDiamonds(GameObject d1, GameObject d2) {
        GetComponent<AudioSource>().PlayOneShot(swapSound);
        //if (!AreMovingExists())
            if (IsCross(d1.transform.position, d2.transform.position)) {
                d1.GetComponent<DiamondController>().SetSwapPos(d2.transform.position);
                d2.GetComponent<DiamondController>().SetSwapPos(d1.transform.position);
            }
    }

    //returns TRUE, if at least one diamond moving
    private bool AreMovingExists() {
        foreach (var temp in GameObject.FindGameObjectsWithTag("Diamond"))
            if (temp.GetComponent<DiamondController>().IsSwapping)
                return true;
        return false;
    }

    private bool IsCross(Vector3 d1, Vector3 d2) {
        return (d1.x == d2.x && (Mathf.Abs(d1.y - d2.y) == 1) || d1.y == d2.y && (Mathf.Abs(d1.x - d2.x) == 1));
    }

    public void CheckFields() {
        checkingFields = true;
        foreach (var temp in FirstRow) {
            RaycastHit2D hit = Physics2D.Raycast(temp.transform.position - Vector3.forward * 3, Vector2.zero);
            if (!hit) {
                print("Row: " + temp.transform.position.x);
                FillColumn(temp.transform.position);
            }
            //print("Check");
        }
        ClearWaste();
        checkingFields = false;
    }

    public void UpdateScene() {
        foreach (var temp in GameObject.FindGameObjectsWithTag("Diamond")) {
            temp.GetComponent<DiamondController>().MesureHeight();
        }
    }

    private void FillColumn(Vector3 pos) {
        RaycastHit2D hit = Physics2D.Raycast(pos + Vector3.down, Vector2.down);
        for (int i = 0; i < pos.y - Mathf.Floor(hit.point.y); i++) {
            //print("spawn: " + pos.x + " | " + pos.y);
            Instantiate(DiamondPrefab, new Vector3(pos.x, pos.y + 1 + i, -1), new Quaternion());
        }
    }

    private void FillField() {
        for (int i = 0; i <= width; i++)
            for (int j = 0; j <= height; j++)
                Instantiate(DiamondPrefab, new Vector3(j, i, -1), new Quaternion());
    }
}
