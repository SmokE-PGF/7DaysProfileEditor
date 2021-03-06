﻿using SevenDaysProfileEditor.GUI;
using SevenDaysProfileEditor.Inventory;
using SevenDaysSaveManipulator.GameData;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Inventory
{
    class SlotInventory : TableLayoutPanel
    {
        public TextBoxQuality qualityBox;
        public TextBoxDegradation degradationBox;
        public string selectedItem;

        private BinderItem itemBinder;
        private ComboBox itemType;
        //private ComboBox magazineItems;
        private ComboBoxToPopulate<string> magazineItems;
        private LabeledBox countBox;
        private LabeledBox labeledQualityBox;
        private LabeledBox labeledDegradationBox;
        private LabeledBox magazineBox;
        private LabeledBox magazineComboBox;
        private Button parts;
        private string[] list;        

        private void nonKeyEvent(object sender, EventArgs e)
        {
            itemType.Text = itemType.SelectedItem.ToString();

            if ((itemType.SelectedItem != null) && !selectedItem.Equals(itemType.SelectedItem.ToString()))
            {
                selectedItem = itemType.SelectedItem.ToString();
                resetSlot();
            }

            else
            {
                itemType.SelectedItem = selectedItem;
            }
        }

        public void resetSlot()
        {
            itemBinder.update(DataItem.getItemDataByName(selectedItem).id);

            int controlNumber = Controls.Count;
            for (int i = 1; i < controlNumber; i++)
            {
                Controls.RemoveAt(1);
            }

            generateSlot();

            Invalidate();
        }

        private void generateSlot()
        {
            if (!itemBinder.name.Equals("air"))
            {
                if (itemBinder.stackNumber > 1)
                {
                    countBox = new LabeledBox("Count", new TextBoxInt(itemBinder.count, 1, itemBinder.stackNumber));
                    Controls.Add(countBox);
                }

                if (itemBinder.hasQuality)
                {
                    int degradation = itemBinder.getDegradation();

                    degradationBox = new TextBoxDegradation(itemBinder.useTimes, 1, degradation, itemBinder);
                    labeledDegradationBox = new LabeledBox("Durability", degradationBox);

                    qualityBox = new TextBoxQuality(itemBinder.quality, 0, DataItem.MAX_QUALITY, degradationBox);
                    labeledQualityBox = new LabeledBox("Quality", qualityBox);

                    if (itemBinder.parts.Length > 0)
                    {
                        qualityBox.Enabled = false;
                        degradationBox.Enabled = false;

                        if (itemBinder.hasAllParts())
                        {
                            degradationBox.Text = "";
                        }

                        else
                        {
                            qualityBox.Text = 0.ToString();
                            degradationBox.Text = 0.ToString();
                        }
                    }

                    Controls.Add(labeledQualityBox);
                    Controls.Add(labeledDegradationBox);

                    if (itemBinder.magazineSize > 0)
                    {
                        magazineBox = new LabeledBox("Magazine", new TextBoxInt(itemBinder.meta, 0, itemBinder.magazineSize));
                        Controls.Add(magazineBox);
                    }

                    if (itemBinder.magazineItems != null && itemBinder.magazineItems.Length > 1)
                    {
                        magazineItems = new ComboBoxToPopulate<string>(itemBinder.magazineItems, itemBinder.selectedAmmoTypeIndex.get());
                        magazineItems.DropDownStyle = ComboBoxStyle.DropDownList;
                        magazineItems.DropDownClosed += (sender, e) =>
                        {
                            itemBinder.selectedAmmoTypeIndex.set((byte)magazineItems.SelectedIndex);
                        };

                        magazineComboBox = new LabeledBox("Selected ammo", magazineItems);

                        Controls.Add(magazineComboBox);
                    }

                    if (itemBinder.itemValue.parts.Length > 0)
                    {
                        parts = new Button();
                        parts.Text = "Parts";

                        parts.Click += (sender, e) =>
                        {
                            new WindowParts(itemBinder, this);
                        };

                        Controls.Add(parts);
                    }
                }
            }
        }

        public SlotInventory(BinderItem itemBinder, string[] comboBoxList)
        {
            this.itemBinder = itemBinder;
            list = comboBoxList;
            selectedItem = itemBinder.name;
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            Width = 230;
            Height = 160;

            itemType = new ComboBoxToPopulate<string>(list, Array.IndexOf(list, itemBinder.name));

            itemType.Width = 220;
            itemType.DropDownStyle = ComboBoxStyle.DropDown;
            itemType.AutoCompleteMode = AutoCompleteMode.SuggestAppend;           

            itemType.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    nonKeyEvent(sender, e);
                }
            };

            itemType.DropDownClosed += new EventHandler(nonKeyEvent);
            itemType.LostFocus += new EventHandler(nonKeyEvent);

            Controls.Add(itemType, 0, 0);

            generateSlot();
        }
    }
}
