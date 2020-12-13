﻿using System;
using UnityEngine;
using UnityEngine.Rendering;

public class EntityFactory : MonoBehaviour {

    public Entity Spawn(EntityData data) {
        switch (data.type) {
            case EntityType.PC:
                return SpawnPC(data);
            case EntityType.NPC:
                return SpawnNPC(data);
            default:
                return null;
        }
    }

    private Entity SpawnPC(EntityData data) {
        var player = new GameObject(data.name);
        player.layer = LayerMask.NameToLayer("Characters");
        player.transform.localScale = new Vector3(2f, 2f, 2f);
        var entity = player.AddComponent<Entity>();
        entity.Init(data);

        var body = new GameObject("Body");
        body.layer = LayerMask.NameToLayer("Characters");
        body.transform.SetParent(player.transform, false);
        body.transform.localPosition = Vector3.zero;
        body.AddComponent<Billboard>();
        body.AddComponent<SortingGroup>();

        var head = new GameObject("Head");
        head.layer = LayerMask.NameToLayer("Characters");
        head.transform.SetParent(body.transform, false);
        head.transform.localPosition = Vector3.zero;

        var bodyViewer = body.AddComponent<EntityViewer>();
        var headViewer = head.AddComponent<EntityViewer>();

        entity.EntityViewer = bodyViewer;
        entity.Type = EntityType.PC;
        entity.ActionTable = new SpriteAction.PC() as SpriteAction;
        entity.ShadowSize = 0.5f;
        // Add more options such as sex etc

        bodyViewer._ViewerType = EntityViewer.ViewerType.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.Children.Add(headViewer);
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.CurrentMotion = SpriteMotion.Idle;
        bodyViewer.Type = entity.Type;

        headViewer.Parent = bodyViewer;
        headViewer.Entity = entity;
        headViewer.SpriteOrder = 1;
        headViewer.Type = entity.Type;
        headViewer._ViewerType = EntityViewer.ViewerType.HEAD;

        return entity;
    }

    private Entity SpawnNPC(EntityData data) {
        var npc = new GameObject(data.name);
        npc.layer = LayerMask.NameToLayer("Characters");
        npc.transform.localScale = new Vector3(2f, 2f, 2f);
        var entity = npc.AddComponent<Entity>();
        entity.Init(data);

        var body = new GameObject("Body");
        body.layer = LayerMask.NameToLayer("Characters");
        body.transform.SetParent(npc.transform, false);
        body.transform.localPosition = Vector3.zero;
        body.AddComponent<Billboard>();
        body.AddComponent<SortingGroup>();

        var bodyViewer = body.AddComponent<EntityViewer>();

        entity.EntityViewer = bodyViewer;
        entity.ActionTable = new SpriteAction.NPC() as SpriteAction;

        bodyViewer._ViewerType = EntityViewer.ViewerType.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.CurrentMotion = SpriteMotion.Idle;
        bodyViewer.Type = entity.Type;

        return entity;
    }

    public Entity SpawnPlayer(CharacterData data) {
        var player = new GameObject(data.Name);
        player.layer = LayerMask.NameToLayer("Characters");
        player.transform.localScale = new Vector3(2f, 2f, 2f);
        var entity = player.AddComponent<Entity>();
        entity.Init(data);

        var body = new GameObject("Body");
        body.layer = LayerMask.NameToLayer("Characters");
        body.transform.SetParent(player.transform, false);
        body.transform.localPosition = Vector3.zero;
        body.AddComponent<Billboard>();
        body.AddComponent<SortingGroup>();

        var head = new GameObject("Head");
        head.layer = LayerMask.NameToLayer("Characters");
        head.transform.SetParent(body.transform, false);
        head.transform.localPosition = Vector3.zero;

        var bodyViewer = body.AddComponent<EntityViewer>();
        var headViewer = head.AddComponent<EntityViewer>();

        entity.EntityViewer = bodyViewer;
        entity.ActionTable = new SpriteAction.PC() as SpriteAction;
        entity.ShadowSize = 0.5f;
        // Add more options such as sex etc

        bodyViewer._ViewerType = EntityViewer.ViewerType.BODY;
        bodyViewer.Entity = entity;
        bodyViewer.Children.Add(headViewer);
        bodyViewer.SpriteOffset = 0.5f;
        bodyViewer.HeadDirection = 0;
        bodyViewer.CurrentMotion = SpriteMotion.Idle;
        bodyViewer.Type = entity.Type;

        headViewer.Parent = bodyViewer;
        headViewer.Entity = entity;
        headViewer.SpriteOrder = 1;
        headViewer.Type = entity.Type;
        headViewer._ViewerType = EntityViewer.ViewerType.HEAD;

        return entity;
    }
}