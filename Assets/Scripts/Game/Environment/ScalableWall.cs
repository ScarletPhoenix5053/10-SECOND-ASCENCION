using System;
using System.Collections.Generic;
using UnityEngine;


namespace Sierra.AGPW.TenSecondAscencion
{
    public class ScalableWall : MonoBehaviour
    {
        private void Awake()
        {
            var objs = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject obj in objs)
            {
                var player = obj.GetComponent<PlayerController>();
                if (player != null) _players.Add(player);
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {            
            var player = collision.GetComponent<PlayerController>();
            if (player == null) return;

            Debug.Log("PLAYER");
            var dir = transform.position - player.transform.position;
            var dirInt = dir.x < 0 ? (int)Math.Floor(dir.x) : (int)Math.Ceiling(dir.x);
            player.TriggerWallTouch(dirInt);
        }

        private List<PlayerController> _players = new List<PlayerController>();
    }    
}