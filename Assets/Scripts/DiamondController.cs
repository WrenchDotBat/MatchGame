using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondController : MonoBehaviour {

    float speed = 3f;
    float swapSpeed = 2f;

    float targetY;
    float targetX;
    int value;
    bool isSwapping = true;
    bool wasActive = false;

    public bool IsSwapping {
        get { return isSwapping; }
    }

    public void SetSwapPos(Vector2 pos) {
        targetX = pos.x;
        targetY = pos.y;
        wasActive = true;
    }

    private void Start() {
        targetX = transform.position.x;
        targetY = transform.position.y;
        value = Random.Range(1, 6);

        switch (value) {
            case 1: GetComponent<Renderer>().material.SetColor("_Color", Color.grey); break;
            case 2: GetComponent<Renderer>().material.SetColor("_Color", Color.cyan); break;
            case 3: GetComponent<Renderer>().material.SetColor("_Color", Color.green); break;
            case 4: GetComponent<Renderer>().material.SetColor("_Color", Color.magenta); break;
            case 5: GetComponent<Renderer>().material.SetColor("_Color", Color.red); break;
        }
        MesureHeight();
    }

    private void Update() {
        if (transform.position.y != targetY) {
            sbyte dir = 1;
            if (transform.position.y > targetY) dir = -1;
            transform.Translate(Vector2.up * dir * Time.deltaTime * speed);
        } else if (transform.position.x != targetX) {
            sbyte dir = 1;
            if (transform.position.x > targetX) dir = -1;
            transform.Translate(Vector2.right * dir * Time.deltaTime * swapSpeed);
        }

        Stabilization();

        isSwapping = !(targetX == transform.position.x && targetY == transform.position.y);
        if (!isSwapping && wasActive)
            DestroyDiamond(value);

    }

    public void DestroyDiamond(int valueColor) {
        if (valueColor != value) return;
        GetComponent<Collider2D>().enabled = false;
        DestroyAt(Vector2.left);
        DestroyAt(Vector2.up);
        DestroyAt(Vector2.right);
        DestroyAt(Vector2.down);
        Destroy(gameObject);
    }

    private void DestroyAt(Vector2 direction) {
        Vector3 pos = new Vector3(direction.x, direction.y, -3) + transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit) {
            DiamondController diamond = hit.transform.GetComponent<DiamondController>();
            if (diamond)
                diamond.DestroyDiamond(value);
        }
    }

    private void Stabilization() {
        if (Mathf.Abs(transform.position.x - targetX) < 0.1) transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        if (Mathf.Abs(transform.position.y - targetY) < 0.1) transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }

    public void MesureHeight() {
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 1, -3f), Vector2.zero);
        if (!hit) {
            Ray2D ray = new Ray2D(transform.position + Vector3.down, Vector2.down);
            Debug.DrawRay(ray.origin, ray.direction, Color.green, 20f);
            RaycastHit2D botRay = Physics2D.Raycast(ray.origin, ray.direction, 15f);
            FallDown(Mathf.Ceil(botRay.point.y));
        }
        //print(transform.position + " shoots at " + hit.transform.position);
    }

    public void FallDown(float y) {
        targetY = y;
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 1, -3), Vector2.zero);
        if (hit)
            hit.transform.GetComponent<DiamondController>().FallDown(y + 1);
    }

}
