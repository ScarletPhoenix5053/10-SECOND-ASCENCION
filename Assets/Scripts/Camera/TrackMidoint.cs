using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sierra.AGPW
{
    public class TrackMidoint : MonoBehaviour
    {
        protected virtual void FixedUpdate()
        {
            if (TrackPerfectley) TrackMidpointPerfectly();
        }

        public Transform PointA;
        public Transform PointB;
        public Vector3 Offset;
        public bool TrackPerfectley = true;

        public Vector3 GetMidpoint()
        {
            var midpoint = (PointA.position + PointB.position) / 2;

            return midpoint + Offset;
        }
        public float GetDistance()
        {
            var dist = Vector3.Distance(PointA.position, PointB.position);

            return dist;
        }
        private void TrackMidpointPerfectly()
        {
            transform.position = GetMidpoint();
        }
    }
}