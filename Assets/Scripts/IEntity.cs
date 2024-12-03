using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Cinemachine;
using SingularityGroup.HotReload;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public enum EntityType
{
    Player,
    Enemy,
    Boss
}

public interface IEntity
{
    EntityType EntityType { get; set; }
    float Mana { get; set; }
    float Health { get; set; }
    void TakeDamage(float damage);
    void Die();
    void Move(Vector3 movement);
    void Freeze();

    void RegisterEvents(GameEventManager gameEventManager);


}