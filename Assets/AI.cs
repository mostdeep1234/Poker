using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
    public static AI aiInstance;

    public bool firstTurn;

    public List<Card> playerHands = new List<Card>();

    public List<Card> sortHandCard = new List<Card>();

    public List<int> scoreCard = new List<int>();

    public List<Card> throwCards = new List<Card>();

    public int throwedScoreCard;

    public int combinationScoreCard;

    public string AIid;

    public Image aiImage;

    public List<Sprite> conditionSprites;

    public List<AI> otherAiPlayers = new List<AI>();


    /// <summary>
    /// //////////////////////////////////////////////////////////////////.
    /// </summary>


    public bool containFlush;

    public bool containStraight;

    public bool containThreeOfAKind;

    public bool containFourOfAKind;

    public bool containFullHouse;

    public bool containOnePair;

    public bool containTwoPair;

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////
    /// </summary>

    public bool throwCardCondition;

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////
    /// </summary>


    public int flushSystemCountSpades;

    public int flushSystemCountHearts;

    public int flushSystemCountClubs;

    public int flushSystemCountDiamonds;

    /// <summary>
    /// //////////////////////////////////////////////////////////////////
    /// </summary>

    public int straightSystemCount;

    /// <summary>
    /// /////////////////////////////////////////////////////////////////
    /// </summary>

    public int threeOfAkindSystemCount;

    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// </summary>

    public int fourOfAKindSystemCount;

    /// <summary>
    /// ///////////////////////////////////////////////////////////////
    /// </summary>

    public int id3SameRankCardFullHouse;

    public int fullHouseThreeRankCardSystem;

    public int fullHouseTwoRankCardSystems;


    /// <summary>
    /// ///////////////////////////////////////////////////////////////
    /// </summary>

    public int idSameRankCardTwoPair;

    public int idSameRankSecondTwoPair;

    public int twoPairFirstCountSystem;

    public int twoPairSecondCountSystem;

    /// <summary>
    /// ///////////////////////////////////////////////////////////////
    /// </summary>
    public int idSameRankFirstCardOnePair;

    public int idSameRankSecondCardOnePair;

    public int idSameRankThirdCardOnePair;

    public int idSameRankFourthCardOnePair;

    public int onePairFirstCountSystem;


    /// <summary>
    /// //////////////////////////////////////////////////////////////
    /// </summary>

    public List<Card> onePairListCard = new List<Card>();

    public List<Card> twoPairListCard = new List<Card>();

    public List<Card> threeOfAKindListCard = new List<Card>();

    public List<Card> fourOfAKindListCard = new List<Card>();

    public List<Card> straightListCard = new List<Card>();

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////
    /// </summary>

    public List<Card> flushListCardSPADES = new List<Card>();

    public List<Card> flushListCardHEARTS = new List<Card>();

    public List<Card> flushListCardCLUBS = new List<Card>();

    public List<Card> flushListCardDIAMONDS = new List<Card>();

    public List<Card> flushListCardToThrow = new List<Card>();

    /// <summary>
    /// ///////////////////////////////////////////////////////////////////////////
    /// </summary>

    public List<Card> fullHouseListCard = new List<Card>();

    public enum AIState
    {
        Firstturn, Think, Throw, Endturn
    };

    public AIState aIState;

    public bool checkTurnAction = false;

    public bool registerAction = false;

    public bool firstTurnAction = false;

    public bool endTurnAction = false;

    public bool thinkAction = false;

    private void Awake()
    {
        aiInstance = this;
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        GameControl.gameControl.aiCheckTurnReady = true;

        if (registerAction == false) RegisterToHand();

        CheckTurn();

        GameControl.gameControl.player1Pos.GetComponent<Player>().firstTurn = false;

        thinkAction = false;

        endTurnAction = false;

    }

    private void Start()
    {

    }

    private void Update()
    {
        State();

        TrackingTotalCardLeft();

        ConditionSpirtes();
    }

    // Update is called once per frame
    void OnDisable()
    {
        ResetAllCondition();

        GameControl.gameControl.throwedCard.Clear();
    }

    /// <summary>
    /// register to hand
    /// </summary>
    public void RegisterToHand()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Card card = this.transform.GetChild(i).GetComponent<Card>();

            playerHands.Add(card);

            scoreCard.Add(card.ScoreCard);
        }

        SortCardOnHand();

        registerAction = true;

        return;
    }

    /// <summary>
    /// sort card on hand
    /// by simple selection sort algorithm
    /// </summary>
    public void SortCardOnHand()
    {
        scoreCard.Sort();

        while (sortHandCard.Count != playerHands.Count)
        {
            int scoreFirstIndex = scoreCard[0];

            for (int i = 0; i < playerHands.Count; i++)
            {
                if (playerHands[i].ScoreCard == scoreFirstIndex)
                {
                    sortHandCard.Add(playerHands[i]);
                }
            }

            scoreCard.RemoveAt(0);

        }

        return;
    }

    /// <summary>
    /// check turn
    /// </summary>
    public void CheckTurn()
    {
        containThreeOfAKind = CountainThreeOfAKind();

        containFlush = ContainFlush();

        containStraight = ContainsStraight();

        containFourOfAKind = ContainFourOfAKind();

        containOnePair = ContainPair();

        containTwoPair = ContainTwoPair();

        checkTurnAction = true;

        return;
    }

    /// <summary>
    /// //////////////////////////////////////////////////////////////
    /// </summary>
    /// <returns></returns>

    public bool ContainPair()
    {
        if (sortHandCard.Count < 5)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < sortHandCard.Count; i++)
            {
                if (i < sortHandCard.Count - 1)
                {
                    if (sortHandCard[i].cardId == sortHandCard[i + 1].cardId)
                    {
                        onePairFirstCountSystem++;

                        if (onePairFirstCountSystem < 2) onePairListCard.Add(sortHandCard[i]);

                        if (onePairFirstCountSystem == 2)
                        {
                            idSameRankFirstCardOnePair = sortHandCard[i].cardId;

                            onePairListCard.Add(sortHandCard[i + 1]);

                            break;
                        }
                    }
                }
            }

            if (idSameRankFirstCardOnePair > 0)
            {
                for (int x = 0; x < sortHandCard.Count; x++)
                {
                    if (sortHandCard[x].cardId != idSameRankFirstCardOnePair)
                    {
                        idSameRankSecondCardOnePair = sortHandCard[x].cardId;

                        onePairListCard.Add(sortHandCard[x]);

                        break;
                    }
                }
            }

            if (idSameRankSecondCardOnePair > 0)
            {
                for (int z = 0; z < sortHandCard.Count; z++)
                {
                    if (sortHandCard[z].cardId != idSameRankFirstCardOnePair && sortHandCard[z].cardId != idSameRankSecondCardOnePair)
                    {
                        idSameRankThirdCardOnePair = sortHandCard[z].cardId;

                        onePairListCard.Add(sortHandCard[z]);

                        break;
                    }
                }
            }

            if (idSameRankThirdCardOnePair > 0)
            {
                for (int a = 0; a < sortHandCard.Count; a++)
                {
                    if (sortHandCard[a].cardId != idSameRankFirstCardOnePair && sortHandCard[a].cardId != idSameRankSecondCardOnePair && sortHandCard[a].cardId != idSameRankThirdCardOnePair)
                    {
                        onePairListCard.Add(sortHandCard[a]);

                        onePairFirstCountSystem = 0;

                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool ContainTwoPair()
    {
        if (sortHandCard.Count < 5)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < sortHandCard.Count; i++)
            {
                if (i < sortHandCard.Count - 1)
                {
                    if (sortHandCard[i].cardId == sortHandCard[i + 1].cardId)
                    {
                        twoPairFirstCountSystem++;

                        if (twoPairFirstCountSystem < 2) twoPairListCard.Add(sortHandCard[i]);

                        if (twoPairFirstCountSystem == 2)
                        {
                            idSameRankCardTwoPair = sortHandCard[i].cardId;

                            twoPairListCard.Add(sortHandCard[i + 1]);

                            break;
                        }
                    }
                }
            }

            for (int x = 0; x < sortHandCard.Count; x++)
            {
                if (x < sortHandCard.Count - 1)
                {
                    if (sortHandCard[x].cardId != idSameRankCardTwoPair)
                    {
                        if (sortHandCard[x] == sortHandCard[x + 1])
                        {
                            twoPairSecondCountSystem++;

                            if (twoPairSecondCountSystem < 2) twoPairListCard.Add(sortHandCard[x]);

                            if (twoPairSecondCountSystem == 2)
                            {
                                idSameRankSecondTwoPair = sortHandCard[x].cardId;

                                twoPairListCard.Add(sortHandCard[x + 1]);

                                break;
                            }
                        }
                    }
                }
            }

            for (int a = 0; a < sortHandCard.Count; a++)
            {
                if (a < sortHandCard.Count - 1 && idSameRankSecondTwoPair > 0)
                {
                    if (sortHandCard[a].cardId != idSameRankCardTwoPair && sortHandCard[a].cardId != idSameRankSecondTwoPair)
                    {
                        twoPairListCard.Add(sortHandCard[a]);

                        twoPairFirstCountSystem = 0;

                        twoPairSecondCountSystem = 0;

                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool CountainThreeOfAKind()
    {
        if (sortHandCard.Count < 5)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < sortHandCard.Count; i++)
            {
                if (i < sortHandCard.Count - 1)
                {
                    if (sortHandCard[i].cardId == sortHandCard[i + 1].cardId)
                    {
                        threeOfAkindSystemCount++;

                        if (threeOfAkindSystemCount < 3) threeOfAKindListCard.Add(sortHandCard[i]);

                        if (threeOfAkindSystemCount == 3)
                        {
                            threeOfAKindListCard.Add(sortHandCard[i + 1]);

                            break;
                        }
                    }
                }
            }

            if (threeOfAkindSystemCount == 3)
            {
                threeOfAkindSystemCount = 0;

                return true;
            }
        }



        return false;
    }

    public bool ContainFourOfAKind()
    {
        if (sortHandCard.Count < 5)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < sortHandCard.Count; i++)
            {
                if (i < sortHandCard.Count - 1)
                {
                    if (sortHandCard[i].cardId == sortHandCard[i + 1].cardId)
                    {
                        fourOfAKindSystemCount++;

                        if (fourOfAKindSystemCount < 4) fourOfAKindListCard.Add(sortHandCard[i]);

                        if (fourOfAKindSystemCount == 4)
                        {
                            fourOfAKindListCard.Add(sortHandCard[i + 1]);

                            break;
                        }
                    }
                }
            }

            if (fourOfAKindSystemCount == 4)
            {
                fourOfAKindSystemCount = 0;

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// by checking all the hands that contain straight
    /// </summary>
    /// <returns></returns>
    public bool ContainsStraight()
    {
        if (sortHandCard.Count < 5)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < sortHandCard.Count; i++)
            {
                if (i < sortHandCard.Count - 1)
                {
                    if ((sortHandCard[i + 1].cardId - sortHandCard[i].cardId) == 1)
                    {
                        straightSystemCount++;

                        if (straightSystemCount < 5) straightListCard.Add(sortHandCard[i]);

                        if (straightSystemCount == 5)
                        {
                            straightListCard.Add(sortHandCard[i + 1]);

                            break;
                        }
                    }
                }
            }

            if (straightSystemCount >= 5)
            {
                straightSystemCount = 0;

                return true;
            }
        }

        return false;
    }

    public bool ContainFlush()
    {
        if (sortHandCard.Count < 5)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < sortHandCard.Count; i++)
            {
                switch (sortHandCard[i].cardLabel)
                {
                    case GameControl.SPADES:
                        flushSystemCountSpades++;
                        if (flushSystemCountSpades <= 5) flushListCardSPADES.Add(sortHandCard[i]);
                        break;
                    case GameControl.HEARTS:
                        flushSystemCountHearts++;
                        if (flushSystemCountHearts <= 5) flushListCardHEARTS.Add(sortHandCard[i]);
                        break;
                    case GameControl.CLUBS:
                        flushSystemCountClubs++;
                        if (flushSystemCountClubs <= 5) flushListCardCLUBS.Add(sortHandCard[i]);
                        break;
                    case GameControl.DIAMONDS:
                        flushSystemCountDiamonds++;
                        if (flushSystemCountDiamonds <= 5) flushListCardDIAMONDS.Add(sortHandCard[i]);
                        break;
                }
            }

            if (flushSystemCountSpades >= 5 || flushSystemCountHearts >= 5 || flushSystemCountClubs >= 5 || flushSystemCountDiamonds >= 5)
            {
                if (flushSystemCountSpades == 5) flushListCardToThrow = flushListCardSPADES;
                if (flushSystemCountHearts == 5) flushListCardToThrow = flushListCardHEARTS;
                if (flushSystemCountClubs == 5) flushListCardToThrow = flushListCardCLUBS;
                if (flushSystemCountDiamonds == 5) flushListCardToThrow = flushListCardDIAMONDS;


                flushSystemCountClubs = 0;
                flushSystemCountDiamonds = 0;
                flushSystemCountHearts = 0;
                flushSystemCountSpades = 0;


                return true;
            }
        }

        return false;
    }

    public bool ContainFullHouse()
    {
        if (sortHandCard.Count < 5)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < sortHandCard.Count; i++)
            {
                if (i < sortHandCard.Count - 1)
                {
                    if (sortHandCard[i].cardId == sortHandCard[i + 1].cardId)
                    {
                        fullHouseThreeRankCardSystem++;

                        if (fullHouseThreeRankCardSystem < 3) fullHouseListCard.Add(sortHandCard[i]);

                        if (fullHouseThreeRankCardSystem == 3)
                        {
                            id3SameRankCardFullHouse = sortHandCard[i].cardId;

                            fullHouseListCard.Add(sortHandCard[i + 1]);

                            break;
                        }
                    }
                }
            }

            for (int x = 0; x < sortHandCard.Count; x++)
            {
                if (x < sortHandCard.Count - 1 && id3SameRankCardFullHouse > 0)
                {
                    if (sortHandCard[x].cardId != id3SameRankCardFullHouse)
                    {
                        if (x < sortHandCard.Count - 1)
                        {
                            if (sortHandCard[x] == sortHandCard[x + 1])
                            {
                                fullHouseListCard.Add(sortHandCard[x]);

                                fullHouseListCard.Add(sortHandCard[x + 1]);

                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// throw the card into the card state instance
    /// </summary>
    /// <param name="card"></param>
    public void ThrowCard(List<Card> card)
    {
        if (GameControl.gameControl.CardThrowTotal() <= 0)
        {
            aIState = AIState.Firstturn;
        }
        return;
    }

    /// <summary>
    /// manage for the state to ai state
    /// </summary>
    public void State()
    {
        switch (aIState)
        {
            case AIState.Firstturn:
                if (firstTurnAction == false) FirstTurnLogic();
                break;
            case AIState.Think:
                if (thinkAction == false) Think();
                break;
            case AIState.Throw:
                break;
            case AIState.Endturn:
                if (endTurnAction == false) StartCoroutine(EndTurn());
                break;
        }
    }

    /// <summary>
    /// first turn logic
    /// </summary>
    public void FirstTurnLogic()
    {
        if (containOnePair)
        {
            Helper.ThrowCard(onePairListCard, playerHands);

            GameControl.onePairOnThrow = true;

            containOnePair = false;
        }
        else if (containTwoPair && containOnePair == false)
        {
            Helper.ThrowCard(twoPairListCard, playerHands);

            GameControl.twoPairOnThrow = true;

            containTwoPair = false;
        }
        else if (containThreeOfAKind && containTwoPair == false && containOnePair == false)
        {
            Helper.ThrowCard(threeOfAKindListCard, playerHands);

            GameControl.threeOfAKindOnThrow = true;

            containThreeOfAKind = false;
        }
        else if (containFourOfAKind && containThreeOfAKind == false && containOnePair == false && containTwoPair == false)
        {
            Helper.ThrowCard(fourOfAKindListCard, playerHands);

            GameControl.fourOfAKindOnThrow = true;

            containFourOfAKind = false;
        }
        else if (containFourOfAKind == false && containThreeOfAKind == false && containOnePair == false && containTwoPair == false)
        {
            int x = Random.Range(0, sortHandCard.Count);

            GameControl.gameControl.throwedCard.Add(sortHandCard[x]);

            playerHands.Remove(sortHandCard[x]);

            GameControl.gameControl.throwedCard[0].transform.SetParent(GameControl.gameControl.cardDeckPos);

            GameControl.gameControl.throwedCard[0].transform.localPosition = new Vector3(0, 0, 0);

            GameControl.gameControl.throwedCard[0].GetComponent<SpriteRenderer>().sortingOrder = 2;
        }

        firstTurnAction = true;

        aIState = AIState.Endturn;

        return;
    }

    public IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(5.0f);

        switch (GameControl.gameControl.playerTurn)
        {
            case GameControl.PlayerTurn.player2:
                GameControl.gameControl.player3Pos.GetComponent<AI>().aIState = AIState.Think;
                GameControl.gameControl.player3Pos.GetComponent<AI>().enabled = true;
                this.GetComponent<AI>().enabled = false;
                GameControl.gameControl.playerTurn = GameControl.PlayerTurn.player3;
                break;
            case GameControl.PlayerTurn.player3:
                GameControl.gameControl.player4Pos.GetComponent<AI>().aIState = AIState.Think;
                GameControl.gameControl.player4Pos.GetComponent<AI>().enabled = true;
                this.GetComponent<AI>().enabled = false;
                GameControl.gameControl.playerTurn = GameControl.PlayerTurn.player4;
                break;
            case GameControl.PlayerTurn.player4:
                GameControl.gameControl.player1Pos.GetComponent<Player>().enabled = true;
                this.GetComponent<AI>().enabled = false;
                GameControl.gameControl.playerTurn = GameControl.PlayerTurn.player1;
                break;
        }

        StopAllCoroutines();

        endTurnAction = true;

        yield break;
    }



    public void Think()
    {
        if (throwCardCondition == false)
        {
            if (GameControl.onePairOnThrow)
            {
                GameControl.onePairOnThrow = false;

                if (containTwoPair)
                {
                    print("PAIR");

                    GameControl.twoPairOnThrow = true;

                    Helper.DeleteThrowCard();

                    Helper.ThrowCard(twoPairListCard, sortHandCard);

                    if (throwCardCondition == false) throwCardCondition = true;

                    Helper.DisableThrowedCondition(otherAiPlayers);

                    containTwoPair = false;

                    endTurnAction = false;

                    aIState = AIState.Endturn;
                }

                else if (containOnePair && containTwoPair == false)
                {
                    throwedScoreCard = Helper.CalculateThrowCard(GameControl.gameControl.throwedCard);

                    combinationScoreCard = Helper.CalculateCombinationCard(onePairListCard);

                    if (throwedScoreCard > combinationScoreCard)
                    {
                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }

                    if (throwedScoreCard < combinationScoreCard)
                    {
                        print("PAIR");

                        GameControl.onePairOnThrow = true;

                        Helper.DeleteThrowCard();

                        Helper.ThrowCard(onePairListCard, sortHandCard);

                        if (throwCardCondition == false) throwCardCondition = true;

                        Helper.DisableThrowedCondition(otherAiPlayers);

                        containOnePair = false;

                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }
                }
                else
                {
                    if (GameControl.gameControl.cardDeckPos.childCount == 1)
                    {
                        int throwedCardScore = GameControl.gameControl.cardDeckPos.GetChild(0).GetComponent<Card>().ScoreCard;

                        Card findBestCard = Helper.findBestCardFor(sortHandCard, throwedCardScore);

                        if (findBestCard != null)
                        {
                            print("SINGLE CARD");

                            Helper.DeleteThrowCard();

                            GameControl.gameControl.throwedCard.Add(findBestCard);

                            sortHandCard.Remove(findBestCard);

                            GameControl.gameControl.throwedCard[0].transform.SetParent(GameControl.gameControl.cardDeckPos);

                            GameControl.gameControl.throwedCard[0].transform.localPosition = new Vector3(0, 0, 0);

                            GameControl.gameControl.throwedCard[0].GetComponent<SpriteRenderer>().sortingOrder = 2;

                            endTurnAction = false;

                            if (throwCardCondition == false) throwCardCondition = true;

                            Helper.DisableThrowedCondition(otherAiPlayers);

                            aIState = AIState.Endturn;
                        }
                        else
                        {
                            GameControl.onePairOnThrow = true;

                            endTurnAction = false;

                            aIState = AIState.Endturn;
                        }
                    }
                    else
                    {
                        GameControl.onePairOnThrow = true;

                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }
                }
            }
            else if (GameControl.twoPairOnThrow)
            {
                GameControl.twoPairOnThrow = false;

                if (containThreeOfAKind)
                {
                    print("PAIR");

                    GameControl.threeOfAKindOnThrow = true;

                    Helper.DeleteThrowCard();

                    Helper.ThrowCard(threeOfAKindListCard, sortHandCard);

                    if (throwCardCondition == false) throwCardCondition = true;

                    Helper.DisableThrowedCondition(otherAiPlayers);

                    containThreeOfAKind = false;

                    endTurnAction = false;

                    aIState = AIState.Endturn;
                }

                else if (containTwoPair && containThreeOfAKind == false)
                {
                    throwedScoreCard = Helper.CalculateThrowCard(GameControl.gameControl.throwedCard);

                    combinationScoreCard = Helper.CalculateCombinationCard(twoPairListCard);

                    if (throwedScoreCard > combinationScoreCard)
                    {
                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }

                    if (throwedScoreCard < combinationScoreCard)
                    {
                        print("PAIR");

                        GameControl.twoPairOnThrow = true;

                        Helper.DeleteThrowCard();

                        Helper.ThrowCard(twoPairListCard, sortHandCard);

                        if (throwCardCondition == false) throwCardCondition = true;

                        Helper.DisableThrowedCondition(otherAiPlayers);

                        containTwoPair = false;

                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }
                }
                else
                {
                    if (GameControl.gameControl.cardDeckPos.childCount == 1)
                    {
                        int throwedCardScore = GameControl.gameControl.cardDeckPos.GetChild(0).GetComponent<Card>().ScoreCard;

                        Card findBestCard = Helper.findBestCardFor(sortHandCard, throwedCardScore);

                        if (findBestCard != null)
                        {
                            print("SINGLE CARD");

                            Helper.DeleteThrowCard();

                            GameControl.gameControl.throwedCard.Add(findBestCard);

                            sortHandCard.Remove(findBestCard);

                            GameControl.gameControl.throwedCard[0].transform.SetParent(GameControl.gameControl.cardDeckPos);

                            GameControl.gameControl.throwedCard[0].transform.localPosition = new Vector3(0, 0, 0);

                            GameControl.gameControl.throwedCard[0].GetComponent<SpriteRenderer>().sortingOrder = 2;

                            if (throwCardCondition == false) throwCardCondition = true;

                            Helper.DisableThrowedCondition(otherAiPlayers);

                            endTurnAction = false;

                            aIState = AIState.Endturn;
                        }
                        else
                        {
                            GameControl.twoPairOnThrow = true;

                            endTurnAction = false;

                            aIState = AIState.Endturn;
                        }
                    }
                    else
                    {
                        GameControl.twoPairOnThrow = true;

                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }
                }
            }
            else if (GameControl.threeOfAKindOnThrow)
            {
                GameControl.threeOfAKindOnThrow = false;

                if (containStraight)
                {
                    print("PAIR");

                    GameControl.straightOnThrow = true;

                    Helper.DeleteThrowCard();

                    Helper.ThrowCard(straightListCard, sortHandCard);

                    if (throwCardCondition == false) throwCardCondition = true;

                    Helper.DisableThrowedCondition(otherAiPlayers);

                    containStraight = false;

                    endTurnAction = false;

                    aIState = AIState.Endturn;
                }

                else if (containThreeOfAKind && containStraight == false)
                {
                    throwedScoreCard = Helper.CalculateThrowCard(GameControl.gameControl.throwedCard);

                    combinationScoreCard = Helper.CalculateCombinationCard(threeOfAKindListCard);

                    if (throwedScoreCard > combinationScoreCard)
                    {
                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }

                    if (throwedScoreCard < combinationScoreCard)
                    {
                        print("PAIR");

                        GameControl.threeOfAKindOnThrow = true;

                        Helper.DeleteThrowCard();

                        Helper.ThrowCard(threeOfAKindListCard, sortHandCard);

                        if (throwCardCondition == false) throwCardCondition = true;

                        Helper.DisableThrowedCondition(otherAiPlayers);

                        containThreeOfAKind = false;

                        endTurnAction = false;

                        aIState = AIState.Endturn;

                    }
                }
                else
                {
                    if (GameControl.gameControl.cardDeckPos.childCount == 1)
                    {
                        int throwedCardScore = GameControl.gameControl.cardDeckPos.GetChild(0).GetComponent<Card>().ScoreCard;

                        Card findBestCard = Helper.findBestCardFor(sortHandCard, throwedCardScore);

                        if (findBestCard != null)
                        {
                            print("SINGLE CARD");

                            Helper.DeleteThrowCard();

                            GameControl.gameControl.throwedCard.Add(findBestCard);

                            sortHandCard.Remove(findBestCard);

                            GameControl.gameControl.throwedCard[0].transform.SetParent(GameControl.gameControl.cardDeckPos);

                            GameControl.gameControl.throwedCard[0].transform.localPosition = new Vector3(0, 0, 0);

                            GameControl.gameControl.throwedCard[0].GetComponent<SpriteRenderer>().sortingOrder = 2;

                            if (throwCardCondition == false) throwCardCondition = true;

                            Helper.DisableThrowedCondition(otherAiPlayers);

                            endTurnAction = false;

                            aIState = AIState.Endturn;
                        }
                        else
                        {
                            GameControl.threeOfAKindOnThrow = true;

                            endTurnAction = false;

                            aIState = AIState.Endturn;
                        }
                    }
                    else
                    {
                        GameControl.threeOfAKindOnThrow = true;

                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }
                }
            }
            else if (GameControl.straightOnThrow)
            {
                GameControl.straightOnThrow = false;

                if (containFlush)
                {
                    print("PAIR");

                    GameControl.flusheOnThrow = true;

                    Helper.DeleteThrowCard();

                    Helper.ThrowCard(flushListCardToThrow, sortHandCard);

                    if (throwCardCondition == false) throwCardCondition = true;

                    Helper.DisableThrowedCondition(otherAiPlayers);

                    containFlush = false;

                    endTurnAction = false;

                    aIState = AIState.Endturn;

                }

                else if (containStraight && containFlush == false)
                {
                    throwedScoreCard = Helper.CalculateThrowCard(GameControl.gameControl.throwedCard);

                    combinationScoreCard = Helper.CalculateCombinationCard(straightListCard);

                    if (throwedScoreCard > combinationScoreCard)
                    {
                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }

                    if (throwedScoreCard < combinationScoreCard)
                    {
                        print("PAIR");

                        GameControl.straightOnThrow = true;

                        Helper.DeleteThrowCard();

                        Helper.ThrowCard(straightListCard, sortHandCard);

                        if (throwCardCondition == false) throwCardCondition = true;

                        Helper.DisableThrowedCondition(otherAiPlayers);

                        containStraight = false;

                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }
                }
                else
                {
                    if (GameControl.gameControl.cardDeckPos.childCount == 1)
                    {
                        int throwedCardScore = GameControl.gameControl.cardDeckPos.GetChild(0).GetComponent<Card>().ScoreCard;

                        Card findBestCard = Helper.findBestCardFor(sortHandCard, throwedCardScore);

                        if (findBestCard != null)
                        {
                            print("SINGLE CARD");

                            Helper.DeleteThrowCard();

                            GameControl.gameControl.throwedCard.Add(findBestCard);

                            sortHandCard.Remove(findBestCard);

                            GameControl.gameControl.throwedCard[0].transform.SetParent(GameControl.gameControl.cardDeckPos);

                            GameControl.gameControl.throwedCard[0].transform.localPosition = new Vector3(0, 0, 0);

                            GameControl.gameControl.throwedCard[0].GetComponent<SpriteRenderer>().sortingOrder = 2;

                            if (throwCardCondition == false) throwCardCondition = true;

                            Helper.DisableThrowedCondition(otherAiPlayers);

                            endTurnAction = false;

                            aIState = AIState.Endturn;
                        }
                        else
                        {
                            GameControl.straightOnThrow = true;

                            endTurnAction = false;

                            aIState = AIState.Endturn;
                        }
                    }
                    else
                    {
                        GameControl.straightOnThrow = true;

                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }
                }
            }
            else if (GameControl.flusheOnThrow)
            {
                GameControl.flusheOnThrow = false;

                if (containFullHouse)
                {
                    print("PAIR");

                    GameControl.fullHouseOnThrow = true;

                    Helper.DeleteThrowCard();

                    Helper.ThrowCard(fullHouseListCard, sortHandCard);

                    if (throwCardCondition == false) throwCardCondition = true;

                    Helper.DisableThrowedCondition(otherAiPlayers);

                    containFullHouse = false;

                    endTurnAction = false;

                    aIState = AIState.Endturn;
                }

                else if (containFlush && containFullHouse == false)
                {
                    throwedScoreCard = Helper.CalculateThrowCard(GameControl.gameControl.throwedCard);

                    combinationScoreCard = Helper.CalculateCombinationCard(fullHouseListCard);

                    if (throwedScoreCard > combinationScoreCard)
                    {
                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }

                    if (throwedScoreCard < combinationScoreCard)
                    {
                        print("PAIR");

                        GameControl.flusheOnThrow = true;

                        Helper.DeleteThrowCard();

                        Helper.ThrowCard(straightListCard, sortHandCard);

                        if (throwCardCondition == false) throwCardCondition = true;

                        Helper.DisableThrowedCondition(otherAiPlayers);

                        containFlush = false;

                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }
                }
                else
                {
                    if (GameControl.gameControl.cardDeckPos.childCount == 1)
                    {
                        int throwedCardScore = GameControl.gameControl.cardDeckPos.GetChild(0).GetComponent<Card>().ScoreCard;

                        Card findBestCard = Helper.findBestCardFor(sortHandCard, throwedCardScore);

                        if (findBestCard != null)
                        {
                            print("SINGLE CARD");

                            Helper.DeleteThrowCard();

                            GameControl.gameControl.throwedCard.Add(findBestCard);

                            sortHandCard.Remove(findBestCard);

                            GameControl.gameControl.throwedCard[0].transform.SetParent(GameControl.gameControl.cardDeckPos);

                            GameControl.gameControl.throwedCard[0].transform.localPosition = new Vector3(0, 0, 0);

                            GameControl.gameControl.throwedCard[0].GetComponent<SpriteRenderer>().sortingOrder = 2;

                            if (throwCardCondition == false) throwCardCondition = true;

                            Helper.DisableThrowedCondition(otherAiPlayers);

                            endTurnAction = false;

                            aIState = AIState.Endturn;
                        }
                        else
                        {
                            GameControl.flusheOnThrow = true;

                            endTurnAction = false;

                            aIState = AIState.Endturn;
                        }
                    }
                    else
                    {
                        GameControl.flusheOnThrow = true;

                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }
                }
            }
            else if (GameControl.fullHouseOnThrow)
            {
                GameControl.fullHouseOnThrow = false;

                if (containFourOfAKind)
                {
                    print("PAIR");

                    GameControl.fourOfAKindOnThrow = true;

                    Helper.DeleteThrowCard();

                    Helper.ThrowCard(fourOfAKindListCard, sortHandCard);

                    if (throwCardCondition == false) throwCardCondition = true;

                    Helper.DisableThrowedCondition(otherAiPlayers);

                    containFourOfAKind = false;

                    endTurnAction = false;

                    aIState = AIState.Endturn;
                }

                else if (containFullHouse && containFourOfAKind == false)
                {
                    throwedScoreCard = Helper.CalculateThrowCard(GameControl.gameControl.throwedCard);

                    combinationScoreCard = Helper.CalculateCombinationCard(fullHouseListCard);

                    if (throwedScoreCard > combinationScoreCard)
                    {
                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }

                    if (throwedScoreCard < combinationScoreCard)
                    {
                        print("PAIR");

                        GameControl.fullHouseOnThrow = true;

                        Helper.DeleteThrowCard();

                        Helper.ThrowCard(fullHouseListCard, sortHandCard);

                        if (throwCardCondition == false) throwCardCondition = true;

                        Helper.DisableThrowedCondition(otherAiPlayers);

                        containFullHouse = false;

                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }
                }
                else
                {
                    if (GameControl.gameControl.cardDeckPos.childCount == 1)
                    {
                        int throwedCardScore = GameControl.gameControl.cardDeckPos.GetChild(0).GetComponent<Card>().ScoreCard;

                        Card findBestCard = Helper.findBestCardFor(sortHandCard, throwedCardScore);

                        if (findBestCard != null)
                        {
                            print("SINGLE CARD");

                            Helper.DeleteThrowCard();

                            GameControl.gameControl.throwedCard.Add(findBestCard);

                            sortHandCard.Remove(findBestCard);

                            GameControl.gameControl.throwedCard[0].transform.SetParent(GameControl.gameControl.cardDeckPos);

                            GameControl.gameControl.throwedCard[0].transform.localPosition = new Vector3(0, 0, 0);

                            GameControl.gameControl.throwedCard[0].GetComponent<SpriteRenderer>().sortingOrder = 2;

                            if (throwCardCondition == false) throwCardCondition = true;

                            Helper.DisableThrowedCondition(otherAiPlayers);

                            endTurnAction = false;

                            aIState = AIState.Endturn;
                        }
                        else
                        {
                            GameControl.fullHouseOnThrow = true;

                            endTurnAction = false;

                            aIState = AIState.Endturn;
                        }
                    }
                    else
                    {
                        GameControl.fullHouseOnThrow = true;

                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }
                }
            }
            else if (GameControl.fourOfAKindOnThrow)
            {
                GameControl.fourOfAKindOnThrow = false;

                if (containFourOfAKind)
                {
                    throwedScoreCard = Helper.CalculateThrowCard(GameControl.gameControl.throwedCard);

                    combinationScoreCard = Helper.CalculateCombinationCard(fourOfAKindListCard);

                    if (throwedScoreCard > combinationScoreCard)
                    {
                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }

                    if (throwedScoreCard < combinationScoreCard)
                    {
                        print("PAIR");

                        GameControl.fourOfAKindOnThrow = true;

                        Helper.DeleteThrowCard();

                        Helper.ThrowCard(fourOfAKindListCard, sortHandCard);

                        if (throwCardCondition == false) throwCardCondition = true;

                        Helper.DisableThrowedCondition(otherAiPlayers);

                        containFourOfAKind = false;

                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }
                }
                else
                {
                    if (GameControl.gameControl.cardDeckPos.childCount == 1)
                    {
                        int throwedCardScore = GameControl.gameControl.cardDeckPos.GetChild(0).GetComponent<Card>().ScoreCard;

                        Card findBestCard = Helper.findBestCardFor(sortHandCard, throwedCardScore);

                        if (findBestCard != null)
                        {
                            print("SINGLE CARD");

                            Helper.DeleteThrowCard();

                            GameControl.gameControl.throwedCard.Add(findBestCard);

                            sortHandCard.Remove(findBestCard);

                            GameControl.gameControl.throwedCard[0].transform.SetParent(GameControl.gameControl.cardDeckPos);

                            GameControl.gameControl.throwedCard[0].transform.localPosition = new Vector3(0, 0, 0);

                            GameControl.gameControl.throwedCard[0].GetComponent<SpriteRenderer>().sortingOrder = 2;

                            if (throwCardCondition == false) throwCardCondition = true;

                            Helper.DisableThrowedCondition(otherAiPlayers);

                            endTurnAction = false;

                            aIState = AIState.Endturn;
                        }
                        else
                        {
                            GameControl.fourOfAKindOnThrow = true;

                            endTurnAction = false;

                            aIState = AIState.Endturn;
                        }
                    }
                    else
                    {
                        GameControl.fourOfAKindOnThrow = true;

                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }
                }
            }
            else
            {
                if (GameControl.gameControl.cardDeckPos.childCount == 1)
                {
                    int throwedCardScore = GameControl.gameControl.cardDeckPos.GetChild(0).GetComponent<Card>().ScoreCard;

                    Card findBestCard = Helper.findBestCardFor(sortHandCard, throwedCardScore);

                    if (findBestCard != null)
                    {
                        print("SINGLE CARD");

                        Helper.DeleteThrowCard();

                        GameControl.gameControl.throwedCard.Add(findBestCard);

                        sortHandCard.Remove(findBestCard);

                        GameControl.gameControl.throwedCard[0].transform.SetParent(GameControl.gameControl.cardDeckPos);

                        GameControl.gameControl.throwedCard[0].transform.localPosition = new Vector3(0, 0, 0);

                        GameControl.gameControl.throwedCard[0].GetComponent<SpriteRenderer>().sortingOrder = 2;

                        if (throwCardCondition == false) throwCardCondition = true;

                        Helper.DisableThrowedCondition(otherAiPlayers);

                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }
                    else
                    {
                        endTurnAction = false;

                        aIState = AIState.Endturn;
                    }
                }
                else
                {
                    endTurnAction = false;

                    aIState = AIState.Endturn;
                }
            }
        }
        else
        {
            if (containOnePair)
            {
                print("PAIR");

                Helper.DeleteThrowCard();

                Helper.ThrowCard(onePairListCard, sortHandCard);

                if (throwCardCondition == false) throwCardCondition = true;

                Helper.DisableThrowedCondition(otherAiPlayers);

                containOnePair = false;

                GameControl.onePairOnThrow = true;

                endTurnAction = false;

                aIState = AIState.Endturn;
            }

            else if (containTwoPair)
            {
                print("TWO PAIR");

                Helper.DeleteThrowCard();

                Helper.ThrowCard(twoPairListCard, sortHandCard);

                if (throwCardCondition == false) throwCardCondition = true;

                Helper.DisableThrowedCondition(otherAiPlayers);

                containTwoPair = false;

                GameControl.twoPairOnThrow = true;

                endTurnAction = false;

                aIState = AIState.Endturn;
            }

            else if (containThreeOfAKind)
            {
                print("THREE OF KIND");

                Helper.DeleteThrowCard();

                Helper.ThrowCard(threeOfAKindListCard, sortHandCard);

                if (throwCardCondition == false) throwCardCondition = true;

                Helper.DisableThrowedCondition(otherAiPlayers);

                containThreeOfAKind = false;

                GameControl.threeOfAKindOnThrow = true;

                endTurnAction = false;

                aIState = AIState.Endturn;
            }

            else if (containStraight)
            {
                print("STRAIGHT");

                Helper.DeleteThrowCard();

                Helper.ThrowCard(fourOfAKindListCard, sortHandCard);

                if (throwCardCondition == false) throwCardCondition = true;

                Helper.DisableThrowedCondition(otherAiPlayers);

                containStraight = false;

                GameControl.straightOnThrow = true;

                endTurnAction = false;

                aIState = AIState.Endturn;
            }

            else if (containFlush)
            {
                print("FLUSH");

                Helper.DeleteThrowCard();

                Helper.ThrowCard(flushListCardToThrow, sortHandCard);

                if (throwCardCondition == false) throwCardCondition = true;

                Helper.DisableThrowedCondition(otherAiPlayers);

                containFlush = false;

                GameControl.flusheOnThrow = true;

                endTurnAction = false;

                aIState = AIState.Endturn;
            }

            else if (containFullHouse)
            {
                print("FULL HOUSE");

                Helper.DeleteThrowCard();

                Helper.ThrowCard(fullHouseListCard, sortHandCard);

                if (throwCardCondition == false) throwCardCondition = true;

                Helper.DisableThrowedCondition(otherAiPlayers);

                containFullHouse = false;

                GameControl.fullHouseOnThrow = true;

                endTurnAction = false;

                aIState = AIState.Endturn;
            }

            else if (containFourOfAKind)
            {
                print("FOUR OF A KIND");

                Helper.DeleteThrowCard();

                Helper.ThrowCard(fourOfAKindListCard, sortHandCard);

                if (throwCardCondition == false) throwCardCondition = true;

                Helper.DisableThrowedCondition(otherAiPlayers);

                containFourOfAKind = false;

                GameControl.fourOfAKindOnThrow = true;

                endTurnAction = false;

                aIState = AIState.Endturn;
            }
            else
            {
                print("SINGLE CARD");

                Helper.DeleteThrowCard();

                GameControl.gameControl.throwedCard.Add(sortHandCard[0]);

                sortHandCard[0].transform.SetParent(GameControl.gameControl.cardDeckPos);

                sortHandCard[0].transform.localPosition = new Vector3(0, 0, 0);

                sortHandCard[0].GetComponent<SpriteRenderer>().sortingOrder = 2;

                if (throwCardCondition == false) throwCardCondition = true;

                Helper.DisableThrowedCondition(otherAiPlayers);

                sortHandCard.RemoveAt(0);

                endTurnAction = false;

                aIState = AIState.Endturn;

            }
        }

        thinkAction = true;

        return;
    }

    public void ResetAllCondition()
    {
        onePairListCard.Clear();

        twoPairListCard.Clear();

        threeOfAKindListCard.Clear();

        fourOfAKindListCard.Clear();

        straightListCard.Clear();

        flushListCardSPADES.Clear();

        flushListCardHEARTS.Clear();

        flushListCardDIAMONDS.Clear();

        flushListCardCLUBS.Clear();

        flushListCardToThrow.Clear();

        containOnePair = false;

        containTwoPair = false;

        containThreeOfAKind = false;

        containFourOfAKind = false;

        containStraight = false;

        containFlush = false;

        return;
    }

    public void TrackingTotalCardLeft()
    {
        if (this.transform.childCount <= 0)
        {
            StopAllCoroutines();

            GameControl.gameControl.gameState = GameControl.GameState.End;
        }

        return;
    }

    /// <summary>
    /// condition sprites
    /// </summary>
    public void ConditionSpirtes()
    {
        //win
        if (this.transform.childCount <= 5)
        {
            GameControl.someoneWillWin = true;

            aiImage.sprite = conditionSprites[0];
        }

        //idle
        else if (this.transform.childCount > 5 && GameControl.someoneWillWin == false)
        {
            aiImage.sprite = conditionSprites[1];
        }

        //lose
        else if (this.transform.childCount > 5 && GameControl.someoneWillWin)
        {
            aiImage.sprite = conditionSprites[2];
        }

        return;
    }
}

