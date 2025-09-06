using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoClipManager : MonoBehaviour
{

    public GameObject videoClipAddButterInBowl;
    public GameObject videoClipAddEggInBowl;
    public GameObject videoClipBakeCake;
    public GameObject videoClipBeatTheEgg;
    public GameObject videoClipMixTheBatter;
    public GameObject videoClipPourBatterInPlate;
    public GameObject videoClipPourButterInCup;
    public GameObject videoClipPourEggInCup;
    public GameObject videoClipPourFlourInCup;



    // Start is called before the first frame update
    void Start()
    {
        DisableAllVideoClip();
    }

    public void DisableAllVideoClip() {
        if (videoClipAddButterInBowl != null)
        {
            videoClipAddButterInBowl.SetActive(false);
        }
        if (videoClipAddEggInBowl != null)
        {
            videoClipAddEggInBowl.SetActive(false);
        }
        if (videoClipBakeCake != null)
        {
            videoClipBakeCake.SetActive(false);
        }
        if (videoClipBeatTheEgg != null)
        {
            videoClipBeatTheEgg.SetActive(false);
        }
        if (videoClipMixTheBatter != null)
        {
            videoClipMixTheBatter.SetActive(false);
        }
        if (videoClipPourBatterInPlate != null)
        {
            videoClipPourBatterInPlate.SetActive(false);
        }
        if (videoClipPourButterInCup != null)
        {
            videoClipPourButterInCup.SetActive(false);
        }
        if (videoClipPourEggInCup != null)
        {
            videoClipPourEggInCup.SetActive(false);
        }
        if (videoClipPourFlourInCup != null)
        {
            videoClipPourFlourInCup.SetActive(false);
        }
    }


    public Dictionary<string, object> PlayVideoClip(string clipName)
    {
        DisableAllVideoClip();
        switch (clipName)
        {
            case "AddButterInBowl":
                if (videoClipAddButterInBowl != null)
                {
                    videoClipAddButterInBowl.SetActive(true);
                    return new Dictionary<string, object> {
                        { "status", "success" },
                        { "message", $"The videoclip '{clipName}' is playing." }
                    };
                }
                else
                {
                    return new Dictionary<string, object> {
                        { "status", "error" },
                        { "message", $"The videoclip '{clipName}' does not exist in this Unity 3D scene." }
                    };
                }

            case "AddEggInBowl":
                if (videoClipAddEggInBowl != null)
                {
                    videoClipAddEggInBowl.SetActive(true);
                    return new Dictionary<string, object> {
                        { "status", "success" },
                        { "message", $"The videoclip '{clipName}' is playing." }
                    };
                }
                else
                {
                    return new Dictionary<string, object> {
                        { "status", "error" },
                        { "message", $"The videoclip '{clipName}' does not exist in this Unity 3D scene." }
                    };
                }

            case "BakeCake":
                if (videoClipBakeCake != null)
                {
                    videoClipBakeCake.SetActive(true);
                    return new Dictionary<string, object> {
                        { "status", "success" },
                        { "message", $"The videoclip '{clipName}' is playing." }
                    };
                }
                else
                {
                    return new Dictionary<string, object> {
                        { "status", "error" },
                        { "message", $"The videoclip '{clipName}' does not exist in this Unity 3D scene." }
                    };
                }

            case "BeatTheEgg":
                if (videoClipBeatTheEgg != null)
                {
                    videoClipBeatTheEgg.SetActive(true);
                    return new Dictionary<string, object> {
                        { "status", "success" },
                        { "message", $"The videoclip '{clipName}' is playing." }
                    };
                }
                else
                {
                    return new Dictionary<string, object> {
                        { "status", "error" },
                        { "message", $"The videoclip '{clipName}' does not exist in this Unity 3D scene." }
                    };
                }

            case "MixTheBatter":
                if (videoClipMixTheBatter != null)
                {
                    videoClipMixTheBatter.SetActive(true);
                    return new Dictionary<string, object> {
                        { "status", "success" },
                        { "message", $"The videoclip '{clipName}' is playing." }
                    };
                }
                else
                {
                    return new Dictionary<string, object> {
                        { "status", "error" },
                        { "message", $"The videoclip '{clipName}' does not exist in this Unity 3D scene." }
                    };
                }

            case "PourBatterInPlate":
                if (videoClipPourBatterInPlate != null)
                {
                    videoClipPourBatterInPlate.SetActive(true);
                    return new Dictionary<string, object> {
                        { "status", "success" },
                        { "message", $"The videoclip '{clipName}' is playing." }
                    };
                }
                else
                {
                    return new Dictionary<string, object> {
                        { "status", "error" },
                        { "message", $"The videoclip '{clipName}' does not exist in this Unity 3D scene." }
                    };
                }

            case "PourButterInCup":
                if (videoClipPourButterInCup != null)
                {
                    videoClipPourButterInCup.SetActive(true);
                    return new Dictionary<string, object> {
                        { "status", "success" },
                        { "message", $"The videoclip '{clipName}' is playing." }
                    };
                }
                else
                {
                    return new Dictionary<string, object> {
                        { "status", "error" },
                        { "message", $"The videoclip '{clipName}' does not exist in this Unity 3D scene." }
                    };
                }

            case "PourEggInCup":
                if (videoClipPourEggInCup != null)
                {
                    videoClipPourEggInCup.SetActive(true);
                    return new Dictionary<string, object> {
                        { "status", "success" },
                        { "message", $"The videoclip '{clipName}' is playing." }
                    };
                }
                else
                {
                    return new Dictionary<string, object> {
                        { "status", "error" },
                        { "message", $"The videoclip '{clipName}' does not exist in this Unity 3D scene." }
                    };
                }

            case "PourFlourInCup":
                if (videoClipPourFlourInCup != null)
                {
                    videoClipPourFlourInCup.SetActive(true);
                    return new Dictionary<string, object> {
                        { "status", "success" },
                        { "message", $"The videoclip '{clipName}' is playing." }
                    };
                }
                else
                {
                    return new Dictionary<string, object> {
                        { "status", "error" },
                        { "message", $"The videoclip '{clipName}' does not exist in this Unity 3D scene." }
                    };
                }

            default:
                Debug.Log($"No matching video clip found for '{clipName}'.");
                return new Dictionary<string, object> {
                    { "status", "error" },
                    { "message", $"The videoclip '{clipName}' was not found." }
                };
        }
    }
}
