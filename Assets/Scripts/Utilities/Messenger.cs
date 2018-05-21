using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messenger
{
    #region Singleton
    private static Messenger inst = null;

    private Messenger()
    {

    }

    public static Messenger GetInstance()
    {
        if(inst == null)
        {
            inst = new Messenger();
        }
        return inst;
    }
    #endregion

    Dictionary<string, List<MessageListener>> mListeners = new Dictionary<string, List<MessageListener>>();
    public delegate void MessageListener(Message msg);

    public void RegisterListener(Message message, MessageListener action)
    {
        if(mListeners.ContainsKey(message.MsgName) == false)
        {
            mListeners.Add(message.MsgName, new List<MessageListener>());
        }

        mListeners[message.MsgName].Add(action);
    }

    public void UnregisterListener(Message message, MessageListener action)
    {
        if (mListeners.ContainsKey(message.MsgName))
        {
            mListeners[message.MsgName].Remove(action);
        }
    }

    public void BroadCastMessage(Message msg)
    {
        if (mListeners.ContainsKey(msg.MsgName))
        {
            List<MessageListener> actions = mListeners[msg.MsgName];
            foreach(var act in actions)
            {
                act(msg);
            }
        }
    }
}
