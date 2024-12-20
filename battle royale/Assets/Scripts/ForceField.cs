using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    public float shrinkWaitTime;
    public float shrinkAmount;
    public float shrinkDuration;
    public float minShrinkAmount;
    public int playerDamage;
    private float lastShrinkEndTime;
    private bool shrinking;
    private float targetDiameter;
    private float lastPlayerCheckTime;

    void Shrink ()
    {
        shrinking = true;
        // make sure we don't shrink below the min amount
        if(transform.localScale.x - shrinkAmount > minShrinkAmount)
            targetDiameter -= shrinkAmount;
        else
            targetDiameter = minShrinkAmount;
        lastShrinkEndTime = Time.time + shrinkDuration;
}

    // Start is called before the first frame update
    void Start()
    {
        lastShrinkEndTime = Time.time;
        targetDiameter = transform.localScale.x;
    }

    void CheckPlayers ()
    {
        if(Time.time - lastPlayerCheckTime > 1.0f)
        {
            lastPlayerCheckTime = Time.time;
            // loop through all players
            foreach(PlayerController player in GameManager.instance.players)
            {
                if(player.dead || !player)
                    continue;
                if(Vector3.Distance(Vector3.zero, player.transform.position) >= transform.localScale.x)
                {
                    player.photonView.RPC("TakeDamage", player.photonPlayer, 0, playerDamage);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayers();
        if(shrinking)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one* targetDiameter, (shrinkAmount / shrinkDuration) * Time.deltaTime);
            if(transform.localScale.x == targetDiameter)
                shrinking = false;
        }
        else
        {
            // can we shrink again?
            if(Time.time - lastShrinkEndTime >= shrinkWaitTime && transform.localScale.x > minShrinkAmount)
                Shrink();
        }
    }
}
