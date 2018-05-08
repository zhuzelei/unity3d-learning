using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController {

    public Text infoText;

	public void loseGame()
    {
        infoText.text = "LOSE!";
    }

    public void resetGame()
    {
        infoText.text = "";
    }
}
