using System.Collections;
using System.Collections.Generic;
using _Scripts.Core.Game.Data;
using UnityEngine;

public class AreaLock_LV3 : MonoBehaviour
{
    public void Start()
    {
        gameObject.SetActive(false);
        if(!GameSaveManager.Instance.GetActiveGameSave().storyData.IsEventComplete(StoryConfig.KEY_STORY_3_END_CUTSCENE))
        {
            GameController.Instance.OnGameEvent += OnGameEvent;
        }
    }

    public void OnGameEvent(string eventId, System.Object info)
    {
        if(eventId == StoryConfig.KEY_STORY_3_START_CUTSCENE)
        {
            gameObject.SetActive(true);
        }
        else if(eventId == StoryConfig.KEY_STORY_3_END_CUTSCENE)
        {
            GameController.Instance.OnGameEvent -= OnGameEvent;
            gameObject.SetActive(false);
        }
    }
}
