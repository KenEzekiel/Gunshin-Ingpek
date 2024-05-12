using System;
using System.Collections.Generic;
using UnityEngine;

public class CheatCommand
{
    private static readonly StatEffect x2SpeedEffect = new(
        "x2_speed",
        StatEffectType.SPEED,
        StatEffectType.MULTIPLICATION,
        2f
    );

    private static int GetCurrentLevel()
    {
        if (!GameSaveManager.Instance.GetActiveGameSave().storyData.IsEventComplete(StoryConfig.KEY_STORY_2_START_CUTSCENE))
        {
            return 1;
        }
        else if (!GameSaveManager.Instance.GetActiveGameSave().storyData.IsEventComplete(StoryConfig.KEY_STORY_3_START_CUTSCENE))
        {
            return 2;
        }
        else if (!GameSaveManager.Instance.GetActiveGameSave().storyData.IsEventComplete(StoryConfig.KEY_STORY_4_START_CUTSCENE))
        {
            return 3;
        }
        else
        {
            return 4;
        }
    }

    public static readonly Dictionary<string, Action> cheatCommands = new(){
        { "no_damage",
            () => {
                GameController.Instance.cheatController.NO_DAMAGE = true;
            }
        },
        { "one_hit_kill",
            () => {
                GameController.Instance.cheatController.ONE_HIT_KILL = true;
            }
        },
        { "motherlode",
            () => {
                GameController.Instance.cheatController.MOTHERLODE = true;
            }
        },
        { "x2_speed",
            () => {
                GameController.Instance.player.effects.Add(x2SpeedEffect);
            }
        },
        { "full_hp_pet",
            () => {
                GameController.Instance.cheatController.FULL_HP_PET = true;
            }
        },
        { "kill_pet",
            () => {
                GameController.Instance.player.StartCoroutine(
                    GameController.Instance.player.KillAllCompanions()
                );
            }
        },
        { "orb",
            () => {
                // Generate a random orb near the player, not on the player
                Vector3 orbPos = GameController.Instance.player.transform.position + new Vector3(1, 0, 0);
                Orb.GenerateRandomOrb(orbPos, "Random Cheat Orb");
            }
        },
        { "skip",
            () => {
                int currentLevel = GetCurrentLevel();

                Debug.Log("Current level: " + currentLevel);

                if (currentLevel == 1)
                {
                    GameSaveManager.Instance.GetActiveGameSave().storyData.CompleteEvent(StoryConfig.KEY_STORY_1_START_CUTSCENE);
                    GameSaveManager.Instance.GetActiveGameSave().storyData.CompleteEvent(StoryConfig.KEY_STORY_1_ENTER_DUNGEON);
                    GameSaveManager.Instance.GetActiveGameSave().storyData.CompleteEvent(StoryConfig.KEY_STORY_1_END_CUTSCENE);
                    GameSaveManager.Instance.GetActiveGameSave().currencyData.AddTransaction(Level1.QUEST_REWARD, Level1.QUEST_NAME);

                    // Teleport to start of level 2
                    Vector3 position = new(-33.1f, 0.073f, 190.17f);
                    GameController.Instance.player.transform.position = position;
                    GameController.Instance.player.TeleportAllCompanions(position);
                } else if (currentLevel == 2)
                {
                    GameSaveManager.Instance.GetActiveGameSave().storyData.CompleteEvent(StoryConfig.KEY_STORY_2_START_CUTSCENE);
                    GameSaveManager.Instance.GetActiveGameSave().storyData.CompleteEvent(StoryConfig.KEY_STORY_2_END_CUTSCENE);
                    GameSaveManager.Instance.GetActiveGameSave().currencyData.AddTransaction(Level2.QUEST_REWARD, Level2.QUEST_NAME);
                    
                    // Teleport to start of level 3
                    Vector3 position = new(-206.1f, 0.073f, 239.76f);
                    GameController.Instance.player.transform.position = position;
                    GameController.Instance.player.TeleportAllCompanions(position);
                } else if (currentLevel == 3)
                {
                    GameSaveManager.Instance.GetActiveGameSave().storyData.CompleteEvent(StoryConfig.KEY_STORY_3_START_CUTSCENE);
                    GameSaveManager.Instance.GetActiveGameSave().storyData.CompleteEvent(StoryConfig.KEY_STORY_3_END_CUTSCENE);
                    GameSaveManager.Instance.GetActiveGameSave().currencyData.AddTransaction(Level3.QUEST_REWARD, Level3.QUEST_NAME);
                    
                    // Teleport to start of level 4
                    Vector3 position = new(-245.13f, 3.685f, 195.6f);
                    GameController.Instance.player.transform.position = position;
                    GameController.Instance.player.TeleportAllCompanions(position);
                } else if (currentLevel == 4)
                {
                    GameSaveManager.Instance.GetActiveGameSave().storyData.CompleteEvent(StoryConfig.KEY_STORY_4_START_CUTSCENE);
                    GameController.Instance.StartCutscene(StoryConfig.KEY_STORY_ENDING_CUTSCENE);
                    GameSaveManager.Instance.GetActiveGameSave().storyData.CompleteEvent(StoryConfig.KEY_STORY_ENDING_CUTSCENE);
                    GameController.Instance.StartCutscene(StoryConfig.KEY_STORY_ENDING_AFTER_CUTSCENE);
                    GameSaveManager.Instance.GetActiveGameSave().storyData.CompleteEvent(StoryConfig.KEY_STORY_ENDING_AFTER_CUTSCENE);
                    GameController.Instance.stateController.PopState();
                    GameController.Instance.stateController.PushState(GameState.FINISH);
                }
            }
        },
    };
}
