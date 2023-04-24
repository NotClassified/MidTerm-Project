using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missile : MonoBehaviour
{
    [HideInInspector] public float missileSpeed;
    [HideInInspector] public float missileEndScale;
    [HideInInspector] public float missileEndTransparency;
    [HideInInspector] public float missileRadius;
    [HideInInspector] public int missileDamage;
    [SerializeField] AnimationCurve indicatorCurve;
    Image radiusIndicator;
    float indicatorSize;
    float time;
    private void Awake()
    {
        radiusIndicator = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        var color = radiusIndicator.color;
        time += Time.deltaTime;
        if (time < missileSpeed)
        {
            //descrease the indicator transparency until missile lands
            color.a = indicatorCurve.Evaluate(time / missileSpeed) * missileEndTransparency;
            radiusIndicator.color = color;

            //increase the indicator scale until missile lands
            indicatorSize = indicatorCurve.Evaluate(time / missileSpeed) * missileEndScale;
            radiusIndicator.rectTransform.sizeDelta = new Vector2(indicatorSize, indicatorSize);
        }
        else
        {
            //this missile has landed, damage all nearby players
            Collider[] colliders = Physics.OverlapSphere(transform.position, missileRadius);
            foreach (Collider col in colliders)
            {
                if (col.tag == ObjectTags.Player)
                {
                    col.GetComponent<PlayerInventory>().DealDamage(missileDamage);
                }
            }

            Destroy(gameObject);
        }
    }
}
