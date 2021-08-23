﻿using C21_Ex02.ConsoleUI;
using C21_Ex02.LogicGame;
using System;

namespace C21_Ex02
{
    public class GameRunner
    {
        private eGameMode m_PlayerVsComputerMode = eGameMode.PlayerVsPlayer;
        private bool v_GameIsAlive = false;
        private bool v_PlayerWantsToQuitGame = false;
        private bool m_Turn = true;
        private const int k_SignPlayer1 = 1;
        private const int k_SignPlayer2 = 2;
        private Player m_PlayerOne = null, m_PlayerTwo = null;
        private ComputerPlayer m_ComputerPlayer = null;
        public int m_SizeOfColumns = 0;
        public int m_SizeOfRows = 0;
        private Board m_GameBoard = null;
        private eCellTokenValue m_CurrentPlayer = eCellTokenValue.Empty;

        public GameRunner()
        {
            v_GameIsAlive = true;
            InitGame();
        }
        public void InitGame()
        {
            Prints.WelcomeMessage();
            Console.WriteLine("Please type size of columns (8-4):");
            while (!(int.TryParse(Console.ReadLine(),out m_SizeOfColumns) && m_SizeOfColumns >= 4 && m_SizeOfColumns <= 8))
            {
                Prints.ErrorSizeMessage();
            }
            
            Console.WriteLine("Please type size of rows (8-4):");
            while (!(int.TryParse(Console.ReadLine(), out m_SizeOfRows) && m_SizeOfRows >= 4 && m_SizeOfRows <= 8))
            {
                Prints.ErrorSizeMessage();
            }

            m_GameBoard = new Board(m_SizeOfColumns, m_SizeOfRows);
            m_PlayerOne = new Player(k_SignPlayer1);
            Prints.ComputerOrPlayerMessage();
            if(Console.ReadLine().Equals("y"))
            {
                m_PlayerVsComputerMode = eGameMode.PlayerVsComputer;
                m_ComputerPlayer = new ComputerPlayer(k_SignPlayer2, m_SizeOfColumns);
            }
            else
            {
                m_PlayerTwo = new Player(k_SignPlayer2);
            }

            Prints.StartMessageQToExit();
        }

        public void ResetGame()
        {
            m_GameBoard.ResetBoard();
            v_GameIsAlive = true;
            v_PlayerWantsToQuitGame = false;
            Prints.StartMessageQToExit();
        }

        public void Run()
        {
            while (v_GameIsAlive)
            {
                ShowBoardUI.ShowBoard(m_GameBoard);
                if(m_Turn)
                {
                    m_CurrentPlayer = eCellTokenValue.Player1;
                    Prints.Player1PlayNowMessage();
                    PlayerMove();

                }
                else if (m_PlayerVsComputerMode == eGameMode.PlayerVsComputer)
                {
                    m_CurrentPlayer = eCellTokenValue.Player2;
                    m_ComputerPlayer.MakeComputerMove(m_GameBoard);
                }
                else
                {
                    m_CurrentPlayer = eCellTokenValue.Player2;
                    Prints.Player2PlayNowMessage();
                    PlayerMove();
                }
                
                ShowBoardUI.ShowBoard(m_GameBoard);
                
                if (v_PlayerWantsToQuitGame)
                {
                    Console.WriteLine("{0} Won!!!", m_CurrentPlayer == eCellTokenValue.Player1 ? eCellTokenValue.Player2 : eCellTokenValue.Player1);
                    printCurrentScore();
                    endGame();
                }
                else if (m_GameBoard.HasWon(m_CurrentPlayer))
                {
                    Console.WriteLine("{0} Won!!!", m_CurrentPlayer);
                    if(m_CurrentPlayer == eCellTokenValue.Player1)
                    {
                        m_PlayerOne.Score++;
                    }
                    else
                    {
                        if(m_PlayerVsComputerMode == eGameMode.PlayerVsPlayer)
                        {
                            m_PlayerTwo.Score++;
                        }
                        else
                        {
                            m_ComputerPlayer.Score++;
                        }
                    }

                    printCurrentScore();
                    endGame();
                }
                else if (m_GameBoard.BoardIsFull())
                {
                    Prints.ItsATie();
                    printCurrentScore();
                    endGame();
                }

                m_Turn = !m_Turn;
            }

            Prints.ExitGameMessage();
            Console.ReadLine();
        }

