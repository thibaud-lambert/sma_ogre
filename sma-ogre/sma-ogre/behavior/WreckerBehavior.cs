﻿using sma_ogre.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sma_ogre.behavior
{
    class WreckerBehavior : Behavior
    {
        static float pickUpDistance = 20;
        static float dropDistance = 20;

        private List<Item> listItem;
        private Item carriedItem;

        static float minWreckerTime = 2;
        static float maxWreckerTime = 10;
        private Timer wreckerTimer;

        public override void Init()
        {
            base.Init();
            this.ChooseTargetPosition();
        }

        public override void ChooseTargetPosition()
        {
            base.ChooseTargetPosition();
            speed = WorldConfig.Singleton.RandFloat(WorldConfig.Singleton.WreckerSpeedRange[0],
                                                    WorldConfig.Singleton.WreckerSpeedRange[1]);
        }

        protected bool WreckerAction(float elapsedTime)
        {
            if (carriedItem == null)
            {
                foreach (Item i in listItem)
                {
                    if (i.Distance(mAgentNode.Position.x, mAgentNode.Position.z) < pickUpDistance)
                    {
                        carriedItem = i;
                        carriedItem.PickUp();
                        listItem.Remove(carriedItem);
                        return true;
                    }
                }
            }
            else
            {
                bool isEmptySpace = true;
                foreach (Item i in listItem)
                {
                    if (i.Distance(mAgentNode.Position.x, mAgentNode.Position.z) < dropDistance)
                    {
                        isEmptySpace = false;

                    }
                }

                if (isEmptySpace)
                {
                    carriedItem.Drop(mAgentNode.Position.x, mAgentNode.Position.z);
                    listItem.Add(carriedItem);
                    carriedItem = null;
                    return true;
                }
            }
            return false;
        }

        public override void Update(float elapsedTime, AgentAnimation animation = null)
        {
            MoveToTargetPosition(elapsedTime);

            if (wreckerTimer.isFinished())
            {
                if (WreckerAction(elapsedTime))
                {
                    wreckerTimer.init();
                }
            }
            else
            {
                wreckerTimer.updateTimer(elapsedTime);
            }

            if (animation != null)
            {
                animation.UpdatePosture(elapsedTime, speed);
            }
        }

        public WreckerBehavior(List<Item> listItem)
        {
            this.listItem = listItem;
            carriedItem = null;
            wreckerTimer = new Timer(minWreckerTime, maxWreckerTime);
            wreckerTimer.init();
        }
    }
}
