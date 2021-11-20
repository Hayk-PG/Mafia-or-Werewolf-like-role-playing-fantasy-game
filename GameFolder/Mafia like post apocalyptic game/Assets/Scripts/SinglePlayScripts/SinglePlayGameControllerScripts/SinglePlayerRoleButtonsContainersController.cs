using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerRoleButtonsContainersController : MonoBehaviour
{
    [Serializable] public class Transforms
    {
        [SerializeField] Transform gameUI;
        [SerializeField] List<Transform> activeRoleButtonsContainers;
        [SerializeField] Transform lostRoleButtonsContainers;
        [SerializeField] Transform playerRoleButtonContainer;

        [SerializeField] Transform holyMissileVfxDest;

        public Transform GameUI
        {
            get => gameUI;
        }
        public List<Transform> ActiveRoleButtonsContainers
        {
            get => activeRoleButtonsContainers;
        }
        public Transform LostRoleButtonsContainers
        {
            get => lostRoleButtonsContainers;
        }
        public Transform PlayerRoleButtonContainer
        {
            get => playerRoleButtonContainer;
        }

        public Transform HolyMissileVfxDest
        {
            get => holyMissileVfxDest;
        }
    }
    [Serializable] public class UI
    {
        [SerializeField] Button seeActivePlayers;
        [SerializeField] Button seeLostPlayers;

        [SerializeField] Image activePlayersEyeIcon;
        [SerializeField] Image lostPlayersEyeIcon;

        [SerializeField] Sprite[] eyeIconsPrefab;

        [SerializeField] CanvasGroup[] activePlayersCanvasGroups;
        [SerializeField] CanvasGroup lostPlayersCanvasGroup;

        public Button SeeActivePlayers
        {
            get => seeActivePlayers;
        }
        public Button SeeLostPlayers
        {
            get => seeLostPlayers;
        }

        public Sprite[] EyeIconsPrefab
        {
            get => eyeIconsPrefab;
        }
        public Sprite ActivePlayersEyeIcon
        {
            get => activePlayersEyeIcon.sprite;
            set => activePlayersEyeIcon.sprite = value;
        }
        public Sprite LostPlayersEyeIcon
        {
            get => lostPlayersEyeIcon.sprite;
            set => lostPlayersEyeIcon.sprite = value;
        }

        public CanvasGroup[] ActivePlayersCanvasGroups
        {
            get => activePlayersCanvasGroups;
        }
        public CanvasGroup LostPlayersCanvasGroup
        {
            get => lostPlayersCanvasGroup;
        }
    }
    [Serializable] public class VFX
    {
        [SerializeField] GameObject holyMissileVFX;

        internal GameObject HolyMissileVFXPrefab
        {
            get => holyMissileVFX;
        }
    }
    [Serializable] public class Animators
    {
        [SerializeField] Animator switchLostPlayersScreenButtonAnim;

        public Animator SwitchLostPlayersScreenButtonAnim
        {
            get => switchLostPlayersScreenButtonAnim;
        }
    }

    public Transforms _Transforms;
    public UI _UI;
    public VFX _VFX;
    public Animators _Animators;



    void Update()
    {
        OnClickSwitchViewButton(_UI.SeeActivePlayers);
        OnClickSwitchViewButton(_UI.SeeLostPlayers);
    }

    public void OnPlayerLost(Transform roleButtonTransform)
    {
        StartCoroutine(MoveToLostPlayersContainerCoroutine(roleButtonTransform));
    }

    IEnumerator MoveToLostPlayersContainerCoroutine(Transform roleButtonTransform)
    {
        if (!roleButtonTransform.GetComponent<SinglePlayRoleButton>().IsPlayer)
        {
            yield return new WaitForSeconds(5);

            int indexCount = _Transforms.ActiveRoleButtonsContainers.Count - 1;
            int RoleButtonParentIndex = _Transforms.ActiveRoleButtonsContainers.IndexOf(roleButtonTransform.parent);
            int nextContainerIndex = RoleButtonParentIndex < indexCount ? RoleButtonParentIndex + 1 : indexCount;

            MoveRoleButton(roleButtonTransform);

            for (int i = nextContainerIndex; i < _Transforms.ActiveRoleButtonsContainers.Count; i++)
            {
                int currentContainerIndex = i;
                int previousContainerIndex = currentContainerIndex > 0 ? currentContainerIndex - 1 : 0;

                if (_Transforms.ActiveRoleButtonsContainers[currentContainerIndex].childCount > 0 && _Transforms.ActiveRoleButtonsContainers[previousContainerIndex].childCount < 5)
                {
                    _Transforms.ActiveRoleButtonsContainers[currentContainerIndex].GetChild(0).transform.SetParent(_Transforms.ActiveRoleButtonsContainers[previousContainerIndex]);                    
                }
            }
        }
    }

    void MoveRoleButton(Transform roleButtonTransform)
    {
        GameObject holyMissileVfx = Instantiate(_VFX.HolyMissileVFXPrefab, new Vector3(roleButtonTransform.position.x, roleButtonTransform.position.y, -100), Quaternion.identity, _Transforms.GameUI);
        roleButtonTransform.SetParent(_Transforms.LostRoleButtonsContainers);
    }

    void OnClickSwitchViewButton(Button button)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => 
        {
            if (button == _UI.SeeActivePlayers)
            {
                button.interactable = false;

                _UI.SeeLostPlayers.interactable = true;
                _UI.ActivePlayersEyeIcon = _UI.EyeIconsPrefab[0];
                _UI.LostPlayersEyeIcon = _UI.EyeIconsPrefab[1];

                foreach (var canvasGroup in _UI.ActivePlayersCanvasGroups)
                {
                    MyCanvasGroups.CanvasGroupActivity(canvasGroup, true);
                }

                MyCanvasGroups.CanvasGroupActivity(_UI.LostPlayersCanvasGroup, false);
            }
            if (button == _UI.SeeLostPlayers)
            {
                button.interactable = false;

                _UI.SeeActivePlayers.interactable = true;
                _UI.ActivePlayersEyeIcon = _UI.EyeIconsPrefab[1];
                _UI.LostPlayersEyeIcon = _UI.EyeIconsPrefab[0];

                foreach (var canvasGroup in _UI.ActivePlayersCanvasGroups)
                {
                    MyCanvasGroups.CanvasGroupActivity(canvasGroup, false);
                }

                MyCanvasGroups.CanvasGroupActivity(_UI.LostPlayersCanvasGroup, true);
            }
        });
    }
}
