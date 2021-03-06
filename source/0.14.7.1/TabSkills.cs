﻿using SevenDaysProfileEditor.GUI;
using SevenDaysSaveManipulator.GameData;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Skills
{
    class TabSkills : TabPage
    {
        private PlayerDataFile playerDataFile;       
        private TableLayoutPanel panel;
        private List<BinderSkill> playerListeners = new List<BinderSkill>();
        private CollectionSkill skillList;

        public TabSkills(PlayerDataFile playerDataFile) : base()
        {
            this.playerDataFile = playerDataFile;
            Text = "Skills";

            skillList = new CollectionSkill(playerDataFile.skills.skillDictionary, playerDataFile.level, playerListeners);

            panel = new TableLayoutPanel();

            panel.Dock = DockStyle.Fill;

            TableLayoutPanel general = new TableLayoutPanel();
            general.Dock = DockStyle.Fill;

            LabeledBox playerLevelBox = new LabeledBox("Player level", new TextBoxPlayerLevel(playerDataFile.level, 1, DataSkill.maxPlayerLevel, playerListeners));
            general.Controls.Add(playerLevelBox);

            LabeledBox skillPointsBox = new LabeledBox("Skill points", new TextBoxInt(playerDataFile.skillPoints, 0, 10000));
            general.Controls.Add(skillPointsBox);

            panel.Controls.Add(general);

            TableLayoutPanel skillsPanel = new TableLayoutPanel();
            skillsPanel.Dock = DockStyle.Fill;
            skillsPanel.AutoScroll = true;

            List<SlotSkill> skillSlotList = new List<SlotSkill>();

            foreach (BinderSkill binderSkill in skillList.skillList)
            {
                SlotSkill skillSlot = new SlotSkill(binderSkill);

                skillSlotList.Add(skillSlot);               
            }

            int count = skillSlotList.Count;
            int perColumn = count / 7;
            int extraColumns = count % 7;
            int done = 0;

            for (int i = 0; i < 7; i++)
            {
                int thisColumn = perColumn;

                if (extraColumns > 0)
                {
                    thisColumn++;
                    extraColumns--;
                }

                for (int j = 0; j < thisColumn; j++)
                {
                    skillsPanel.Controls.Add(skillSlotList[done], i, j);
                    done++;
                }
            }

            panel.Controls.Add(skillsPanel, 0, 1);

            Controls.Add(panel);
        }
    }
}
