﻿using SevenDaysSaveManipulator.GameData;
using System.Collections.Generic;

namespace SevenDaysProfileEditor.Quests
{
    class BinderQuest
    {
        public DataQuest dataQuest;
        public Quest quest;

        public string id;
        public string categoryKey;
        public List<BinderObjective> objectives;

        public Value<QuestState> currentState;
        public Value<bool> isTracked;
        public Value<ulong> finishTime;
        public Dictionary<string, string> dataVariables;

        public BinderQuest(Quest quest)
        {
            this.quest = quest;
            this.dataQuest = DataQuest.getQuestById(quest.id);

            currentState = quest.currentState;
            isTracked = quest.isTracked;
            finishTime = quest.finishTime;
            dataVariables = quest.dataVariables;

            id = dataQuest.id;
            categoryKey = dataQuest.categoryKey;
            objectives = new List<BinderObjective>();

            for (int i = 0; i < dataQuest.objectives.Count; i++)
            {
                objectives.Add(new BinderObjective(dataQuest.objectives[i], quest.objectives[i]));
            }
        }
    }
}
