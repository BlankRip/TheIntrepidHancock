using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitScript : MonoBehaviour
{
    [SerializeField] GameObject lockMovement;

    private void OnTriggerEnter(Collider other)
    {
        AudioManger.instance.ExitDungeonClip();
        lockMovement.SetActive(true);
        StartCoroutine(Exit());
    }

    IEnumerator Exit()
    {
        yield return new WaitForSeconds(4.7f);
        SceneShifter.LoadScene("Victory");
    }
}
