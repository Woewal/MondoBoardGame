using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandCamera : MonoBehaviour {

    public static IslandCamera instance;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] AnimationCurve easing;

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }

    public IEnumerator PanTowardsLocation(float time, Vector3 targetLocation)
    {
        float currentTime = 0;

        Vector3 originalPosition = transform.position;

        while (currentTime < time)
        {
            transform.position = Vector3.Slerp(originalPosition, targetLocation, easing.Evaluate(currentTime / time));
            currentTime += Time.deltaTime;

            yield return null;
        }

        transform.position = targetLocation;
    }
}
