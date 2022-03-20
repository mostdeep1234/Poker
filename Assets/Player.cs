using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player player;

    public List<Card> cardInHand = new List<Card>();

    public List<AI> aiManager = new List<AI>();

    public List<Card> throwCard = new List<Card>();

    public List<Card> sortThrowCard = new List<Card>();

    public List<int> scoreCardHands = new List<int>();

    public Card holdCard;

    public bool turnCheck;

    public bool firstTurn = true;

    public Image avatarImage;

    public List<Sprite> sprites = new List<Sprite>();

    public List<AI> AI = new List<AI>();

    public int scoreFirstIndex;

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    public bool onePair;

    public bool twoPair;

    public bool threeOfAKind;

    public bool flush;

    public bool straight;

    public bool fourOfAKind;


    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    public int onePairFirstCountSystem;

    public int idSameRankFirstCountOnePair;

    public int idSameRankSecondOnePair;

    public int idSameRankThirdOnePair;

    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////.
    /// </summary>

    public int twoPairFirstCountSystem;

    public int twoPairSecondCountSystem;

    public int idSameRankTwoPair;

    public int idSameRankSecondTwoPair;

    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    public int threeOfAkindSystemCount;

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    public int straightSystemCount;

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>

    public int spadesCount;

    public int heartsCount;

    public int clubsCount;

    public int diamondsCount;

    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    public int fourOfAKindSystemCount;

    
    private void Awake()
    {
        player = this;
    }

    private void OnEnable()
    {
        turnCheck = true;
    }

    private void OnDisable()
    {
        turnCheck = false;

        ControlStateEnd();
    }

    // Start is called before the first frame update
    void Start()
    {
        AssignAvatarSprites();

        RegisterHand();
    }

    /// <summary>
    /// assign avatar sprites
    /// </summary>
    public void AssignAvatarSprites ()
    {
        switch(PlayerPrefs.GetString("avatar"))
        {
            case SelectCharacter.LISTZ:
                avatarImage.sprite = sprites[0];
                break;
            case SelectCharacter.KANEKO:
                avatarImage.sprite = sprites[1];
                break;
            case SelectCharacter.KARUMA:
                avatarImage.sprite = sprites[2]; 
                break;
        }

        return;
    }

    /// <summary>
    /// manage for the register hand
    /// and select for the child count to check
    /// </summary>
    public void RegisterHand ()
    {
        StartCoroutine(RegisterHandEvent());

        return;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator RegisterHandEvent ()
    {
        yield return new WaitUntil(() => this.transform.childCount > 0);

        for (int i = 0; i < this.transform.childCount; i++)
        {
            cardInHand.Add(this.transform.GetChild(0).GetComponent<Card>());
        }

        yield break;
    }

    /// <summary>
    /// register to throw card
    /// </summary>
    public void RegisterToThrowCard ()
    {
        if(turnCheck)
        {
            if(throwCard.Count > 0 && throwCard.Count <= 5)
            {
                if (GameControl.gameControl.cardDeckPos.transform.childCount <= 0)
                {
                    if (throwCard.Count <= 5 && throwCard.Count != 1)
                    {
                        //do nothing
                    }
                    else if (throwCard.Count <= 5 && throwCard.Count == 1)
                    {
                        Helper.DeleteThrowCard();

                        Helper.ThrowCard(throwCard, cardInHand);

                        Helper.StopAnimationOnCard(throwCard);

                        Helper.DisableThrowedCondition(AI);

                        throwCard.Clear();

                        this.GetComponent<Player>().enabled = false;

                        GameControl.gameControl.player2Pos.GetComponent<AI>().enabled = true;

                        GameControl.gameControl.playerTurn = GameControl.PlayerTurn.player2;
                    }
                }
                else if (GameControl.gameControl.cardDeckPos.transform.childCount == 1)
                {
                    if (throwCard.Count <= 5 && throwCard.Count == 1)
                    {
                        int score = GameControl.gameControl.cardDeckPos.transform.GetChild(0).GetComponent<Card>().ScoreCard;

                        int cardHandScore = throwCard[0].ScoreCard;

                        if (cardHandScore > score)
                        {
                            Helper.DeleteThrowCard();

                            Helper.ThrowCard(throwCard, cardInHand);

                            Helper.StopAnimationOnCard(throwCard);

                            Helper.DisableThrowedCondition(AI);

                            throwCard.Clear();

                            this.GetComponent<Player>().enabled = false;

                            GameControl.gameControl.player2Pos.GetComponent<AI>().enabled = true;

                            GameControl.gameControl.playerTurn = GameControl.PlayerTurn.player2;
                        }
                    }
                }
                else if (firstTurn)
                {
                    if (throwCard.Count <= 5 && throwCard.Count == 1)
                    {
                        Helper.DeleteThrowCard();

                        Helper.ThrowCard(throwCard, cardInHand);

                        Helper.StopAnimationOnCard(throwCard);

                        Helper.DisableThrowedCondition(AI);

                        throwCard.Clear();

                        this.GetComponent<Player>().enabled = false;

                        GameControl.gameControl.player2Pos.GetComponent<AI>().enabled = true;

                        GameControl.gameControl.playerTurn = GameControl.PlayerTurn.player2;
                    }

                    else if(throwCard.Count > 1 && throwCard.Count <= 5)
                    {
                        if (throwCard.Count > 1 && throwCard.Count <= 5)
                        {
                            SortCardInHand();

                            CardHandCheck();

                            int scoreCard = 0;

                            int playerScoreCard = 0;

                            for (int i = 0; i < GameControl.gameControl.cardDeckPos.childCount; i++)
                            {
                                scoreCard += GameControl.gameControl.cardDeckPos.GetChild(i).GetComponent<Card>().ScoreCard;
                            }

                            for (int x = 0; x < sortThrowCard.Count; x++)
                            {
                                playerScoreCard += sortThrowCard[x].ScoreCard;
                            }

                            if (playerScoreCard > scoreCard)
                            {
                                StartCoroutine(ThrowCards());
                            }
                        }
                    }
                }

                else if (GameControl.gameControl.cardDeckPos.transform.childCount > 1)
                {
                    if (throwCard.Count > 1 && throwCard.Count <= 5)
                    {
                        SortCardInHand();

                        CardHandCheck();

                        int scoreCard = 0;

                        int playerScoreCard = 0;

                        for (int i = 0; i < GameControl.gameControl.cardDeckPos.childCount; i++)
                        {
                            scoreCard += GameControl.gameControl.cardDeckPos.GetChild(i).GetComponent<Card>().ScoreCard;
                        }

                        for (int x = 0; x < sortThrowCard.Count; x++)
                        {
                            playerScoreCard += sortThrowCard[x].ScoreCard;
                        }

                        if (playerScoreCard > scoreCard)
                        {
                            StartCoroutine(ThrowCards());
                        }
                    }
                }
            }
        }

        return;
    }

    /// <summary>
    /// manage to throw cards
    /// </summary>
    /// <returns></returns>
    IEnumerator ThrowCards ()
    {
        Helper.DeleteThrowCard();

        Helper.ThrowCard(throwCard, cardInHand);

        Helper.StopAnimationOnCard(throwCard);

        Helper.DisableThrowedCondition(AI);

        throwCard.Clear();

        this.GetComponent<Player>().enabled = false;

        GameControl.gameControl.player2Pos.GetComponent<AI>().enabled = true;

        GameControl.gameControl.playerTurn = GameControl.PlayerTurn.player2;

        yield break;
    }

    /// <summary>
    /// sort hand card
    /// </summary>
    public void SortCardInHand ()
    {
        if(throwCard.Count > 0)
        {
            for (int i = 0; i < throwCard.Count; i++)
            {
                Card card = throwCard[i];

                scoreCardHands.Add(card.ScoreCard);
            }

            scoreCardHands.Sort();

            if(scoreCardHands.Count > 0)
            {
                while (sortThrowCard.Count != throwCard.Count)
                {
                    if(scoreCardHands.Count > 0) scoreFirstIndex = scoreCardHands[0];

                    for (int i = 0; i < throwCard.Count; i++)
                    {
                        if (throwCard[i].ScoreCard == scoreFirstIndex)
                        {
                            sortThrowCard.Add(throwCard[i]);
                        }
                    }

                    if(scoreCardHands.Count > 0) scoreCardHands.RemoveAt(0);

                }
            }
        }

        return;
    }

    /// <summary>
    /// chceck the card hand om check
    /// and first for the card hand
    /// </summary>
    public void CardHandCheck ()
    {
        OnePairCheck();

        TwoPairCheck();

        ThreeOfAKindCheck();

        StraightCheck();

        FlushCheck();

        FourOFakindCheck();

        return;
    }

    /// <summary>
    /// manage to select for the one pair 
    /// </summary>
    public void OnePairCheck ()
    {
        //ONE PAIR CHECK
        for (int i = 0; i < sortThrowCard.Count; i++)
        {
            if (i < sortThrowCard.Count - 1)
            {
                if (sortThrowCard[i].cardId == sortThrowCard[i + 1].cardId)
                {
                    onePairFirstCountSystem++;

                    if (onePairFirstCountSystem == 2)
                    {
                        idSameRankFirstCountOnePair = sortThrowCard[i].cardId;

                        break;
                    }
                }
            }
        }

        if (idSameRankFirstCountOnePair > 0)
        {
            for (int x = 0; x < sortThrowCard.Count; x++)
            {
                if (sortThrowCard[x].cardId != idSameRankFirstCountOnePair)
                {
                    idSameRankFirstCountOnePair = sortThrowCard[x].cardId;

                    break;
                }
            }
        }

        if (idSameRankSecondOnePair > 0)
        {
            for (int z = 0; z < sortThrowCard.Count; z++)
            {
                if (sortThrowCard[z].cardId != idSameRankFirstCountOnePair && sortThrowCard[z].cardId != idSameRankSecondOnePair)
                {
                    idSameRankThirdOnePair = sortThrowCard[z].cardId;

                    break;
                }
            }
        }

        if (idSameRankThirdOnePair > 0)
        {
            for (int a = 0; a < sortThrowCard.Count; a++)
            {
                if (sortThrowCard[a].cardId != idSameRankFirstCountOnePair && sortThrowCard[a].cardId != idSameRankSecondOnePair && sortThrowCard[a].cardId != idSameRankThirdOnePair)
                {
                    onePairFirstCountSystem = 0;

                    onePair = true;
                }
            }
        }


        return;
    }

    /// <summary>
    /// for the two pair check
    /// </summary>
    public void TwoPairCheck ()
    {
        //TWO PAIR CHECK
        for (int i = 0; i < sortThrowCard.Count; i++)
        {
            if (i < sortThrowCard.Count - 1)
            {
                if (sortThrowCard[i].cardId == sortThrowCard[i + 1].cardId)
                {
                    twoPairFirstCountSystem++;

                    if (twoPairFirstCountSystem == 2)
                    {
                        idSameRankTwoPair = sortThrowCard[i].cardId;

                        break;
                    }
                }
            }
        }

        for (int x = 0; x < sortThrowCard.Count; x++)
        {
            if (x < sortThrowCard.Count - 1)
            {
                if (sortThrowCard[x].cardId != idSameRankTwoPair)
                {
                    if (sortThrowCard[x] == sortThrowCard[x + 1])
                    {
                        twoPairSecondCountSystem++;

                        if (twoPairSecondCountSystem == 2)
                        {
                            idSameRankSecondTwoPair = sortThrowCard[x].cardId;

                            break;
                        }
                    }
                }
            }
        }

        for (int a = 0; a < sortThrowCard.Count; a++)
        {
            if (a < sortThrowCard.Count - 1 && idSameRankSecondTwoPair > 0)
            {
                if (sortThrowCard[a].cardId != idSameRankTwoPair && sortThrowCard[a].cardId != idSameRankSecondTwoPair)
                {
                    twoPairFirstCountSystem = 0;

                    twoPairSecondCountSystem = 0;

                    twoPair = true;
                }
            }
        }

        return;
    }

    /// <summary>
    /// theree of a kind
    /// </summary>
    public void ThreeOfAKindCheck ()
    {
        for (int i = 0; i < sortThrowCard.Count; i++)
        {
            if (i < sortThrowCard.Count - 1)
            {
                if (sortThrowCard[i].cardId == sortThrowCard[i + 1].cardId)
                {
                    threeOfAkindSystemCount++;

                    if (threeOfAkindSystemCount == 3)
                    {
                        threeOfAkindSystemCount = 0;

                        threeOfAKind = true;

                        break;
                    }
                }
            }
        }

        return;
    }

    /// <summary>
    /// straight check
    /// </summary>
    public void StraightCheck ()
    {
        for (int i = 0; i < sortThrowCard.Count; i++)
        {
            if (i < sortThrowCard.Count - 1)
            {
                if ((sortThrowCard[i + 1].cardId - sortThrowCard[i].cardId) == 1)
                {
                    straightSystemCount++;

                    if (straightSystemCount == 5)
                    {
                        straightSystemCount = 0;

                        straight = true;

                        break;
                    }
                }
            }
        }

        return;
    }

    /// <summary>
    /// manage the flush check
    /// </summary>
    public void FlushCheck ()
    {
        for (int i = 0; i < sortThrowCard.Count; i++)
        {
            switch (throwCard[i].cardLabel)
            {
                case GameControl.SPADES:
                    spadesCount++;
                    break;
                case GameControl.HEARTS:
                    heartsCount++;
                    break;
                case GameControl.CLUBS:
                    clubsCount++;
                    break;
                case GameControl.DIAMONDS:
                    diamondsCount++;
                    break;
            }
        }

        if (spadesCount >= 5 || heartsCount >= 5 || clubsCount >= 5 || diamondsCount >= 5)
        {
            spadesCount = 0;
            heartsCount = 0;
            clubsCount = 0;
            diamondsCount = 0;


            flush = true;
        }

        return;
    }

    /// <summary>
    /// four of a kind check
    /// </summary>
    public void FourOFakindCheck ()
    {
        for (int i = 0; i < sortThrowCard.Count; i++)
        {
            if (i < sortThrowCard.Count - 1)
            {
                if (sortThrowCard[i].cardId == sortThrowCard[i + 1].cardId)
                {
                    fourOfAKindSystemCount++;

                    if (fourOfAKindSystemCount == 4)
                    {
                        fourOfAKindSystemCount = 0;

                        fourOfAKind = true;

                        break;
                    }
                }
            }
        }

        return;
    }

    /// <summary>
    /// control state end
    /// </summary>
    public void ControlStateEnd ()
    {
        if (this.transform.childCount <= 0 && GameControl.gameControl.playStart)
        {
            StopAllCoroutines();

            GameControl.gameControl.gameState = GameControl.GameState.End;
        }

        return;
    }

    /// <summary>
    /// pass the turn from the player
    /// </summary>
    public void PassTurn ()
    {
        if(turnCheck)
        {
            this.GetComponent<Player>().enabled = false;

            GameControl.gameControl.player2Pos.GetComponent<AI>().enabled = true;

            GameControl.gameControl.playerTurn = GameControl.PlayerTurn.player2;
        }

        return;
    }
}
