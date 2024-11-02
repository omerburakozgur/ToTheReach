using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimationStrings
{
    // Movement
    public static string isMoving = "isMoving";
    public static string isRunning = "isRunning";
    public static string jumpTrigger = "jump";
    internal static string isSliding = "isSliding";

    // Animations
    public static string launch = "launch";
    internal static string collectTrigger = "collectTrigger";

    // Values
    public static string yVelocity = "yVelocity";

    // Combat
    internal static string attackTrigger = "attack";
    internal static string rangedAttackTrigger = "rangedAttack";
    internal static string isHit = "isHit";
    internal static string hitTrigger = "hit";
    internal static string attackCooldown = "attackCooldown";
    internal static string spawnCooldown = "spawnCooldown";
    internal static string spawnedEnemy = "spawnedEnemy";
    internal static string specialAttackTrigger = "spAttack";

    // Health Status 
    internal static string isAlive = "isAlive";

    // Checks
    public static string isGrounded = "isGrounded";
    public static string isOnWall = "isOnWall";
    public static string isOnCeiling = "isOnCeiling";
    internal static string canMove = "canMove";
    internal static string lockVelocity = "lockVelocity";

    // Target Check
    internal static string hasTarget = "hasTarget";
    internal static string hasRangedTarget = "hasRangedTarget";
    internal static string hasRangedTargetVertical = "hasRangedTargetVertical";
    internal static string projectileHitTrigger = "projectileHit";
}
