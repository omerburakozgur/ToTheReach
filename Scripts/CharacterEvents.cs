using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvents
{
    // Character Damaged
    public static UnityAction<GameObject, int> characterDamaged;

    // Character Healed
    public static UnityAction<GameObject, int> characterHealed;

    public static UnityAction<GameObject, int> arrowPickedUp;

    public static UnityAction<int> staminaPotionPickedUp;




}
