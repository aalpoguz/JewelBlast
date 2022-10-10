using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MatchController : MonoBehaviour
{
    Board board;
    public List<Jewels> FoundedJewelList = new List<Jewels>();

    private void Awake()
    {
        board = Object.FindObjectOfType<Board>();
    }

    public void FindMatches()
    {
        FoundedJewelList.Clear();

        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                Jewels validJewel = board.allJewels[x, y];

                if (validJewel != null)
                {
                    // X line match control
                    if (x > 0 && x < board.width - 1)
                    {
                        Jewels leftJewel = board.allJewels[x - 1, y];
                        Jewels rightJewel = board.allJewels[x + 1, y];
                        if (leftJewel != null && rightJewel != null)
                        {
                            if (leftJewel.type == validJewel.type && rightJewel.type == validJewel.type)
                            {
                                validJewel.isMatch = true;
                                leftJewel.isMatch = true;
                                rightJewel.isMatch = true;
                                FoundedJewelList.Add(validJewel);
                                FoundedJewelList.Add(leftJewel);
                                FoundedJewelList.Add(rightJewel);

                            }
                        }
                    }

                    // Y line match control
                    if (y > 0 && y < board.height - 1)
                    {
                        Jewels downJewel = board.allJewels[x, y - 1];
                        Jewels upJewel = board.allJewels[x, y + 1];

                        if (downJewel != null && upJewel != null)
                        {
                            if (downJewel.type == validJewel.type && upJewel.type == validJewel.type)
                            {
                                validJewel.isMatch = true;
                                downJewel.isMatch = true;
                                upJewel.isMatch = true;

                                FoundedJewelList.Add(validJewel);
                                FoundedJewelList.Add(downJewel);
                                FoundedJewelList.Add(upJewel);
                            }
                        }

                    }
                }
            }
        }//loops done

        if (FoundedJewelList.Count > 0)
        {
            FoundedJewelList = FoundedJewelList.Distinct().ToList();
        }
    }
}
