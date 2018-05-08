using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Publish
{
    void notify(ActorState state, int pos, GameObject actor);
    // 发布函数
    void add(Observer observer);
    // 委托添加事件
    void delete(Observer observer);
    // 委托取消事件
}

public interface Observer
{
    void notified(ActorState state, int pos, GameObject actor);
    // 实现接收函数
}

public enum ActorState { ENTER_AREA, DEATH }

public class Publisher : Publish {

    private delegate void ActionUpdate(ActorState state, int pos, GameObject actor);
    private ActionUpdate updatelist;
    // 委托定义

    //单例模式
    private static Publish _instance;
    public static Publish getInstance()
    {
        if (_instance == null) _instance = new Publisher();
        return _instance;
    }

    public void notify(ActorState state, int pos, GameObject actor)
    {
        if (updatelist != null) updatelist(state, pos, actor);
        // 发布信息
    }

    public void add(Observer observer)
    {
        updatelist += observer.notified;
    }

    public void delete(Observer observer)
    {
        updatelist -= observer.notified;
    }
}
