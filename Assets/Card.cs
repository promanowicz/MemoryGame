using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour{

    public int cardId;

    public GameObject cover;

    public GameObject insideCard;

    public GameLogic gameLogic;

    private bool isCardVisible = false;

    private void OnMouseDown(){
        makeCardVisible();
        gameLogic.cardTapped(this);
    }

    private void makeCardVisible(){
        isCardVisible = true;
        cover.SetActive(!isCardVisible);
    }

    public void makeCardIInVisible(){
        isCardVisible = false;
        cover.SetActive(!isCardVisible);
    }

    public bool isVisible(){
        return isCardVisible;
    }

    public void destroy(){
        Destroy(gameObject);
    }

    public void setCardSprite(Sprite spr){
        insideCard.GetComponent<SpriteRenderer>().sprite = spr;
    }

    public float getSpritesWidth(){
        return cover.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    public float getSpritesHeight(){
        return cover.GetComponent<SpriteRenderer>().bounds.size.y;
    }
}
