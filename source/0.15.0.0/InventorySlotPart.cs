﻿using SevenDaysProfileEditor.Data;
using SevenDaysSaveManipulator.GameData;
using System.Drawing;
using System.Windows.Forms;

namespace SevenDaysProfileEditor.Inventory
{
    internal class InventorySlotPart : InventorySlotBase
    {
        private InventorySlotItem parent;

        public InventorySlotPart(ItemBinder itemBinder, InventorySlotItem parent, int partIndex, int textBoxWidth, int labeledControlWidth)
            : base(itemBinder, textBoxWidth, labeledControlWidth)
        {
            this.parent = parent;

            Size = new Size(326, 104);

            imageLabel = new Label();
            imageLabel.Anchor = AnchorStyles.Left;
            imageLabel.Size = new Size(IconData.ICON_WIDTH, IconData.ICON_HEIGHT);

            SetImage();

            Controls.Add(imageLabel, 0, 0);

            itemCore = new TableLayoutPanel();
            itemCore.Anchor = AnchorStyles.Right;
            itemCore.Size = new Size(200, 100);

            CreateSelector(new string[] { parent.itemBinder.partNames[partIndex], "air" });
            selector.DropDownStyle = ComboBoxStyle.DropDownList;
            itemCore.Controls.Add(selector, 0, 0);

            basicInfo = GenerateBasicInfo();
            itemCore.Controls.Add(basicInfo, 0, 1);

            Controls.Add(itemCore, 1, 0);
        }

        protected override void SelectionChangedAction()
        {
            if (parent.itemBinder.HasAllParts())
            {
                parent.qualityBox.Text = parent.itemBinder.GetQualityFromParts().ToString();
                parent.itemBinder.meta.Set(parent.itemBinder.magazineSize);
                parent.magazineBox.Enabled = true;
            }
            else
            {
                parent.qualityBox.Text = "";
                parent.itemBinder.meta.Set(0);
                parent.magazineBox.Text = "";
                parent.magazineBox.Enabled = false;
            }
        }

        public override void ValueUpdated(Value<int> source)
        {
            if (source.Equals(itemBinder.quality))
            {
                degradationBox.UpdateMax(itemBinder.GetMaxDegradationForQuality());

                parent.qualityBox.Text = parent.itemBinder.GetQualityFromParts().ToString();
            }
        }
    }
}