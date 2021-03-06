﻿using SevenDaysSaveManipulator.GameData;
using System.Collections.Generic;

namespace SevenDaysProfileEditor.Quests
{
    class BinderObjective
    {
        public DataObjective dataObjective;
        public BaseObjective baseObjective;

        public string name;
        public int minValue;
        public int maxValue;

        public Value<byte> currentValue;

        public BinderObjective(DataObjective dataObjective, BaseObjective baseObjective)
        {
            this.dataObjective = dataObjective;
            this.baseObjective = baseObjective;

            name = dataObjective.name;
            minValue = dataObjective.minValue;
            maxValue = dataObjective.maxValue;

            currentValue = baseObjective.currentValue;
        }
    }
}
