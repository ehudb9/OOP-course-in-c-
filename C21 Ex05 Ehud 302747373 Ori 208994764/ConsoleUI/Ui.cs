﻿using LogicGame;

namespace WindowUI
{
    public class Ui
    {
        private readonly GameSettingForm r_gameSettingForm;
        private BoardGameForm m_boardGameForm;

        public Ui()
        {
            r_gameSettingForm = new GameSettingForm();
        }

        public void RunGame()
        {
            r_gameSettingForm.ShowDialog();
            if(r_gameSettingForm.StartButtonSelected)
            {
                buildGameFromSetting();
                m_boardGameForm.ShowDialog();
            }
        }

        private void buildGameFromSetting()
        {
            int userSelectedBoardRowsNumber = r_gameSettingForm.RowsUpDown;
            int userSelectedBoardColsNumber = r_gameSettingForm.ColumnsUpDown;
            string name2;
            eGameMode userChoiceGameMode;
            GameRunner gameRunner = new GameRunner();
            if (!r_gameSettingForm.Player2CheckBox)
            {
                userChoiceGameMode = eGameMode.PlayerVsComputer;
                name2 = GameSettingForm.k_ComputerName + ":";

            }
            else
            {
                userChoiceGameMode = eGameMode.PlayerVsPlayer;
                name2 = $"{r_gameSettingForm.Player2TextBox}:";
            }

            gameRunner.InitGame(userSelectedBoardRowsNumber, userSelectedBoardColsNumber, userChoiceGameMode);
            string name1 = $"{r_gameSettingForm.Player1TextBox}:"; 
            m_boardGameForm = new BoardGameForm(name1, name2, gameRunner, userChoiceGameMode);
        }
    }
}
