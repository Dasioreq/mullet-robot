using UnityEngine;

/**
*   @class AddCasing
*   @brief Script for lengthening an ammo belt of a weapon
*
*   This class simulates flailing casings being added to the outer side of a firearm with every shot. 
*   Adding links to the ArticulationBody joint chain regenerates the simulation, so instead, every link starts off INVISIBLE and with every call to \ref AddCasing::AddLink the next link in line becomes visible, until finally breaking Under the Pressure and creating a clone that detaches and falls off.
*/
public class AddCasing : MonoBehaviour
{
    [SerializeField] GameObject casingPrefab;
    [SerializeField] Vector3 offset;
    [SerializeField] uint chainMaxLength = 16;

    private ArticulationBody ab;
    private Transform currentLink;

    /// @brief Initializes the ArticulationBody joint chain and Component references
    void Start()
    {
        ab = GetComponent<ArticulationBody>();

        GameObject lastCasing = gameObject;
        for(int i = 0; i < chainMaxLength; i++)
        {
            GameObject casing = Instantiate(casingPrefab, lastCasing.transform.position + lastCasing.transform.rotation * offset, lastCasing.transform.rotation, lastCasing.transform);
            casing.transform.localScale = Vector3.one;

            ArticulationBody casingAb = casing.AddComponent<ArticulationBody>();
            casingAb.jointType = ArticulationJointType.SphericalJoint;
            casingAb.mass = 5;

            casingAb.twistLock = ArticulationDofLock.LimitedMotion;
            var xDrive = casingAb.xDrive;
            xDrive.lowerLimit = -25;
            xDrive.upperLimit = 25;
            casingAb.xDrive = xDrive;

            casingAb.swingYLock = ArticulationDofLock.LimitedMotion;
            var yDrive = casingAb.yDrive;
            yDrive.lowerLimit = -2.5f;
            yDrive.upperLimit = 2.5f;
            yDrive.stiffness = 10;
            casingAb.yDrive = yDrive;

            casingAb.swingZLock = ArticulationDofLock.LockedMotion;
            var zDrive = casingAb.zDrive;
            zDrive.lowerLimit = -2.5f;
            zDrive.upperLimit = 2.5f;
            zDrive.stiffness = 10;
            casingAb.zDrive = zDrive;

            casing.GetComponent<MeshRenderer>().enabled = false;
            casing.transform.SetParent(lastCasing.transform);
            lastCasing = casing;
        }
        currentLink = gameObject.transform;
    }

    /// @brief Adds a link to the chain
    public void AddLink()
    {
        if(currentLink.childCount == 0)
        {
            currentLink = gameObject.transform;
            currentLink.GetChild(0).localScale = Vector3.one * 100;
            var looseBelt = Instantiate(currentLink.GetChild(0), currentLink.GetChild(0).transform.position, currentLink.GetChild(0).rotation, null);
            currentLink.GetChild(0).localScale = Vector3.one;
            looseBelt.GetComponent<ArticulationBody>().linearVelocity += currentLink.GetChild(0).rotation * new Vector3(2, 0, 0) + Vector3.up * 2.5f;
            var i = looseBelt;
            while(i.childCount > 0)
            {
                i = i.GetChild(0);
            }

            i = currentLink;
            while(i.childCount > 0)
            {
                i = i.GetChild(0);
                i.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            currentLink = currentLink.GetChild(0);
            currentLink.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
