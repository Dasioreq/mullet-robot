using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ViewmodelAmbientLighting : MonoBehaviour
{
    [SerializeField] Color sunColor;

    private Quaternion sunDirection;
    private GameObject sun;

    void Start()
    {
        sun = GameObject.Find("ViewmodelSun");
        sunDirection = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.down) * Quaternion.AngleAxis(Random.Range(45, 135), Vector3.right);
    }

    IEnumerator Switcheroo() // NOTE: Change name for production
    {
        float time = 0f;
        var previousColor = sun.GetComponent<Light>().color;
        var previousDirection = sun.transform.rotation;

        while (time < .5f)
        {
            sun.GetComponent<Light>().color = Color.Lerp(previousColor, sunColor, time * 2);
            sun.transform.rotation = Quaternion.Lerp(previousDirection, sunDirection, time * 2);

            time += Time.deltaTime;
            yield return null;
        }

        sun.GetComponent<Light>().color = sunColor;
        sun.transform.rotation = sunDirection;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "PlayerObj")
        {
            StartCoroutine(Switcheroo());
        }
    }
}
