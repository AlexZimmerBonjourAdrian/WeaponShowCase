using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public sealed class GoapAgent : MonoBehaviour
{

    private FSM stateMachine;

    private FSM.FSMState idleState;
    private FSM.FSMState moveToState;
    private FSM.FSMState staticToState;
    private FSM.FSMState performActionState;


    private HashSet<GoapAction> availableAction;

    private Queue<GoapAction> currentActions;

    private IGoap dataProvider;

    private GoapPlanner planner;
   
    public FindOfView Range;
    void Start()
    {
        stateMachine = new FSM();
        availableAction = new HashSet<GoapAction>();
        currentActions = new Queue<GoapAction>();
        planner = new GoapPlanner();
        Range = GameObject.Find("Character1_Head").GetComponent<FindOfView>();
        findDataProvider();
        createIdleState();
        createMoveToState();
        createStaticToState();
        createPerformActionState();
        stateMachine.pushState(idleState);
        loadActions();



    }
   

    void Update()
    {
        stateMachine.Update(this.gameObject);
    }
    public void addAction(GoapAction action)
    {
        availableAction.Add (action);
    }

    public GoapAction getAction(Type action)
    {
        foreach (GoapAction currAction in availableAction)
        {
            if (currAction.GetType().Equals(action))
            {
                return currAction;
            }
        }

        return null;
    }

    public void removeAction(GoapAction action)
    {
        availableAction.Remove(action);
    }

    private bool hasActionPlan()
    {
        return currentActions.Count > 0;
    }

    private void createIdleState()
    {

        
        idleState = (fsm, gameObj) =>
        {
            HashSet<KeyValuePair<string, object>> worldState = dataProvider.getWorldState();
            HashSet<KeyValuePair<string, object>> goal = dataProvider.createGoalState();
            Queue<GoapAction> plan = planner.plan (gameObject, availableAction, worldState, goal);

            if (plan != null)
            {
                currentActions = plan;
                dataProvider.planFound(goal, plan);


                fsm.popState();
                fsm.pushState(performActionState);
            }
            else
            {
                dataProvider.planFailed(goal);
                fsm.popState();
                fsm.pushState(idleState);
            }
        };
    }
    private void createPerformActionState()
    {

        performActionState = (fsm, gameObj) =>
        {

            if (!hasActionPlan())
            {
                fsm.popState();
                fsm.pushState(idleState);
                dataProvider.actionsFinished();
                return;
            }

            GoapAction action = currentActions.Peek();
            if (action.isDone())
            {
                currentActions.Dequeue();
            }

            if (hasActionPlan())
            {
                bool isStatic = action.requiredIsStatic() ? action.isStatic() : true;
                action = currentActions.Peek();
                bool inRange = action.requiresInRange() ? action.isInRange() : true;
                //bool isStatic = action.requiredIsStatic() ? action.isStatic() : false;
                if(isStatic)
                {
                    if (inRange)
                    {
                        bool success = action.perform(gameObj);
                        if (!success)
                        {
                            fsm.popState();
                            fsm.pushState(idleState);
                            createIdleState();
                            dataProvider.planAborted(action);
                        }
                    }
                    else
                    {
                        fsm.pushState(moveToState);
                    }
                }
                else
                {
                    fsm.pushState(staticToState);
                }
            }
            else
            {
                fsm.popState();
                fsm.pushState(idleState);
                dataProvider.actionsFinished();
            }
        };
    }

    private void createStaticToState()
    {
        staticToState = (fsm, gameObject) =>
        {
            GoapAction action = currentActions.Peek();
            if (action.requiresInRange() && action.target == null)
            {
                fsm.popState();
                fsm.popState();
                fsm.pushState(moveToState);
                return;

            }
            if(dataProvider.staticAgent(action))
            {
                fsm.popState();
            }
        };
    }
   

    private void createMoveToState()
    {
        moveToState = (fsm, gameObj) =>
        {
            //float distanceToTarget = Vector3.Distance(currentActions.Peek().target.transform.position, this.transform.position);
            GoapAction action = currentActions.Peek();
            if (action.requiresInRange() && action.target == null)
            {
                fsm.popState();
                fsm.popState();
                fsm.pushState(idleState);
                return;
            }

            if (dataProvider.moveAgent(action))
            {
                fsm.popState();
            }

        };
    }

    private void findDataProvider()
    {
        foreach (Component comp in gameObject.GetComponents(typeof(Component)))
        {
            if (typeof(IGoap).IsAssignableFrom(comp.GetType()))
            {
                dataProvider = (IGoap)comp;
                return;
            }
        }
    }



    private void loadActions()
    {
        GoapAction[] actions = gameObject.GetComponents<GoapAction>();
        foreach(GoapAction a in actions)
        {
            availableAction.Add(a);
        }
        Debug.Log("Found actions: " + prettyPrint(actions));
    }


    public static string prettyPrint(HashSet<KeyValuePair<string,object>> state)
    {
        string s = "";
        foreach(KeyValuePair<string,object>kvp in state)
        {
            s += kvp + ":" + kvp.Value.ToString();
            s += ",";

        }
        return s;
    }

    public static string prettyPrint(Queue<GoapAction> actions)
    {
        String s = "";
        foreach (GoapAction a in actions)
        {
            s += a.GetType().Name;
            s += "-> ";
        }
        s += "GOAL";
        return s;
    }

    public static string prettyPrint(GoapAction[] actions)
    {
        String s = "";
        foreach (GoapAction a in actions)
        {
            s += a.GetType().Name;
            s += ", ";
        }
        return s;
    }

    public static string prettyPrint(GoapAction action)
    {
        String s = "" + action.GetType().Name;
        return s;
    }

}
