﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;

namespace EvilutionClass
{

    /// <summary>
    /// StoryBoard class.  This class contains all the scenes inside the application.  This class also dictates what the CurrentScene is.
    /// </summary>
    public static class StoryBoard
    {
        // The Dictionary (list) of scenes inside the StoryBoard
        public static Dictionary<string, GenericScene> SceneDictionary = new Dictionary<string, GenericScene>();

        /// <summary>
        /// Update the StoryBoard.
        /// </summary>
        /// <param name="dt">The delta time.</param>
        /// <param name="input">The input item if any.</param>
        public static bool Update(TimeSpan dt, GenericMessage message)
        {
            
            // The STORYBOARD should only response to Low Level Game Events like SceneSwitch
            if (message is Message_SceneSwitch)
            {
                if (null != CurrentScene.mp)
                {
                    CurrentScene.mp.Pause();
                    CurrentScene.mp.PlaybackSession.Position = TimeSpan.Zero;
                }

                Message_SceneSwitch mss = message as Message_SceneSwitch;

                // find the scene
                if (SceneDictionary.ContainsKey(mss.TargetScene))
                {
                    // push the scene on to the history
                    SceneHistory.Push(CurrentScene);

                    // switch and kill the message
                    CurrentScene = SceneDictionary[mss.TargetScene];
                    CurrentScene.Reset();

                    if(AudioManager.AudioDictionary.TryGetValue(mss.TargetScene, out CurrentScene.mp))
                        CurrentScene.mp.Play();
                }

                return true;
            }
            else if (message is Message_GoBack)
            {
                // check to see if the history list is empty
                if (SceneHistory.Count == 0)
                {
                    //Do nothing
                }
                else
                {
                    // Pop the scene from the history list and set it as the current scene
                    CurrentScene = SceneHistory.Pop();
                    CurrentScene.Reset();
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Add a scene into the StoryBoard.  Applies a key for easy access.
        /// </summary>
        /// <param name="key">A unique name for the scene.</param>
        /// <param name="scene">The scene to add to the SceneDictionary.</param>
        /// <returns>Returns true on success.  False otherwise.</returns>
        public static bool AddScene(GenericScene gs)
        {
            int size_before_add = SceneDictionary.Count;
            SceneDictionary.Add(gs.Name, gs);
            int size_after_add = SceneDictionary.Count;

            return (size_after_add > size_before_add);

        }

        /// <summary>
        /// The CurrentScene.
        /// </summary>
        public static GenericScene CurrentScene { get; set; }


        /// <summary>
        /// The StoryBoard history.  This acts like a something of a web history.
        /// </summary>
        public static Stack<GenericScene> SceneHistory = new Stack<GenericScene>();


    }
}