﻿using System.Windows.Forms;
using System;

namespace WindowUI
{
    public class ColumnNumberButton : Button
    {
        private int m_ColumnNumberValue;

        public int ColumnNumberValue
        {
            get => m_ColumnNumberValue;
            set
            {
                m_ColumnNumberValue = value;
            }
        }

        public ColumnNumberButton(int i_Column)
        {
            Anchor = AnchorStyles.Left;
            Anchor = AnchorStyles.Top;
            m_ColumnNumberValue = i_Column;
            Text = m_ColumnNumberValue.ToString();
            Width = 30;
            Height = 20;
        }

        public void OnClickOccurred(int i_Column, bool i_IsColumnIsFull)
        {
            if (i_Column == m_ColumnNumberValue && i_IsColumnIsFull)
            {
                Enabled = false;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ColumnNumberButton
            // 
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Click += new System.EventHandler(this.ColumnNumberButton_Click);
            this.ResumeLayout(false);

        }

        private void ColumnNumberButton_Click(object sender, System.EventArgs e)
        {
            Console.WriteLine(m_ColumnNumberValue.ToString());
        }
    }
}
