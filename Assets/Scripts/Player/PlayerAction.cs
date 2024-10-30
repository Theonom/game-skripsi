using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    //For Anim Down
    public void Down()
    {
        anim.SetBool("Down", true);
        Player.playerDown = true;
    }

    public void PlayerAttackFalse()
    {
        Player.playerAttack = false;
    }

    public void PlaySfx(string nameSfx)
    {
        AudioManager.instance.PlaySfx(nameSfx);
    }
}