        public void PlayerMove()
        {
            Prints.ChooseColumn();
            string chosenColumnStr = Console.ReadLine();
            bool isValidUserInput = false;
            bool isRowDigit = false;
            int numOfColumnToInsert = 0;
            if (isPlayerWantsToQuit(chosenColumnStr))
            {
                isValidUserInput = true;
            }
            while (!isValidUserInput)
            {
                if(string.IsNullOrEmpty(chosenColumnStr))
                {
                    Console.WriteLine("Please enter non-empty number");
                    chosenColumnStr = Console.ReadLine();
                    if (isPlayerWantsToQuit(chosenColumnStr))
                    {
                        break;
                    }
                }
                else if(chosenColumnStr.Length < 2)
                {
                    isRowDigit = char.IsDigit(char.Parse(chosenColumnStr));
                    
                    if (isRowDigit)
                    {
                        if(int.TryParse(chosenColumnStr, out numOfColumnToInsert))
                        {
                            if(!IsValidColumn(numOfColumnToInsert))
                            {
                                if (m_GameBoard.IsFullColumn(numOfColumnToInsert))
                                {
                                    Prints.ColumnIsFullMessage();
                                }
                                else
                                {
                                    Prints.ErrorSizeMessage();
                                }
                                Prints.ChooseColumn();
                                chosenColumnStr = Console.ReadLine();
                            }
                            else
                            {
                                m_GameBoard.InsertCellToBoard(numOfColumnToInsert, m_CurrentPlayer);
                                isValidUserInput = true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("There was an error with your input. Please try again");
                            Prints.ChooseColumn();
                            chosenColumnStr = Console.ReadLine();
                        }
                    }
                    else
                    {
                        Prints.InvalidColumnNumberErrorMessage(); 
                        Prints.ChooseColumn();
                        chosenColumnStr = Console.ReadLine();
                        if (isPlayerWantsToQuit(chosenColumnStr))
                        {
                            break;
                        }
                    }
                }
                else
                {
                    Prints.InvalidColumnNumberErrorMessage();
                    Prints.ChooseColumn();
                    chosenColumnStr = Console.ReadLine();
                    if (isPlayerWantsToQuit(chosenColumnStr))
                    {
                        break;
                    }
                }
            }
        }

        public bool IsValidColumn(int i_ChosenColumn)
        {
            return i_ChosenColumn > 0  && i_ChosenColumn <= m_SizeOfColumns && !m_GameBoard.IsFullColumn(i_ChosenColumn);
        }

        private void endGame()
        {
            bool isValidAnswer = false;
            while (!isValidAnswer)
            {
                Prints.RestarttGameOfferMessage();
                string userAnswer = Console.ReadLine();
                if (userAnswer.Equals("y"))
                {
                    ResetGame();
                    isValidAnswer = true;
                }
                else if (userAnswer.Equals("Q"))
                {
                    v_GameIsAlive = false;
                    isValidAnswer = true;
                }
            }
        }

        private void printCurrentScore()
        {
            if (m_PlayerVsComputerMode == eGameMode.PlayerVsPlayer)
            {
                Console.WriteLine("current score is : \n\tplayer 1: {0}\n\tplayer 2: {1}\n", m_PlayerOne.Score, m_PlayerTwo.Score);
            }
            else
            {
                Console.WriteLine("current score is : \n\tplayer: {0}\n\tcomputer: {1}\n", m_PlayerOne.Score, m_ComputerPlayer.Score);
            }
        }

        private void scoreAfterPlayerWantsToQuit()
        {
            if (m_CurrentPlayer == eCellTokenValue.Player1)
            {
                if (m_PlayerVsComputerMode == eGameMode.PlayerVsComputer)
                {
                    m_ComputerPlayer.Score++;
                }
                else
                {
                    m_PlayerTwo.Score++;
                }
            }
            else
            {
                m_PlayerOne.Score++;
            }
        }

        private bool isPlayerWantsToQuit(string i_ChosenColumnStr)
        {
            bool v_isPlayerWantsToQuit = false;
            
            if (i_ChosenColumnStr.Equals("Q"))
            {
                v_isPlayerWantsToQuit = true;
                scoreAfterPlayerWantsToQuit();
                v_GameIsAlive = false;
                v_PlayerWantsToQuitGame = true;
                printCurrentScore();
            }

            return v_isPlayerWantsToQuit;
        }
    }
}
