using UnityEngine;
using System.Collections;

namespace Sierra.AGPW
{
    public class CameraController : TrackMidoint
    {
        private void Awake()
        {
            _cam = Camera.main;
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            transform.position += new Vector3(0,0,_distance);

            var frustumHeight = 
                2.0f *
                Mathf.Abs(transform.position.z) *
                Mathf.Tan(_cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            var frustumWidth = frustumHeight * _cam.aspect;
            
            if (frustumWidth - MinDistFromFustrum*2 < GetDistance())
            {
                //Debug.Log("Too Far");
                transform.position -= new Vector3(0, 0, ZoomSpeed);
            }
            else if (frustumWidth - MaxDistFromFustrum * 2 > GetDistance())
            {
                //Debug.Log("Too Close");
                transform.position += new Vector3(0, 0, ZoomSpeed);
            }

            _distance = transform.position.z;
        }

        public float MinDistFromFustrum = 3;
        public float MaxDistFromFustrum = 4.5f;
        public float ZoomSpeed = 0.25f;

        private Camera _cam;
        private float _distance = -15f;
    }
}