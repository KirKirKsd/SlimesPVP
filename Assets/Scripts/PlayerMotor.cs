using System;
using Mirror;
using UnityEngine;

public class PlayerMotor : NetworkBehaviour {

    [Header("Movement")] 
    [SerializeField] private float speed;

    [Header("Shooting")] 
    [SerializeField] private Transform pistolPivot;
    [SerializeField] private GameObject pistol;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bullet;
    
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private new Camera camera;

    private void Start() {
        if (!isLocalPlayer) {
            Destroy(this);
        }
        
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }

    private void Update() {
        Movement();
        Aiming();
        if (Input.GetMouseButton(0)) Shoot();
    }

    private void Movement() {
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input.x != 0) sr.flipX = input.x < 0;
        rb.velocity = input.normalized * speed;
    }

    private void Aiming() {
        Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 playerPosition = transform.position;
        var angle = Mathf.Atan2(playerPosition.y - mousePosition.y, playerPosition.x - mousePosition.x) * Mathf.Rad2Deg;
        if (playerPosition.x - mousePosition.x < 0) pistol.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        if (playerPosition.x - mousePosition.x > 0) pistol.transform.localScale = new Vector3(0.2f, -0.2f, 0.2f);
        pistolPivot.rotation = Quaternion.Euler(0, 0, angle + 180);
    }

    private void Shoot() {
        Instantiate(bullet, shootPoint.position, Quaternion.identity);
    }
    
}