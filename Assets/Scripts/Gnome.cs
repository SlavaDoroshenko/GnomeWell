using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnome : MonoBehaviour
{
    public Transform cameraFollowTarget;

    public Rigidbody2D ropeBody;

    public Sprite armHoldingEmpty;
    public Sprite armHoldingTreasure;

    public SpriteRenderer holdingArm;

    public GameObject deathPrefab;
    public GameObject flameDeatPrefab;
    public GameObject ghostPrefab;

    public float delayBeforeRemoving = 3.0f;
    public float delayBeforeReleasingGost = 0.25f;

    public GameObject bloodFontainPrefab;

    bool dead = false;

    bool _holdingTreasure = false;

    public bool holdingTreasure
    {
        get
        {
            return _holdingTreasure;
        }

        set
        {
            if (dead == true)
            {
                return;
            }

            _holdingTreasure = value;

            if (holdingArm != null)
            {
                if (_holdingTreasure)
                {
                    holdingArm.sprite = armHoldingTreasure;
                }
                else
                {
                    holdingArm.sprite = armHoldingEmpty;
                }
            }
        }
    }

    public enum DamageType
    {
        Slicing,
        Burning
    }

    public void ShowDamageEffect(DamageType type)
    {
        switch (type)
        {
            case DamageType.Burning:
                if (flameDeatPrefab != null)
                {
                    Instantiate(flameDeatPrefab, cameraFollowTarget.position, cameraFollowTarget.rotation);
                }
                break;

            case DamageType.Slicing:
                if (deathPrefab != null)
                {
                    Instantiate(deathPrefab, cameraFollowTarget.position, cameraFollowTarget.rotation);
                }
                break;
        }
    }

    public void DestroyGnome(DamageType type)
    {
        holdingTreasure = false;

        dead = true;

        foreach (BodyPart part in GetComponentsInChildren<BodyPart>())
        {
            switch (type)
            {
                case DamageType.Burning:
                    bool shouldBurn = Random.Range(0, 2) == 0;
                    if (shouldBurn)
                    {
                        part.ApplyDamageSprite(type);
                    }
                    break;

                case DamageType.Slicing:
                    part.ApplyDamageSprite(type);
                    break;
            }

            bool shouldDetach = Random.Range(0, 2) == 0;

            if (shouldDetach)
            {
                part.Detached();
            }

            if (part.bloodFontainOrigin != null && bloodFontainPrefab != null)
            {
                GameObject fountain = Instantiate(bloodFontainPrefab, part.bloodFontainOrigin.position, part.bloodFontainOrigin.rotation) as GameObject;

                fountain.transform.SetParent(this.cameraFollowTarget, false);
            }

            var allJoints = part.GetComponentsInChildren<Joint2D>();
            foreach (Joint2D joint in allJoints)
            {
                Destroy(joint);
            }
        }

        var remove = gameObject.AddComponent<RemoveAfterDalay>();
        remove.delay = delayBeforeRemoving;

        StartCoroutine(ReleaseGhost());
    }

    IEnumerator ReleaseGhost()
    {
        if (ghostPrefab == null)
        {
            yield break;
        }

        yield return new WaitForSeconds(delayBeforeReleasingGost);

        Instantiate(ghostPrefab, transform.position, Quaternion.identity);
    }
}
