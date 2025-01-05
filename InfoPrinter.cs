using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExpShare;

public class InfoPrinter : MonoBehaviour {
    private struct Message {
        public string Text;
        public float Duration;
    }

    private readonly Queue<Message> messageQueue = new Queue<Message>();
    
    private void Start() {
        Plugin.Logger.LogInfo("InfoPrinter is starting!");
    }
    
    private void Update() {
        if (messageQueue.Count == 0) return;
        
        Queue<Message> tempQueue = new Queue<Message>();

        while (messageQueue.Count > 0) {
            var message = messageQueue.Dequeue();
            message.Duration -= Time.deltaTime;
            if (message.Duration > 0) {
                tempQueue.Enqueue(message);
            }
        }

        while (tempQueue.Count > 0) {
            messageQueue.Enqueue(tempQueue.Dequeue());
        }
    }

    private void OnGUI() {
        if (messageQueue.Count == 0) return;
        

        int yOffSet = 10;
        foreach (var message in messageQueue.Reverse()) {
            float alpha = message.Duration / 10;
            GUIStyle style = new GUIStyle {fontSize = 16, normal = {textColor = new Color(1, 1, 1, alpha)}};
            
            GUI.Label(new Rect(10, yOffSet, 500, 20), message.Text, style);
            
            yOffSet += 20;
        }
    }
    
    public void AddMessage(string text, float duration) {
        messageQueue.Enqueue(new Message {Text = text, Duration = duration});
    }
}