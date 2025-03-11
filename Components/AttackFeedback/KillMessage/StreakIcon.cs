using System;
using System.Collections;
using UnityEngine;

public class StreakIcon : MonoBehaviour {
    private Animator animator;
    private KillStreakController controller;
    
    private GameObject targetPosition;
    public GameObject TargetPosition {
        get {
            return targetPosition == null ? controller.FirstStreak : targetPosition;
        }
        set {
            if (targetPosition) {
                Destroy(targetPosition);
            }
            targetPosition = value;
        }
    }

    private bool needDestroyWhenOnPosition;
    public bool NeedDestroyWhenOnPosition {
        get => needDestroyWhenOnPosition;
        set {
            if (value) {
                needDestroyWhenOnPosition = value;
                StartCoroutine(DestroyWhenOnPosition());
            }
        }
    }

    private void Start() {
        this.animator = this.GetComponent<Animator>();
    }

    private void Update() {
        if (TargetPosition) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, TargetPosition.transform.position, 200f * Time.deltaTime);
        }
    }
    
    public void InitStrekIcon(GameObject placeholder, KillStreakController controller) {
        this.transform.localPosition = new Vector3(0, 0, 0);
        this.transform.position = placeholder.transform.position;
        this.targetPosition = placeholder;
        this.needDestroyWhenOnPosition = false;
        this.controller = controller;
    }

    public void Destroy(float delay = 0f) {
        Destroy(this.targetPosition.gameObject, delay);
        Destroy(this.gameObject, delay);
    }

    private IEnumerator DestroyWhenOnPosition() {
        while (this.transform.position != this.targetPosition.transform.position) {
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }

    public void Fade() {
        this.animator.SetTrigger("Fade");
        Destroy(0.25f);
    }
}