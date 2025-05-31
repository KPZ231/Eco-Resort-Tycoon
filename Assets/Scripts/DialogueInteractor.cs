using UnityEngine;

public class DialogueInteractor : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
            {
                Dialogue dialogue = hit.collider.GetComponent<Dialogue>();
                if (dialogue != null)
                {
                    dialogue.StartDialogue();
                }
            }
        }
    }
}