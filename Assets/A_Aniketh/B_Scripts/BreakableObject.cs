using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    ScoreScript score_relic;
    [SerializeField] GameObject fractureVersion;
    [SerializeField] GameObject relic;
    [SerializeField] int scoreToAdd;
    [SerializeField] int hitsBeforeBreak = 2;
    [Range(1, 2)] [SerializeField] float increaseSizeBy = 1.1f;
    [Range(0, 0.5f)] [SerializeField] float maxMovePosition = 0.1f;
    Vector3 initialSize;
    Vector3 initialPosition;
    Vector3 addPosition;
    float movePosition;
    int hitsTaken;

    void Start()
    {
        hitsTaken = 0;
        initialSize = transform.localScale;
        initialPosition = transform.position;
        movePosition = Random.Range(0, maxMovePosition);
        addPosition = new Vector3(movePosition, 0, -movePosition);
        score_relic = FindObjectOfType<ScoreScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon")
        {
            Debug.Log("<color=red>Hitting</color>");
            if (hitsTaken <= hitsBeforeBreak)
            {
                hitsTaken++;
                transform.localScale = initialSize * increaseSizeBy;
                transform.position = initialPosition + addPosition;
                StartCoroutine(BackToOrigial());
            }

            if (hitsTaken > hitsBeforeBreak)
            {
                score_relic.currentScore += scoreToAdd;
                Instantiate(fractureVersion, transform.position, transform.rotation);
                if (score_relic.spawnRelic)
                {
                    Debug.Log("<color=blue> Here Spawn Relic</color>");
                    score_relic.spawnRelic = false;
                }
                Destroy(gameObject);
            }
        }
    }

    IEnumerator BackToOrigial()
    {
        yield return new WaitForSeconds(0.1f);
        transform.localScale = initialSize;
        transform.position = initialPosition;
    }
}
