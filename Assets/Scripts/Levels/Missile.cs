using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missile : MonoBehaviour
{
    [HideInInspector] public LevelMissiles levelMissiles;
    [HideInInspector] public MissilePooler missilePooler;

    [HideInInspector] public float indicatorSize;
    [HideInInspector] public float time;

    [SerializeField] AnimationCurve indicatorCurve;
    Image radiusIndicator;

    private void Awake()
    {
        radiusIndicator = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        var color = radiusIndicator.color;
        time += Time.deltaTime;
        if (time < levelMissiles.missileSpeed)
        {
            //descrease the indicator transparency until missile lands
            color.a = indicatorCurve.Evaluate(time / levelMissiles.missileSpeed) * levelMissiles.missileEndTransparency;
            radiusIndicator.color = color;

            //increase the indicator scale until missile lands
            indicatorSize = indicatorCurve.Evaluate(time / levelMissiles.missileSpeed) * levelMissiles.missileEndScale;
            radiusIndicator.rectTransform.sizeDelta = new Vector2(indicatorSize, indicatorSize);
        }
        else
        {
            //this missile has landed, damage all nearby players
            Collider[] colliders = Physics.OverlapSphere(transform.position, levelMissiles.missileRadius);
            foreach (Collider col in colliders)
            {
                if (col.tag == ObjectTags.Player)
                {
                    col.GetComponent<PlayerInventory>().DealDamage(levelMissiles.missileDamage, transform.position);
                }
                else if (col.tag == ObjectTags.Wall || col.tag == ObjectTags.BreakableWall)
                {
                    Destroy(col.gameObject);
                }
                else if (col.tag == ObjectTags.Chest)
                {
                    FindObjectOfType<ChestPooler>().ReleaseChestInstance(col.gameObject);
                }
            }

            missilePooler.ReleaseMissileInstance(gameObject);
        }
    }
}
