﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour {

    public void Switch(string scene) {
        SceneManager.LoadScene(scene);
    }

    public void Exit() {
        Application.Quit();
    }

}
