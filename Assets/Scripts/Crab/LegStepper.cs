using System;
using System.Collections;
using DitzelGames.FastIK;
using UnityEngine;

namespace ScallyWags
{
    public class LegStepper : MonoBehaviour
    {
        // The position and rotation we want to stay in range of
        [SerializeField] private Transform homeTransform;

        [SerializeField] private Transform target;

        // Stay within this distance of home
        [SerializeField] private float wantStepAtDistance;

        // How long a step takes to complete
        [SerializeField] private float moveDuration;
        
        // Fraction of the max distance from home we want to overshoot by
        [SerializeField] float stepOvershootFraction;

        // Is the leg moving?
        public bool Moving;

        private void Start()
        {
            var gm = new GameObject();
            target = Instantiate(gm, transform.position, Quaternion.identity).transform;
            gm.name = "LegStepperTarget";
            var IK = gameObject.AddComponent<FastIKFabric>();
            IK.Target = target;
        }

        void Update()
        {
            // If we are already moving, don't start another move
            if (Moving) return;

            float distFromHome = Vector3.Distance(target.position, homeTransform.position);

            // If we are too far off in position or rotation
            if (distFromHome > wantStepAtDistance)
            {
                // Start the step coroutine
                StartCoroutine(MoveToHome());
            }
        }

        // Coroutines must return an IEnumerator
        private IEnumerator MoveToHome()
        {
            // Indicate we're moving (used later)
            Moving = true;

            // Store the initial conditions
            Quaternion startRot = target.rotation;
            Vector3 startPoint = target.position;

            Quaternion endRot = homeTransform.rotation;

            // Directional vector from the foot to the home position
            Vector3 towardHome = (homeTransform.position - transform.position);
            
            // Total distance to overshoot by   
            float overshootDistance = wantStepAtDistance * stepOvershootFraction;
            Vector3 overshootVector = towardHome * overshootDistance;
            
            // Since we don't ground the point in this simplified implementation,
            // we restrict the overshoot vector to be level with the ground
            // by projecting it on the world XZ plane.
            overshootVector = Vector3.ProjectOnPlane(overshootVector, Vector3.up);

            // Apply the overshoot
            Vector3 endPoint = homeTransform.position + overshootVector;

            // We want to pass through the center point
            Vector3 centerPoint = (startPoint + endPoint) / 2;
            // But also lift off, so we move it up by half the step distance (arbitrarily)
            centerPoint += homeTransform.up * Vector3.Distance(startPoint, endPoint) / 2f;

            float timeElapsed = 0;
            do
            {
                timeElapsed += Time.deltaTime;
                float normalizedTime = timeElapsed / moveDuration;
                normalizedTime = Easing.Cubic.InOut(normalizedTime);

                // Quadratic bezier curve
                target.position =
                    Vector3.Lerp(
                        Vector3.Lerp(startPoint, centerPoint, normalizedTime),
                        Vector3.Lerp(centerPoint, endPoint, normalizedTime),
                        normalizedTime
                    );

                target.rotation = Quaternion.Slerp(startRot, endRot, normalizedTime);

                yield return null;
            }
            while (timeElapsed < moveDuration);

            // Done moving
            Moving = false;
        }
    }
}