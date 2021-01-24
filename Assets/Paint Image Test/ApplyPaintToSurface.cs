using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyPaintToSurface : MonoBehaviour
{
    [Header("Main Player Object")]
    [Header("Press'q' to change images")]
    public GameObject main;
    [Header("Images to be Painted")]
    public GameObject[] image;

    private GameObject selectedImage;
    private int i = 0;

    [Header("Image Signs")]
    public GameObject[] sign;

    [Header("Painting Sound")]
    public AudioSource m_MyAudioSource1;

    [Header("Switching Sound")]
    public AudioSource m_MyAudioSource2;

    [Header("Painting Range")]
    public float range = 5f;



    void Awake()
    {
        //On Awake, turn off all painting image signs

        for (int j = 0; j < sign.Length; j++)
        {
            sign[j].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // for Quiting application
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }

        // for changing painting image prepab. Also change the active status of signs on screen, if there are multiple signs, use
        // loop to deactive all image prepfab sign and only active the current selectedimage sign
        // the key 'q' is used to 
        if (Input.GetKeyDown("q"))
        {
            for (int j = 0; j < sign.Length; j++)
            {
                sign[j].SetActive(false);
            }

            m_MyAudioSource2.Play();
            selectedImage = image[i];
            sign[i].SetActive(true);

            if (i < image.Length)
            {
                i++;
            }

            if (i >= image.Length)
            {
                i = 0;
            }

        }


        //The following section use raycast the point to the front of the player to detect painting surface, when raycast hit and click mouse button
        //It will instantiate a image prefab at the hit point location and pointing to the same direction player is facing.

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, range, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");
            //Debug.Log("HitPoint is" + hit.point);
            //Debug.Log("HitPoint normal is" + hit.normal);

            //Debug.Log("C.Rotation" + main.transform.rotation.x + ", " + main.transform.rotation.y + ", " + main.transform.rotation.z);
            //Debug.Log("C.Rotation Euler" + Quaternion.Euler(main.transform.rotation.x, main.transform.rotation.y, main.transform.rotation.z));

            if (Input.GetMouseButtonDown(0))
            {
                // Make a new Quaternion rotation according to hit.normal, this make the new image in next step attach to hit point and parallel
                Quaternion rotation = Quaternion.FromToRotation((new Vector3(0, 1, 0)), hit.normal);

                // Instantiate new image , that is "paint" on surface
                GameObject a = Instantiate(selectedImage, hit.point, rotation);

                // Rotate newly instantiated image to player's direction, or maybe in VR, the controller's direction.
                a.transform.Rotate(0, main.transform.eulerAngles.y, 0, Space.Self);


                // Make spray paint sound
                if (!m_MyAudioSource1.isPlaying)
                {
                    m_MyAudioSource1.Play();
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
            
        }
    }
}
