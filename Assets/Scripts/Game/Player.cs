using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public float Speed = 3;

    private CharacterController cc;
    private float xMove;
    private float zMove;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }
    private void Update()
    {
        xMove = Input.GetAxisRaw("Horizontal");
        zMove = Input.GetAxisRaw("Vertical");

        if (xMove != 0 || zMove != 0)
        {
            cc.Move(new Vector3(xMove, 0, zMove).normalized * Speed * Time.deltaTime);
        }
    }
}
