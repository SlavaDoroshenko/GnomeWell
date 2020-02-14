using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BodyPart : MonoBehaviour
{
    public Sprite detachedSprite;
    public Sprite burnedSprite;

    public Transform bloodFontainOrigin;

    bool detached = false;

    public void Detached()
    {
        detached = true;

        this.tag = "Untagged";

        transform.SetParent(null, true);
    }

    private void Update()
    {
        if (!detached)
        {
            return;
        }

        var rigidbody = GetComponent<Rigidbody2D>();

        if (rigidbody.IsSleeping())
        {
            foreach (Joint2D joint in GetComponents<Joint2D>())
            {
                Destroy(joint);
            }

            foreach (Rigidbody2D rigidbody1 in GetComponents<Rigidbody2D>())
            {
                Destroy(rigidbody1);
            }

            foreach (Collider2D collider in GetComponents<Collider2D>())
            {
                Destroy(collider);
            }

            Destroy(this);
        }
    }

    public void ApplyDamageSprite(Gnome.DamageType damageType)
    {
        Sprite spriteToUse = null;

        switch (damageType) {
            case Gnome.DamageType.Burning: spriteToUse = burnedSprite;
            break;
            case Gnome.DamageType.Slicing: spriteToUse = detachedSprite;
            break;
        }

        if (spriteToUse != null)
        {
            GetComponent<SpriteRenderer>().sprite = spriteToUse;
        }
    }
}
