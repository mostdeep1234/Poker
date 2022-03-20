using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public static GameControl gameControl;

    public enum GameState
    {
        Start, Play, End
    };

    public enum PlayerTurn
    {
        player1, player2, player3, player4
    };

    public Transform player1Pos;

    public Transform player2Pos;

    public Transform player3Pos;

    public Transform player4Pos;

    public GameObject cardDeckSpawn;

    public List<GameObject> listOfDeckCardSpawn = new List<GameObject>();

    public List<Card> player1cards = new List<Card>();

    public List<Card> player2cards = new List<Card>();

    public List<Card> player3cards = new List<Card>();

    public List<Card> player4cards = new List<Card>();

    public int threeCardsPlayer1;

    public int threeCardsPlayer2;

    public int threeCardsPlayer3;

    public int threeCardsPlayer4;

    public List<int> cardsPlayerThreeList = new List<int>();

    public List<Card> cardPlayerThreeListsCard = new List<Card>();

    public List<Card> throwedCard = new List<Card>();

    public List<Animator> charAnimators = new List<Animator>();

    public List<Text> cardOnHandsPlayers = new List<Text>();

    public Text winNotice;

    public Transform cardDeckPos;

    public bool spawnCard;

    public bool divideCard;

    public bool dealingCondition;

    public bool playStart;

    public bool aiCheckTurnReady;

    public int scoreCardAnnualy = 2;

    public GameState gameState;

    public PlayerTurn playerTurn;

    /// <summary>
    /// //////////////////////////////////////////////////////
    /// </summary>

    public const string SPADES = "spades";

    public const string HEARTS = "hearts";

    public const string CLUBS = "clubs";

    public const string DIAMONDS = "diamond";

    public static bool flusheOnThrow;

    public static bool straightOnThrow;

    public static bool threeOfAKindOnThrow;

    public static bool fourOfAKindOnThrow;

    public static bool twoPairOnThrow;

    public static bool onePairOnThrow;

    public static bool fullHouseOnThrow;

    public int tempId;

    public static bool someoneWillWin;

    public bool gameStart; 

    private void Awake()
    {
        gameControl = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameStateManager();

        SetCardOnHand();

    }

    private void LateUpdate()
    {
        AnimationManager();
    }

    /// <summary>
    /// manage for the game state manager
    /// </summary>
    public void GameStateManager ()
    {
        switch(gameState)
        {
            case GameState.Start:
                SpawnCard();
                break;
            case GameState.Play:
                DivideCard();
                PlayManager();
                break;
            case GameState.End:
                WhoWinner();
                break;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(0);

        return;
    }

    /// <summary>
    /// set card on hand
    /// </summary>
    public void SetCardOnHand ()
    {
        cardOnHandsPlayers[0].text = player1Pos.childCount.ToString();

        cardOnHandsPlayers[1].text = player2Pos.childCount.ToString();

        cardOnHandsPlayers[2].text = player3Pos.childCount.ToString();

        cardOnHandsPlayers[3].text = player4Pos.childCount.ToString();

        return;
    }

    /// <summary>
    /// manage to spawn the card
    /// </summary>
    public void SpawnCard ()
    {
        if(spawnCard == false)
        {
            spawnCard = true;

            StartCoroutine(SpawnCardOperation());
        }

        return;
    }

    /// <summary>
    /// set the card label
    /// contains spades and etc
    /// </summary>
    /// <param name="id"></param>
    /// <param name="card"></param>
    public void SetCardLabel (int id, Card card)
    {
        switch(id)
        {
            case 0:
                card.cardLabel = HEARTS;
                break;
            case 1:
                card.cardLabel = CLUBS;
                break;
            case 2:
                card.cardLabel = DIAMONDS;
                break;
            case 3:
                card.cardLabel = SPADES;
                break;
        }

        return;
    }

    /// <summary>
    /// manage to spawn card operation selections
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnCardOperation ()
    {
        for (int i = 0; i < 52; i++)
        {
            GameObject cardObj = (GameObject)Instantiate(cardDeckSpawn);

            Card cardObjCard = cardObj.GetComponent<Card>();

            SetCardLabel(tempId, cardObjCard);

            ////////////////////////////////////////////////////////////////////////////////////
            ///
            if (scoreCardAnnualy < 14)
            {
                cardObjCard.cardId = scoreCardAnnualy;

                scoreCardAnnualy++;
            }
            else if (scoreCardAnnualy >= 14)
            {
                cardObjCard.cardId = scoreCardAnnualy;

                SetCardLabel(tempId, cardObjCard);

                tempId++;

                scoreCardAnnualy = 2;
            }

            //////////////////////////////////////////////////////////////////////////////////////

            cardObj.transform.SetParent(cardDeckPos);

            cardObj.transform.localPosition = new Vector3(0, 0, 0);
           
            cardObjCard.spriteRenderer.sprite = cardObjCard.cardSprites[i];

            listOfDeckCardSpawn.Add(cardObj);
        }

        FischerYatesCardShuffle(listOfDeckCardSpawn);

        cardDeckPos.transform.DetachChildren();

        for(int x = 0; x < listOfDeckCardSpawn.Count; x++)
        {
            listOfDeckCardSpawn[x].transform.SetParent(cardDeckPos);

            listOfDeckCardSpawn[x].transform.localPosition = Vector3.zero;
        }

        yield return new WaitForEndOfFrame();

        gameState = GameState.Play;

        yield break;
    }

    /// <summary>
    /// manage the fischer yates card mixing
    /// </summary>
    public void FischerYatesCardShuffle<T> (IList<T> cardLists)
    {
        System.Random random = new System.Random();

        int n = cardLists.Count;

        while(n > 1)
        {
            n--;

            int kN = random.Next(n);

            T type = cardLists[kN];

            cardLists[kN] = cardLists[n];

            cardLists[n] = type;
        }

        return;
    }

    /// <summary>
    /// manage for divide card selections
    /// </summary>
    public void DivideCard ()
    {
        if(divideCard == false)
        {
            divideCard = true;

            StartCoroutine(DivideACardOperation());
        }

        return;
    }

    /// <summary>
    /// manage to divide card operation
    /// and select to divide for the card operation
    /// </summary>
    /// <returns></returns>
    IEnumerator DivideACardOperation()
    {
        //divide to player 1
        for (int i = 0; i < 13; i++)
        {
            Card card1 = listOfDeckCardSpawn[i].GetComponent<Card>();

            player1cards.Add(card1);
        }

        StartCoroutine(CardSync(player1cards, player1Pos));

        //divide to player 2
        for(int x = 13; x < 13 * 2; x++)
        {
            Card card2 = listOfDeckCardSpawn[x].GetComponent<Card>();

            player2cards.Add(card2);
        }

        StartCoroutine(CardSync(player2cards, player2Pos));

        //divide to player 3
        for (int y = 13 * 2; y < 13 * 3; y++)
        {
            Card card3 = listOfDeckCardSpawn[y].GetComponent<Card>();

            player3cards.Add(card3);
        }

        StartCoroutine(CardSync(player3cards, player3Pos));

        //divide to player 4
        for (int z = 13 * 3; z < 13 * 4; z++)
        {
            Card card4 = listOfDeckCardSpawn[z].GetComponent<Card>();

            player4cards.Add(card4);
        }

        StartCoroutine(CardSync(player4cards, player4Pos));

        listOfDeckCardSpawn.Clear();

        yield return new WaitUntil(() => cardDeckPos.transform.childCount == 0 && listOfDeckCardSpawn.Count == 0);

        cardDeckPos.GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitUntil(() => player4cards.Count == 13);

        StartCoroutine(DealingControl());

        yield break;
    }

    /// <summary>
    /// manage for the card sync lists
    /// on tyoe
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cardList"></param>
    /// <returns></returns>
    IEnumerator CardSync (List<Card> cardList, Transform cardPos)
    {
        yield return new WaitUntil(() => cardList.Count == 13);

        for(int i = 0; i < cardList.Count; i++)
        {
            cardList[i].transform.SetParent(cardPos);

            cardList[i].transform.localPosition = Vector3.zero;

            cardList[i].GetComponent<SpriteRenderer>().sortingOrder = 2;

            cardList[i].transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        //check if not equal to player 1
        if(!cardList.Equals(player1cards)) 
            StartCoroutine(FaceDownCard(cardList));

        yield break;
    }

    /// <summary>
    /// face down card other than player cards
    /// </summary>
    /// <param name="cardList"></param>
    /// <returns></returns> 
    IEnumerator FaceDownCard(List<Card> cardList)
    {
        for(int i = 0; i < cardList.Count; i++)
        {
            cardList[i].GetComponent<SpriteRenderer>().sortingOrder = 0;
        }

        yield break;
    }

    /// <summary>
    /// control who is the first
    /// by searching 3 card on each players
    /// </summary>
    public IEnumerator DealingControl()
    {
        for (int a = 0; a < player1cards.Count; a++)
        {
            if(player1cards[a].cardId == 3)
            {
                cardPlayerThreeListsCard.Add(player1cards[a]);

                player1cards[a].transform.SetParent(cardDeckPos);

                player1cards[a].transform.localPosition = Vector3.zero;

                threeCardsPlayer1++;
            }
        }

        cardsPlayerThreeList.Add(threeCardsPlayer1);

        for(int b = 0; b < player2cards.Count; b++)
        {
            if(player2cards[b].cardId == 3)
            {
                cardPlayerThreeListsCard.Add(player2cards[b]);

                player2cards[b].transform.SetParent(cardDeckPos);

                player2cards[b].transform.localPosition = Vector3.zero;
                
                threeCardsPlayer2++;
            }
        }

        cardsPlayerThreeList.Add(threeCardsPlayer2);

        for(int c = 0; c < player3cards.Count; c++)
        {
            if(player3cards[c].cardId == 3)
            {
                cardPlayerThreeListsCard.Add(player3cards[c]);

                player3cards[c].transform.SetParent(cardDeckPos);

                player3cards[c].transform.localPosition = Vector3.zero;

                threeCardsPlayer3++;
            }
        }

        cardsPlayerThreeList.Add(threeCardsPlayer3);

        for(int d = 0; d < player4cards.Count; d++)
        {
            if(player4cards[d].cardId == 3)
            {
                cardPlayerThreeListsCard.Add(player4cards[d]);

                player4cards[d].transform.SetParent(cardDeckPos);

                player4cards[d].transform.localPosition = Vector3.zero;

                threeCardsPlayer4++; 
            }
        }

        cardsPlayerThreeList.Add(threeCardsPlayer4);

        yield return new WaitUntil(() => cardsPlayerThreeList.Count == 4);

        //if total 3 is same
        if (Helper.AreAllSame(cardsPlayerThreeList))
        {
            for(int i = 0; i < cardPlayerThreeListsCard.Count; i++)
            {
                if(cardPlayerThreeListsCard[i].cardLabel == SPADES)
                {
                    switch(i)
                    {
                        case 0:
                            playerTurn = PlayerTurn.player1;
                            break;
                        case 1:
                            playerTurn = PlayerTurn.player2;
                            break;
                        case 2:
                            playerTurn = PlayerTurn.player3;
                            break;
                        case 3:
                            playerTurn = PlayerTurn.player4;
                            break;
                    }
                }
            }
        }

        //if not
        else
        {
            int max = FindMax(cardsPlayerThreeList);

            for (int i = 0; i < cardsPlayerThreeList.Count; i++)
            {
                if (cardsPlayerThreeList[i] == max)
                {
                    switch (i)
                    {
                        case 0:
                            playerTurn = PlayerTurn.player1;
                            break;
                        case 1:
                            playerTurn = PlayerTurn.player2;
                            break;
                        case 2:
                            playerTurn = PlayerTurn.player3;
                            break;
                        case 3:
                            playerTurn = PlayerTurn.player4;
                            break;
                    }
                }
            }
        }

        playStart = true;

        yield break;
    }

    /// <summary>
    /// manage to find max
    /// </summary>
    /// <param name="listFind"></param>
    public int FindMax (List<int> listFind)
    {
        int maxValue = listFind.Max();

        return maxValue;
    }

    /// <summary>
    /// manage to play the play manager
    /// and instancly using playing to manage for the play manager
    /// </summary>
    public void PlayManager ()
    {
        if(playStart)
        {
            playStart = false;

            player1Pos.GetComponent<SpriteRenderer>().enabled = false;

            // spread the card for player 1
            float x = player1Pos.transform.position.x - 3.0f;
            float y = player1Pos.transform.position.y;
            float z = player1Pos.transform.position.z;
            player1Pos.transform.position = new Vector3(x, y, z);

            for(int i = 0; i < player1cards.Count; i++)
            {
                Transform playerCardTrans = player1cards[i].transform;

                float xCard = (playerCardTrans.position.x + 0.8f * i) - 0.3f;

                float yCard = playerCardTrans.position.y;

                float zCard = playerCardTrans.position.z;

                player1cards[i].transform.position = new Vector3(xCard, yCard, zCard); 
            }

            for(int xL = 0; xL < cardPlayerThreeListsCard.Count; xL++)
            {
                cardPlayerThreeListsCard[xL].transform.localPosition = Vector3.zero; 
            }

            for(int i = 0; i < cardDeckPos.childCount; i++)
            {
                cardDeckPos.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
            }

            cardDeckPos.DetachChildren();

            //switch turn operation
            switch (playerTurn)
            {
                case PlayerTurn.player1:
                    player1Pos.GetComponent<Player>().enabled = true;
                    player2Pos.GetComponent<AI>().enabled = false;
                    player3Pos.GetComponent<AI>().enabled = false;
                    player4Pos.GetComponent<AI>().enabled = false;
                    break;
                case PlayerTurn.player2:
                    player1Pos.GetComponent<Player>().enabled = false;
                    player2Pos.GetComponent<AI>().enabled = true;
                    player3Pos.GetComponent<AI>().enabled = false;
                    player4Pos.GetComponent<AI>().enabled = false;
                    break;
                case PlayerTurn.player3:
                    player1Pos.GetComponent<Player>().enabled = false;
                    player2Pos.GetComponent<AI>().enabled = false;
                    player3Pos.GetComponent<AI>().enabled = true;
                    player4Pos.GetComponent<AI>().enabled = false;
                    break;
                case PlayerTurn.player4:
                    player1Pos.GetComponent<Player>().enabled = false;
                    player2Pos.GetComponent<AI>().enabled = false;
                    player3Pos.GetComponent<AI>().enabled = false;
                    player4Pos.GetComponent<AI>().enabled = true;
                    break;
            }

            gameStart = true;
        }
        
        return;
    }

    /// <summary>
    /// manage for the animation manager instance
    /// </summary>
    public void AnimationManager ()
    {
        //switch turn operation
        switch (playerTurn)
        {
            case PlayerTurn.player1:
                charAnimators[1].GetComponent<Image>().color = new Color(255, 255, 255, 255);
                charAnimators[2].GetComponent<Image>().color = new Color(255, 255, 255, 255);
                charAnimators[3].GetComponent<Image>().color = new Color(255, 255, 255, 255);
                charAnimators[0].enabled = true;
                charAnimators[1].enabled = false;
                charAnimators[2].enabled = false;
                charAnimators[3].enabled = false;
                break;
            case PlayerTurn.player2:
                charAnimators[0].enabled = false;
                charAnimators[1].enabled = true;
                charAnimators[2].enabled = false;
                charAnimators[3].enabled = false;
                charAnimators[0].GetComponent<Image>().color = new Color(255, 255, 255, 255);
                charAnimators[2].GetComponent<Image>().color = new Color(255, 255, 255, 255);
                charAnimators[3].GetComponent<Image>().color = new Color(255, 255, 255, 255);
                break;
            case PlayerTurn.player3:
                charAnimators[0].enabled = false;
                charAnimators[1].enabled = false;
                charAnimators[2].enabled = true;
                charAnimators[3].enabled = false; 
                charAnimators[0].GetComponent<Image>().color = new Color(255, 255, 255, 255);
                charAnimators[1].GetComponent<Image>().color = new Color(255, 255, 255, 255);
                charAnimators[3].GetComponent<Image>().color = new Color(255, 255, 255, 255);
                break;
            case PlayerTurn.player4:
                charAnimators[0].enabled = false;
                charAnimators[1].enabled = false;
                charAnimators[2].enabled = false;
                charAnimators[3].enabled = true;
                charAnimators[0].GetComponent<Image>().color = new Color(255, 255, 255, 255);
                charAnimators[1].GetComponent<Image>().color = new Color(255, 255, 255, 255);
                charAnimators[2].GetComponent<Image>().color = new Color(255, 255, 255, 255);
                break;
        }

        return;
    }

    /// <summary>
    /// manage for the card deck pos
    /// </summary>
    public int CardThrowTotal ()
    {
       return cardDeckPos.transform.childCount;
    }

    /// <summary>
    /// manage who is the winner
    /// </summary>
    public void WhoWinner ()
    {
        if(player1Pos.transform.childCount <= 0)
        {
            print("PLAYER 1 WIN");

            winNotice.text = "player 1 win, press enter to play again";
        }

        if(player2Pos.transform.childCount <= 0)
        {
            print("PLAYER 2 WIN");

            winNotice.text = "player 2 win, press enter to play again";
        }

        if (player3Pos.transform.childCount <= 0)
        {
            print("PLAYER 3 WIN");

            winNotice.text = "player 3 win, press enter to play again";
        }

        if(player4Pos.transform.childCount <= 0)
        {
            print("PLAYER 4 WIN");

            winNotice.text = "player 4 win, press enter to play again";
        }

        if (Input.GetKeyDown(KeyCode.Return))
            SceneManager.LoadScene(0);
        return;
    }
}
