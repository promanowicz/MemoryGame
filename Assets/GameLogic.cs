using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour{
    public Text scoreTextView;
    public Text timeRemainingTextView;

    //Rozmiar gry, wyrażony w ilości kart/2
    public int initialSize = 2;
    public int maxSize = 8;
    public int gameTime = 90;
    private Card card1 = null;

    public GameObject finalScoreLabelText;
    public GameObject finalScoreTextGO;
    public GameObject buttonReset;
    public Text finalScoreText;

    public void cardTapped(Card card){
        if (card1 == null){
            card1 = card;
        }
        else{
            StartCoroutine(hideCardsAfterSec(card1, card, 1));
        }
    }

    private IEnumerator hideCardsAfterSec(Card c1, Card c2, int sec){
        yield return new WaitForSeconds(sec);
        if (c1.cardId == c2.cardId){
            cardsids.Remove(c1.cardId);
            c1.destroy();
            c2.destroy();
            addPoints();
            if (cardsids.Count == 0){
                if (currSize < maxSize){
                    currSize += 1;
                    instantiateGame(currSize);
                }
                else{
                    showEndScore();
                }
            }
        }
        else{
            c1.makeCardIInVisible();
            c2.makeCardIInVisible();
            card1 = null;
        }
    }

    public Camera cam;
    private int currentPoints = 0;
    private string pointsText = "Points: ";
    private float timeRemaining = 90;
    private string timeText = "Time: ";

    void Start(){
        timeRemaining = gameTime;
        transform.position = cam.ScreenToWorldPoint(
            new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));
        instantiateGame(initialSize);
    }

    // Update is called once per frame
    void Update(){
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0){
            timeRemaining = 0;
            showEndScore();
        }
        timeRemainingTextView.text = timeText + ((int) timeRemaining);
    }

    private void addPoints(){
        if (!isGameEnded){
            currentPoints += 100;
            scoreTextView.text = pointsText + currentPoints;
        }
    }


    public GameObject mockCard;
    public Sprite[] cardSpritesList;
    private List<GameObject> cards = new List<GameObject>();
    private List<int> cardsids = new List<int>();
    private int currSize = -1;

    private void instantiateGame(int size){
        if (size >= maxSize){
            showEndScore();
            return;
        }
        else{
            hideEndScore();
        }
        foreach (var go in cards){
            Destroy(go);
        }
        currSize = size;
        cards.Clear();
        for (int i = 0; i < currSize && i < maxSize; i++){
            GameObject obj = Instantiate(mockCard);
            obj.SetActive(true);
            Card objCard = obj.GetComponent<Card>();
            objCard.cardId = i;
            objCard.setCardSprite(cardSpritesList[i]);

            cardsids.Add(i);
            cards.Add(obj);
            cards.Add(Instantiate(obj));
        }
        shuffleList();
        applyPositionToCards();
    }

    private void shuffleList(){
        List<GameObject> tmpList = new List<GameObject>(cards.Count);
        int elements = cards.Count;
        for (int i = 0; i < elements; i++){
            int pos = Random.Range(0, cards.Count);
            tmpList.Add(cards[pos]);
            cards.RemoveAt(pos);
        }
        cards = tmpList;
    }

    private const int maxCardsInRow = 4;

    private void applyPositionToCards(){
        int cardsCount = cards.Count;
        if (cardsCount == 0) return;
        float xsize = cards[0].GetComponent<Card>().getSpritesWidth();
        float ysize = cards[0].GetComponent<Card>().getSpritesHeight();

        int rows = cardsCount / maxCardsInRow + 1;


        float posX = -xsize * 1.5f;
        float posY = ysize * ((rows - 1) / 2) - ysize/2;

        for (int i = 0; i < cards.Count; i++){
            if (i != 0 && i % maxCardsInRow == 0){
                posX = -xsize * 1.5f;
                posY -= ysize;
            }
            cards[i].transform.position = new Vector3(transform.position.x + posX, transform.position.y + posY, 0);
            posX += xsize;
        }
    }

    private bool isGameEnded = false;

    private void showEndScore(){
        isGameEnded = true;
        finalScoreTextGO.SetActive(true);
        finalScoreLabelText.SetActive(true);
        finalScoreText.text = "Score: " + currentPoints;
        buttonReset.SetActive(true);
    }

    private void hideEndScore(){
        isGameEnded = false;
        finalScoreTextGO.SetActive(false);
        finalScoreLabelText.SetActive(false);
        buttonReset.SetActive(false);
    }

    public void restartGame(){
        finalScoreTextGO.SetActive(false);
        finalScoreLabelText.SetActive(false);
        buttonReset.SetActive(false);
        currentPoints = 0;
        timeRemaining = gameTime;
        instantiateGame(initialSize);
    }
}
