using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_ThirdJump : MonoBehaviour
{
    [SerializeField] private GameObject ShotPoint;
    [SerializeField] private GameObject Sword;
    [SerializeField] private Transform SwordSpawnPoint;
    [SerializeField] private Player Player;
    private GameObject SwordInstance;
    private Vector2 worldPosistion;
    private Vector2 direction;
    private float angle;
    
    void Start()
    {
     Player = GetComponent<Player>();
     Sword.SetActive(false);   
    }
    void Update()
    {
        HandleRotation();
        HandleThirdJump();
        
    }
    void HandleRotation()
    {
        worldPosistion = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        direction = (worldPosistion - (Vector2)ShotPoint.transform.position).normalized;
        ShotPoint.transform.right = direction;
        angle   = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Vector3 localScale = new Vector3(1f, 1f, 1f);
        if(angle > 20 || angle < -20)
        {
            localScale.y = -1f;
        }
        else
        {
            localScale.y = 1f;
        }
        ShotPoint.transform.localScale = localScale;
    }

    void HandleThirdJump()
    {
        if(Player.jumpCount == 1 && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Sword.SetActive(true);
            SwordInstance = Instantiate(Sword, SwordSpawnPoint.position, ShotPoint.transform.rotation);
        }
    }

}

