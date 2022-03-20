using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Helper
{
    /// <summary>
    ///   Checks whether all items in the enumerable are same (Uses <see cref="object.Equals(object)" /> to check for equality)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerable">The enumerable.</param>
    /// <returns>
    public static bool AreAllSame<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));

        using (var enumerator = enumerable.GetEnumerator())
        {
            var toCompare = default(T);
            if (enumerator.MoveNext())
            {
                toCompare = enumerator.Current;
            }

            while (enumerator.MoveNext())
            {
                if (toCompare != null && !toCompare.Equals(enumerator.Current))
                {
                    return false;
                }
            }
        }

        return true;
    }

    /// <summary>
    /// throwing the combination cards from player hands
    /// </summary>
    /// <param name="cardCombination"></param>
    /// <param name="playerHands"></param>
    public static void ThrowCard(List<Card> cardCombination, List<Card> playerHands, float offset = -0.3f)
    {
        DeleteThrowCard();

        for (int i = 0; i < cardCombination.Count; i++)
        {
            GameControl.gameControl.throwedCard.Add(cardCombination[i]);

            playerHands.Remove(cardCombination[i]);
        }

        if (GameControl.gameControl.throwedCard.Count > 0)
        {
            for (int x = 0; x < GameControl.gameControl.throwedCard.Count; x++)
            {
                GameControl.gameControl.throwedCard[x].transform.SetParent(GameControl.gameControl.cardDeckPos);

                float xPos = offset + 0.2f * x;

                float yPos = 0;

                float zPos = 0;

                GameControl.gameControl.throwedCard[x].transform.localPosition = new Vector3(xPos, yPos, zPos);

                GameControl.gameControl.throwedCard[x].GetComponent<SpriteRenderer>().sortingOrder = 2 + x;

            }
        }
        else
        {
            for (int x = 0; x < cardCombination.Count; x++)
            {
                cardCombination[x].transform.SetParent(GameControl.gameControl.cardDeckPos);

                float xPos = offset + 0.2f * x;

                float yPos = 0;

                float zPos = 0;

                cardCombination[x].transform.localPosition = new Vector3(xPos, yPos, zPos);

                cardCombination[x].GetComponent<SpriteRenderer>().sortingOrder = 2 + x;

            }
        }

        return;
    }

    /// <summary>
    /// calculate throw card score
    /// </summary>
    /// <param name="cardCombination"></param>
    /// <returns></returns>
    public static int CalculateThrowCard(List<Card> cardCombination)
    {
        int throwed = 0;

        for (int i = 0; i < GameControl.gameControl.throwedCard.Count; i++)
        {
            throwed += GameControl.gameControl.throwedCard[i].ScoreCard;
        }



        return throwed;
    }

    /// <summary>
    /// calculate combination card
    /// </summary>
    /// <param name="cardCombination"></param>
    /// <returns></returns>
    public static int CalculateCombinationCard(List<Card> cardCombination)
    {
        int combination = 0;

        for (int x = 0; x < cardCombination.Count; x++)
        {
            combination += cardCombination[x].ScoreCard;
        }

        return combination;
    }

    /// <summary>
    /// delete the throw cards
    /// </summary>
    public static void DeleteThrowCard()
    {
        for (int i = 0; i < GameControl.gameControl.cardDeckPos.childCount; i++)
            GameControl.gameControl.cardDeckPos.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;

        GameControl.gameControl.cardDeckPos.DetachChildren();

        return;
    }

    /// <summary>
    /// stop animation on cards
    /// </summary>
    /// <param name="cards"></param>
    public static void StopAnimationOnCard(List<Card> cards)
    {
        foreach (Card c in cards)
        {
            c.animator.enabled = false;

            c.spriteRenderer.color = new Color(255, 255, 255, 255);
        }

        return;
    }

    /// <summary>
    /// find the best card in hand for the score enemy (if one card vs one card)
    /// </summary>
    /// <param name="playerHands"></param>
    /// <param name="cardEnemyScore"></param>
    /// <returns></returns>
    public static Card findBestCardFor (List<Card> playerHands, int cardEnemyScore)
    {
        for(int i = 0; i < playerHands.Count; i++)
        {
            if(playerHands[i].ScoreCard > cardEnemyScore)
            {
                return playerHands[i];
            }
        }

        return null;
    }

    /// <summary>
    /// copy the card list to another card lists
    /// </summary>
    /// <param name="cardFrom"></param>
    /// <param name="cardTo"></param>
    public static void AddToPrivateList(List<Card> cardFrom, List<Card> cardTo)
    {
        for (int i = 0; i < cardFrom.Count; i++)
        {
            cardTo.Add(cardFrom[i]);
        }

        return;
    }

    /// <summary>
    /// manage to disable the AI players
    /// </summary>
    /// <param name="aiPlayers"></param>
    public static void DisableThrowedCondition (List<AI> aiPlayers)
    {
        for(int i = 0; i < aiPlayers.Count; i++)
        {
            aiPlayers[i].throwCardCondition = false;
        }

        return;
    }

    /// <summary>
    /// crd lists for the player 
    /// </summary>
    /// <param name="playerLists"></param>
    public static void CheckPlayerCardInThrow (List<Card> playerLists)
    {
        //check for one pair


        return;
    }
}
