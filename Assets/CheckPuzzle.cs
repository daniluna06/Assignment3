using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;
using Oculus.Interaction;
using Oculus.Interaction.Collections;
public class CheckPuzzle : MonoBehaviour
{
    // Target puzzle goal
    public Transform targetPuzzle;

    // Boolean variable to indicate whether this puzzle is solved or not
    public bool isSolved;

    // Read game object components
    private AudioSource audioSource;
    private Rigidbody rigidbody;
    private BoxCollider boxCollider;
    void Start()
    {
        // By using 'GetComponent<>()' function, we can read a component of this game object
        // The below lines read audio source, rigid body, and box collider
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // We can calculate the distance between points in 3D space by using Vector3.Distance(Vector3 A, Vector3 B) function
        float distance = Vector3.Distance(transform.position, targetPuzzle.position);

        // If the distance between this puzzle piece and puzzle goal is less than 0.05f and it is the first time to detect it
        if (distance < 0.05f && !isSolved)
        {
            // these two lines force the controller to release the grabbed puzzle piece 
            // the first line accesses the first child game object which has 'GrabInteractable' script and read one of the script variable, 'Interactors'
            // the second line checks each interactable variable in 'Interactors' and run Unselect()
            // unselect() is the function to force the controller to release the grabbed puzzle piece
            IEnumerable<GrabInteractor> setInteractors = transform.GetChild(0).GetComponent<GrabInteractable>().Interactors;
            foreach (GrabInteractor interactor in setInteractors) { interactor.Unselect(); }

            // to place the puzzle piece with the right position and orientation
            // we make this game object have same position and rotation value with the target puzzle goal
            transform.SetPositionAndRotation(targetPuzzle.position, targetPuzzle.rotation);
            targetPuzzle.gameObject.SetActive(false);

            // to fix it on the position, we set the rigidbody.freeze both position and rotation
            // it makes the virutal object keep position and rotation, regardless of physics calculation
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;

            // to disable to grab again, we turn off the box collider
            boxCollider.enabled = false;

            // for our sound effect, we play an audio clip
            audioSource.Play();

            // to run the above lines only one time, we set 'isSolve' true
            isSolved = true;

        }
    }
}
